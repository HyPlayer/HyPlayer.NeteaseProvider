using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.ApiContracts.Album;
using HyPlayer.NeteaseApi.ApiContracts.Artist;
using HyPlayer.NeteaseApi.ApiContracts.Cloud;
using HyPlayer.NeteaseApi.ApiContracts.Comment;
using HyPlayer.NeteaseApi.ApiContracts.Login;
using HyPlayer.NeteaseApi.ApiContracts.ListenTogether;
using HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Dual;
using HyPlayer.NeteaseApi.ApiContracts.PersonalFM;
using HyPlayer.NeteaseApi.ApiContracts.Playlist;
using HyPlayer.NeteaseApi.ApiContracts.Recommend;
using HyPlayer.NeteaseApi.ApiContracts.Song;
using HyPlayer.NeteaseApi.ApiContracts.User;
using HyPlayer.NeteaseApi.ApiContracts.Utils;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Models;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.NeteaseProvider.Models;
using HyPlayer.PlayCore.Abstraction.Interfaces.PlayListContainer;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.Resources;
using HyPlayer.PlayCore.Abstraction.Models.SingleItems;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace HyPlayer.NeteaseProvider;

public partial class NeteaseProvider
{
    private const string CloudAudioBucket = "jd-musicrep-privatecloud-audio-public";
    private const string CloudCoverBucket = "yyimgs";
    private static readonly HttpClient CloudUploadHttpClient = new();

    private static readonly string[] DefaultPlaylistCategories =
    [
        "华语",
        "流行",
        "摇滚",
        "民谣",
        "电子",
        "轻音乐",
        "ACG",
        "学习",
        "工作",
        "治愈"
    ];

    public async Task<ProviderSessionInfo> LoginAsync(string accountId, string secret, CancellationToken ctk = default)
    {
        var loggedIn = accountId.Contains('@')
            ? await LoginEmailAsync(accountId, secret, ctk: ctk)
            : await LoginCellphoneAsync(accountId, secret, ctk: ctk);

        if (!loggedIn)
            return new ProviderSessionInfo { IsAuthenticated = false };

        return await GetSessionInfoAsync(ctk);
    }

    public Task LogoutAsync(CancellationToken ctk = default)
    {
        Handler.Option.Cookies.Clear();
        LoginedUser = null;
        return Task.CompletedTask;
    }

    public async Task<ProviderSessionInfo> GetSessionInfoAsync(CancellationToken ctk = default)
    {
        var result = await RequestAsync(NeteaseApis.LoginStatusApi, new LoginStatusRequest(), ctk);
        return result.Match(
            success =>
            {
                LoginedUser = success.Profile?.MapToNeteaseUser();
                return CreateSessionInfo(LoginedUser);
            },
            _ => CreateSessionInfo(LoginedUser));
    }

    public Task ImportSessionAsync(IReadOnlyDictionary<string, string> sessionValues, CancellationToken ctk = default)
    {
        Handler.Option.Cookies.Clear();
        foreach (var sessionValue in sessionValues)
            Handler.Option.Cookies[sessionValue.Key] = sessionValue.Value;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyDictionary<string, string>> ExportSessionAsync(CancellationToken ctk = default)
    {
        return Task.FromResult<IReadOnlyDictionary<string, string>>(
            Handler.Option.Cookies.ToDictionary(pair => pair.Key, pair => pair.Value));
    }

    public async Task AnnounceDeviceAsync(string deviceName, CancellationToken ctk = default)
    {
        await RequestAsync(NeteaseApis.LoginAnnounceDeviceApi,
            new LoginAnnounceDeviceRequest { DeviceName = deviceName }, ctk);
    }

    public async Task<ProviderQrLoginChallenge> CreateQrLoginChallengeAsync(CancellationToken ctk = default)
    {
        var result = await RequestAsync(NeteaseApis.LoginQrCodeUnikeyApi, new LoginQrCodeUnikeyRequest(), ctk);
        return result.Match(success => new ProviderQrLoginChallenge
            {
                ChallengeId = success.Unikey ?? string.Empty,
                Uri = new Uri($"https://music.163.com/login?codekey={Uri.EscapeDataString(success.Unikey ?? string.Empty)}")
            },
            error => throw error);
    }

    public async Task<ProviderQrLoginState> GetQrLoginStateAsync(string challengeId, CancellationToken ctk = default)
    {
        var result = await RequestAsync(NeteaseApis.LoginQrCodeCheckApi,
            new LoginQrCodeCheckRequest { Unikey = challengeId }, ctk);
        return result.Match(success => new ProviderQrLoginState
            {
                Status = MapQrStatus(success.Code),
                SessionInfo = success.Code is 200 or 803 ? CreateSessionInfo(LoginedUser) : null,
                Message = success.Message
            },
            error => throw error);
    }

    public async Task<ContainerBase> CreateContainerAsync(string name, bool isPrivate, CancellationToken ctk = default)
    {
        await RequestAsync(NeteaseApis.PlaylistCreateApi,
            new PlaylistCreateRequest { Name = name, Privacy = isPrivate ? 10 : 0 }, ctk);

        var userId = LoginedUser?.ActualId ?? (await GetSessionInfoAsync(ctk)).UserId;
        if (userId is not null)
        {
            var containers = await GetUserContainersAsync(userId, 0, 50, ctk);
            var created = containers.Items.OfType<NeteasePlaylist>().FirstOrDefault(playlist => playlist.Name == name);
            if (created is not null) return created;
        }

        return new NeteasePlaylist { Name = name, ActualId = null };
    }

    public async Task DeleteContainerAsync(string containerId, CancellationToken ctk = default)
    {
        await RequestAsync(NeteaseApis.PlaylistDeleteApi, new PlaylistDeleteRequest { Id = containerId }, ctk);
    }

    public async Task SetContainerPrivacyAsync(string containerId, bool isPrivate, CancellationToken ctk = default)
    {
        if (!isPrivate)
            throw new NotSupportedException("NetEase only exposes conversion to a private playlist through the current API contract.");

        await RequestAsync(NeteaseApis.PlaylistPrivacyApi, new PlaylistPrivacyRequest { Id = containerId }, ctk);
    }

    public async Task SubscribeContainerAsync(string containerId, CancellationToken ctk = default)
    {
        await RequestAsync(NeteaseApis.PlaylistSubscribeApi,
            new PlaylistSubscribeRequest { PlaylistId = containerId, IsSubscribe = true }, ctk);
    }

    public async Task UnsubscribeContainerAsync(string containerId, CancellationToken ctk = default)
    {
        await RequestAsync(NeteaseApis.PlaylistSubscribeApi,
            new PlaylistSubscribeRequest { PlaylistId = containerId, IsSubscribe = false }, ctk);
    }

    public Task<ContainerBase?> GetCommentContainerAsync(CancellationToken ctk = default)
    {
        return Task.FromResult<ContainerBase?>(null);
    }

    public async Task<ProviderPageResult<CommentBase>> GetCommentsAsync(string itemId, string typeId, int offset, int count, CancellationToken ctk = default)
    {
        var pageNo = count <= 0 ? 1 : (offset / count) + 1;
        var result = await RequestAsync(NeteaseApis.CommentsApi,
            new CommentsRequest
            {
                ResourceId = itemId,
                ResourceType = TypeIdToSearchIdMapper.MapToResourceId(typeId),
                PageNo = pageNo,
                PageSize = count,
                CommentSortType = CommentSortType.Recommend
            }, ctk);

        return result.Match(success => new ProviderPageResult<CommentBase>
            {
                Items = success.Data?.Comments?.Select(comment => (CommentBase)comment.MapToNeteaseComment()).ToList() ?? new List<CommentBase>(),
                HasMore = success.Data?.HasMore is true,
                NextOffset = success.Data?.HasMore is true ? offset + count : null,
                TotalCount = success.Data is null ? null : (int?)success.Data.TotalCount
            },
            _ => EmptyPage<CommentBase>());
    }

    public Task<CommentBase?> PostCommentAsync(string itemId, string typeId, string content, string? replyToCommentId = null, CancellationToken ctk = default)
    {
        throw new NotSupportedException("The current NetEase API contracts do not include a comment posting endpoint.");
    }

    public async Task SetCommentLikeStateAsync(string itemId, string typeId, string commentId, bool like, CancellationToken ctk = default)
    {
        await RequestAsync(NeteaseApis.CommentLikeApi,
            new CommentLikeRequest
            {
                CommentId = commentId,
                IsLike = like,
                ResourceType = TypeIdToSearchIdMapper.MapToResourceId(typeId),
                ThreadId = $"{NeteaseApi.Extensions.NeteaseUtils.CommentTypeToThreadPrefix(TypeIdToSearchIdMapper.MapToResourceId(typeId))}{itemId}"
            }, ctk);
    }

    public async Task<ProviderPageResult<CommentBase>> GetThreadedCommentsAsync(string itemId, string typeId, string commentId, int offset, int count, CancellationToken ctk = default)
    {
        var result = await RequestAsync(NeteaseApis.CommentFloorApi,
            new CommentFloorRequest
            {
                ResourceId = itemId,
                ResourceType = TypeIdToSearchIdMapper.MapToResourceId(typeId),
                ParentCommentId = commentId,
                Limit = count
            }, ctk);

        return result.Match(success => new ProviderPageResult<CommentBase>
            {
                Items = success.Data?.Comments?.Select(comment => (CommentBase)comment.MapToNeteaseComment()).ToList() ?? new List<CommentBase>(),
                HasMore = success.Data?.HasMore is true,
                NextOffset = success.Data?.HasMore is true ? offset + count : null,
                TotalCount = success.Data?.TotalCount
            },
            _ => EmptyPage<CommentBase>());
    }

    public async Task<ContainerBase> GetSearchSuggestionsAsync(string keyword, CancellationToken ctk = default)
    {
        var result = await RequestAsync(NeteaseApis.SearchSuggestionApi,
            new SearchSuggestionRequest { Keyword = keyword }, ctk);

        return result.Match(success => new NeteaseActionGettableContainer(() => Task.FromResult(
                success.Result?.AllMatch?.Select<SearchSuggestionResponse.SearchSuggestionResponseResult.SearchSuggestionResponseResultItem, ProvidableItemBase>(item => new NeteaseSearchSuggestion
                {
                    ActualId = item.Keyword,
                    Name = item.Keyword ?? string.Empty,
                    SuggestionType = item.Type,
                    Algorithm = item.Algorithm,
                    LastKeyword = item.LastKeyword,
                    Feature = item.Feature
                }).ToList() ?? new List<ProvidableItemBase>()))
            {
                ActualId = keyword,
                Name = "搜索建议"
            } as ContainerBase,
            error => throw error);
    }

    public async Task<ProviderPageResult<ProvidableItemBase>> GetContainerItemsPageAsync(string containerId, int offset, int count, CancellationToken ctk = default)
    {
        var playlist = await GetPlaylistById(containerId, ctk);
        if (playlist is null)
            return EmptyPage<ProvidableItemBase>();

        var result = await playlist.GetProgressiveItemsListAsync(offset, count, ctk);
        return new ProviderPageResult<ProvidableItemBase>
        {
            Items = result.Item2,
            HasMore = result.Item1,
            NextOffset = result.Item1 ? offset + count : null,
            TotalCount = playlist.TrackCount > 0 ? playlist.TrackCount : null
        };
    }

    public Task<List<ProviderCategory>> GetContainerCategoriesAsync(string? typeId = null, CancellationToken ctk = default)
    {
        return Task.FromResult(DefaultPlaylistCategories.Select(category => new ProviderCategory
        {
            Id = category,
            Name = category,
            ParentId = typeId
        }).ToList());
    }

    public async Task<List<SingleSongBase>> GetPersonalFmQueueAsync(CancellationToken ctk = default)
    {
        var result = await RequestAsync(NeteaseApis.PersonalFmApi,
            new PersonalFmRequest { Mode = "DEFAULT" }, ctk);
        return result.Match(
            success => success.Items?.Select(item => (SingleSongBase)item.MapToNeteaseMusic()).ToList() ?? new List<SingleSongBase>(),
            _ => new List<SingleSongBase>());
    }

    public async Task TrashPersonalFmSongAsync(string songId, CancellationToken ctk = default)
    {
        await RequestAsync(NeteaseApis.PersonalFmTrashApi, new FmTrashRequest { Id = songId }, ctk);
    }

    public async Task<ContainerBase> GetPersonalFmContextAsync(string songId, CancellationToken ctk = default)
    {
        return await GetContextRecommendationAsync(songId, NeteaseTypeIds.SingleSong, 10, ctk);
    }

    public Task<ProvidableItemBase?> GetUserAsync(string? userId = null, CancellationToken ctk = default)
    {
        if (userId is null)
            return Task.FromResult<ProvidableItemBase?>(LoginedUser);

        return Task.FromResult<ProvidableItemBase?>(new NeteaseUser { ActualId = userId, Name = userId });
    }

    public async Task<ProviderPageResult<ContainerBase>> GetUserContainersAsync(string? userId, int offset, int count, CancellationToken ctk = default)
    {
        userId ??= LoginedUser?.ActualId;
        if (userId is null)
            return EmptyPage<ContainerBase>();

        var result = await RequestAsync(NeteaseApis.UserPlaylistApi,
            new UserPlaylistRequest { Uid = userId, Offset = offset, Limit = count }, ctk);

        return result.Match(success =>
            {
                var items = success.Playlists?.Select(playlist => (ContainerBase)playlist.MapToNeteasePlaylist()).ToList() ?? new List<ContainerBase>();
                return new ProviderPageResult<ContainerBase>
                {
                    Items = items,
                    HasMore = items.Count == count,
                    NextOffset = items.Count == count ? offset + count : null
                };
            },
            _ => EmptyPage<ContainerBase>());
    }

    public async Task<ProviderPageResult<ProvidableItemBase>> GetUserLibraryItemsAsync(string typeId, int offset, int count, CancellationToken ctk = default)
    {
        if (typeId == NeteaseTypeIds.SingleSong)
        {
            var cloudItems = await GetCloudLibraryItemsAsync(offset, count, ctk);
            return new ProviderPageResult<ProvidableItemBase>
            {
                Items = cloudItems.Items.Cast<ProvidableItemBase>().ToList(),
                HasMore = cloudItems.HasMore,
                NextOffset = cloudItems.NextOffset,
                TotalCount = cloudItems.TotalCount
            };
        }

        return EmptyPage<ProvidableItemBase>();
    }

    public async Task<List<ProvidableItemBase>> GetUserListeningHistoryAsync(string userId, string rangeId, CancellationToken ctk = default)
    {
        var result = await RequestAsync<UserRecordAllResponse, UserRecordRequest, UserRecordResponse, ErrorResultBase, UserRecordActualRequest>(
            NeteaseApis.UserRecordApi,
            new UserRecordRequest
            {
                UserId = userId,
                RecordType = rangeId.Equals("recent", StringComparison.OrdinalIgnoreCase) ? UserRecordType.WeekData : UserRecordType.All,
                Count = 120
            }, ctk);

        return result.Match(
            success => success.AllData?.Select(item => item.Song).Where(song => song is not null).Select(song => (ProvidableItemBase)song!.MapToNeteaseMusic()).ToList() ?? new List<ProvidableItemBase>(),
            _ => new List<ProvidableItemBase>());
    }

    public async Task<ProviderPageResult<CloudLibraryItemBase>> GetCloudLibraryItemsAsync(int offset, int count, CancellationToken ctk = default)
    {
        var result = await RequestAsync(NeteaseApis.CloudGetApi,
            new CloudGetRequest { Offset = offset, Limit = count }, ctk);

        return result.Match(success => new ProviderPageResult<CloudLibraryItemBase>
            {
                Items = success.Songs?.Select(song => (CloudLibraryItemBase)song.MapToNeteaseCloudLibraryItem()).ToList() ?? new List<CloudLibraryItemBase>(),
                HasMore = success.HasMore,
                NextOffset = success.HasMore ? offset + count : null,
                TotalCount = success.Count
            },
            _ => EmptyPage<CloudLibraryItemBase>());
    }

    public async Task DeleteCloudLibraryItemAsync(string itemId, CancellationToken ctk = default)
    {
        await RequestAsync(NeteaseApis.CloudDeleteApi, new CloudDeleteRequest { Id = itemId }, ctk);
    }

    public async Task<CloudLibraryItemBase> UploadCloudLibraryItemAsync(
        ResourceBase resource,
        IReadOnlyDictionary<string, string> metadata,
        CancellationToken ctk = default)
    {
        var streamResult = await resource.GetResourceAsync(ctk: ctk);
        if (streamResult is not IResourceResultOf<Stream> streamResource)
            throw new InvalidOperationException("Cloud upload requires a stream resource.");

        await using var stream = await streamResource.GetResourceAsync(ctk)
            ?? throw new InvalidOperationException("Cloud upload stream is empty.");
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, ctk);
        var bytes = memoryStream.ToArray();
        var md5 = Convert.ToHexString(MD5.HashData(bytes)).ToLowerInvariant();

        var extension = GetMetadata(metadata, "extension", resource.ExtensionName ?? string.Empty).TrimStart('.');
        var bitrate = int.TryParse(GetMetadata(metadata, "bitrate", "0"), out var parsedBitrate) ? parsedBitrate : 0;
        var fileName = GetMetadata(metadata, "fileName", resource.ResourceName ?? $"upload.{extension}");
        var contentType = GetMetadata(metadata, "contentType", "application/octet-stream");
        var title = GetMetadata(metadata, "title", Path.GetFileNameWithoutExtension(fileName));

        var checkResult = await RequestAsync(NeteaseApis.CloudUploadCheckApi,
            new CloudUploadCheckRequest
            {
                Ext = extension,
                Md5 = md5,
                Bitrate = bitrate,
                Length = bytes.LongLength
            }, ctk);
        if (checkResult.IsError)
            throw new InvalidOperationException(checkResult.Error?.Message ?? "Cloud upload check failed.");

        var infoRequest = new CloudUploadInfoRequest
        {
            Md5 = md5,
            SongId = checkResult.Value!.SongId!,
            FileName = fileName,
            Song = title,
            Album = GetMetadata(metadata, "album", string.Empty),
            Artist = GetMetadata(metadata, "artist", string.Empty),
            Bitrate = bitrate
        };

        if (checkResult.Value?.NeedUpload is not false)
        {
            var tokenResult = await RequestAsync(NeteaseApis.CloudUploadTokenAllocApi,
                new CloudUploadTokenAllocRequest { FileName = fileName, Md5 = md5 }, ctk);
            if (tokenResult.IsError)
                throw new InvalidOperationException(tokenResult.Error?.Message ?? "Cloud upload token allocation failed.");

            var objectKey = tokenResult.Value!.Data!.ObjectKey;
            var loadBalancer = await GetUploadLoadBalancerAsync(CloudAudioBucket, ctk);
            var targetLink = $"{loadBalancer}/{CloudAudioBucket}/{objectKey}?version=1.0";
            await UploadToNosAsync(targetLink, new MemoryStream(bytes), md5, tokenResult.Value.Data.Token, contentType, ctk: ctk);
            infoRequest.ResourceId = tokenResult.Value.Data.ResourceId!;
            infoRequest.ObjectKey = $"{CloudAudioBucket}/{objectKey}";
        }

        if (metadata.TryGetValue("coverBase64", out var coverBase64) && !string.IsNullOrWhiteSpace(coverBase64))
            infoRequest.CoverId = await UploadCloudCoverAsync(fileName, Convert.FromBase64String(coverBase64), ctk);

        var infoResult = await RequestAsync(NeteaseApis.CloudUploadInfoApi, infoRequest, ctk);
        if (infoResult.IsError)
            throw new InvalidOperationException(infoResult.Error?.Message ?? "Cloud upload info failed.");

        var pubResult = await RequestAsync(NeteaseApis.CloudPubApi,
            new CloudPubRequest { SongId = infoResult.Value!.SongId! }, ctk);
        if (pubResult.IsError)
            throw new InvalidOperationException(pubResult.Error?.Message ?? "Cloud publish failed.");

        return infoResult.Value.PrivateCloud?.MapToNeteaseCloudLibraryItem()
               ?? new NeteaseCloudLibraryItem
               {
                   ActualId = infoResult.Value.SongId,
                   Name = title,
                   FileName = fileName,
                   FileSize = bytes.LongLength,
                   UploadedAt = DateTimeOffset.Now
               };
    }

    public async Task<ProviderPageResult<ProvidableItemBase>> GetScopedItemsPageAsync(string parentId, string parentTypeId, string itemTypeId, int offset, int count, CancellationToken ctk = default)
    {
        if (parentTypeId == NeteaseTypeIds.Artist)
        {
            var container = new NeteaseArtistSubContainer
            {
                ActualId = $"{MapArtistScopedPrefix(itemTypeId)}{parentId}",
                Name = parentId
            };
            var result = await container.GetProgressiveItemsListAsync(offset, count, ctk);
            return new ProviderPageResult<ProvidableItemBase>
            {
                Items = result.Item2,
                HasMore = result.Item1,
                NextOffset = result.Item1 ? offset + count : null
            };
        }

        if (parentTypeId == NeteaseTypeIds.Playlist && itemTypeId == NeteaseTypeIds.SingleSong)
            return await GetContainerItemsPageAsync(parentId, offset, count, ctk);

        return EmptyPage<ProvidableItemBase>();
    }

    public async Task<ContainerBase> GetContextRecommendationAsync(string itemId, string typeId, int count, CancellationToken ctk = default)
    {
        if (typeId != NeteaseTypeIds.SingleSong)
            return new NeteaseActionGettableContainer { ActualId = itemId, Name = "相关推荐" };

        var result = await RequestAsync(NeteaseApis.PlaymodeIntelligenceListApi,
            new PlaymodeIntelligenceListRequest
            {
                SongId = itemId,
                PlaylistId = string.Empty,
                StartMusicId = itemId,
                Count = count
            }, ctk);

        return result.Match(success => new NeteaseActionGettableContainer(() => Task.FromResult(
                success.Data?.Select(item => item.SongInfo).Where(song => song is not null).Select(song => (ProvidableItemBase)song!.MapToNeteaseMusic()).ToList()
                ?? new List<ProvidableItemBase>()))
            {
                ActualId = itemId,
                Name = "相关推荐"
            } as ContainerBase,
            _ => new NeteaseActionGettableContainer { ActualId = itemId, Name = "相关推荐" });
    }

    public async Task<string> CreateListenTogetherRoomAsync(List<SingleSongBase> queue, CancellationToken ctk = default)
    {
        var result = await RequestAsync(NeteaseApis.ListenTogetherRoomCreateApi, new ListenTogetherRoomCreateRequest(), ctk);
        return result.Match(success => success.Data?.RoomInfo?.RoomId ?? string.Empty, _ => string.Empty);
    }

    public async Task<bool> CanJoinListenTogetherRoomAsync(string roomId, CancellationToken ctk = default)
    {
        var result = await RequestAsync(NeteaseApis.ListenTogetherRoomCheckApi,
            new ListenTogetherRoomCheckRequest { RoomId = roomId }, ctk);
        return result.Match(success => success.Data?.Joinable is true, _ => false);
    }

    public Task SendListenTogetherPlaybackCommandAsync(
        string roomId,
        ProviderListenTogetherPlaybackCommand command,
        CancellationToken ctk = default)
    {
        return RequestAsync(NeteaseApis.ListenTogetherPlayCommandApi,
            new ListenTogetherPlayCommandRequest
            {
                RoomId = roomId,
                CommandType = MapListenTogetherPlaybackCommand(command.CommandId),
                PlayStatus = command.IsPlaying
                    ? ListenTogetherHeartBeatRequest.ListenTogetherPlayStatus.Play
                    : ListenTogetherHeartBeatRequest.ListenTogetherPlayStatus.Pause,
                FormerSongId = command.FormerItemId ?? string.Empty,
                TargetSongId = command.TargetItemId ?? string.Empty,
                ClientSeq = command.ClientSeq,
                Progress = (long)command.Position.TotalMilliseconds
            }, ctk);
    }

    public Task ReportListenTogetherQueueAsync(
        string roomId,
        ProviderListenTogetherQueueReport report,
        CancellationToken ctk = default)
    {
        return RequestAsync(NeteaseApis.ListenTogetherSyncListReportApi,
            new ListenTogetherSyncListReportRequest
            {
                RoomId = roomId,
                CommandType = ListenTogetherSyncListReportRequest.ListenTogetherSyncListReportCommandType.PlayModeChange,
                PlayMode = MapListenTogetherPlayMode(report.PlayModeId),
                UserId = report.UserId ?? LoginedUser?.ActualId ?? string.Empty,
                ClientSeq = report.ClientSeq,
                AnchorPosition = report.AnchorPosition,
                AnchorSongId = report.AnchorItemId ?? string.Empty,
                DisplaySongList = report.Queue.Select(song => song.ActualId ?? string.Empty).Where(id => !string.IsNullOrWhiteSpace(id)).ToArray()
            }, ctk);
    }

    public async Task<ProviderListenTogetherStatus?> GetListenTogetherStatusAsync(string roomId, CancellationToken ctk = default)
    {
        var result = await RequestAsync(NeteaseApis.ListenTogetherStatusApi, new ListenTogetherStatusRequest(), ctk);
        return result.Match(success =>
            {
                var roomInfo = success.Data?.RoomInfo;
                return new ProviderListenTogetherStatus
                {
                    IsInRoom = success.Data?.IsInRoom is true,
                    RoomId = roomInfo?.RoomId,
                    Users = roomInfo?.RoomUsers?.Select(user => new ProviderListenTogetherUser
                    {
                        UserId = user.UserId ?? string.Empty,
                        Nickname = user.Nickname ?? string.Empty,
                        AvatarUrl = user.AvatarUrl ?? string.Empty
                    }).ToList() ?? []
                };
            },
            _ => (ProviderListenTogetherStatus?)null);
    }

    public async Task<ProvidableItemDynamicMetadata> GetDynamicMetadataAsync(string itemId, string typeId, CancellationToken ctk = default)
    {
        if (typeId == NeteaseTypeIds.Album)
        {
            var result = await RequestAsync(NeteaseApis.AlbumDetailDynamicApi, new AlbumDetailDynamicRequest { Id = itemId }, ctk);
            return result.Match(success => new ProvidableItemDynamicMetadata
                {
                    CommentCount = success.CommentCount,
                    ShareCount = success.ShareCount,
                    LikedCount = success.SubCount,
                    IsLiked = success.IsSub
                },
                _ => new ProvidableItemDynamicMetadata());
        }

        if (typeId == NeteaseTypeIds.Playlist)
        {
            var playlist = await GetPlaylistById(itemId, ctk);
            return new ProvidableItemDynamicMetadata
            {
                CommentCount = playlist?.CommentCount,
                ShareCount = playlist?.ShareCount,
                LikedCount = playlist?.SubscribedCount,
                IsLiked = playlist?.Subscribed
            };
        }

        return new ProvidableItemDynamicMetadata();
    }

    private static ProviderSessionInfo CreateSessionInfo(NeteaseUser? user)
    {
        return new ProviderSessionInfo
        {
            IsAuthenticated = user is not null,
            UserId = user?.ActualId,
            DisplayName = user?.Name
        };
    }

    private static ProviderQrLoginStatus MapQrStatus(int code)
    {
        return code switch
        {
            200 or 803 => ProviderQrLoginStatus.Authorized,
            800 => ProviderQrLoginStatus.Expired,
            801 => ProviderQrLoginStatus.WaitingForScan,
            802 => ProviderQrLoginStatus.WaitingForConfirmation,
            _ => ProviderQrLoginStatus.Failed
        };
    }

    private static ProviderPageResult<T> EmptyPage<T>()
    {
        return new ProviderPageResult<T>
        {
            Items = new List<T>(),
            HasMore = false
        };
    }

    private static string MapArtistScopedPrefix(string itemTypeId)
    {
        return itemTypeId switch
        {
            NeteaseTypeIds.Album => "alb",
            NeteaseTypeIds.SingleSong => "tim",
            _ => "hot"
        };
    }

    private static ListenTogetherPlayCommandRequest.ListenTogetherPlayCommandRequestCommandType MapListenTogetherPlaybackCommand(string commandId)
    {
        return commandId switch
        {
            "pause" => ListenTogetherPlayCommandRequest.ListenTogetherPlayCommandRequestCommandType.Pause,
            "previous" => ListenTogetherPlayCommandRequest.ListenTogetherPlayCommandRequestCommandType.Previous,
            "next" => ListenTogetherPlayCommandRequest.ListenTogetherPlayCommandRequestCommandType.Next,
            "goto" => ListenTogetherPlayCommandRequest.ListenTogetherPlayCommandRequestCommandType.Goto,
            "progress" => ListenTogetherPlayCommandRequest.ListenTogetherPlayCommandRequestCommandType.Progress,
            _ => ListenTogetherPlayCommandRequest.ListenTogetherPlayCommandRequestCommandType.Play
        };
    }

    private static ListenTogetherSyncListReportRequest.ListenTogetherSyncListReportPlayMode MapListenTogetherPlayMode(string playModeId)
    {
        return playModeId switch
        {
            "sgl" => ListenTogetherSyncListReportRequest.ListenTogetherSyncListReportPlayMode.SingleLoop,
            "shn" => ListenTogetherSyncListReportRequest.ListenTogetherSyncListReportPlayMode.Random,
            _ => ListenTogetherSyncListReportRequest.ListenTogetherSyncListReportPlayMode.OrderLoop
        };
    }

    private async Task<string> UploadCloudCoverAsync(string fileName, byte[] coverBytes, CancellationToken ctk)
    {
        var coverMd5 = Convert.ToHexString(MD5.HashData(coverBytes)).ToLowerInvariant();
        var coverAllocResult = await RequestAsync(NeteaseApis.CloudUploadCoverTokenAllocApi,
            new CloudUploadCoverTokenAllocRequest
            {
                Ext = "png",
                Filename = $"{fileName}_cover"
            }, ctk);
        if (coverAllocResult.IsError)
            throw new InvalidOperationException(coverAllocResult.Error?.Message ?? "Cloud cover token allocation failed.");

        var loadBalancer = await GetUploadLoadBalancerAsync(CloudCoverBucket, ctk);
        var targetLink = $"{loadBalancer}/{CloudCoverBucket}/{coverAllocResult.Value?.Result?.ObjectKey}?version=1.0";
        await UploadToNosAsync(targetLink, new MemoryStream(coverBytes), coverMd5, coverAllocResult.Value?.Result?.Token, "image/png", ctk: ctk);
        return coverAllocResult.Value?.Result?.DocId ?? string.Empty;
    }

    private async Task<string> GetUploadLoadBalancerAsync(string bucket, CancellationToken ctk)
    {
        const string fallback = "http://45.127.129.8";
        var result = await RequestAsync(NeteaseApis.NeteaseUploadLoadBalancerGetApi,
            new NeteaseUploadLoadBalancerGetRequest { Bucket = bucket }, ctk);
        return result.IsSuccess ? result.Value!.Upload?.FirstOrDefault() ?? fallback : fallback;
    }

    private static async Task UploadToNosAsync(
        string targetLink,
        Stream stream,
        string md5,
        string? token,
        string contentType,
        int chunkSize = 1048576,
        CancellationToken ctk = default)
    {
        if (string.IsNullOrEmpty(token)) throw new ArgumentException("Token must not be null or empty", nameof(token));
        if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);

        string? context = null;
        var isEnd = false;
        var offset = 0;
        while (!isEnd && !ctk.IsCancellationRequested)
        {
            var buffer = new byte[chunkSize];
            var bytesRead = await stream.ReadAsync(buffer, 0, chunkSize, ctk);
            isEnd = bytesRead < chunkSize;
            if (bytesRead == 0) break;

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                new Uri($"{targetLink}&offset={offset * chunkSize}&complete={isEnd.ToString().ToLowerInvariant()}&context={context}"));
            using var content = new ByteArrayContent(buffer, 0, bytesRead);
            content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            content.Headers.Add("Content-MD5", md5);
            request.Headers.Add("x-nos-token", token);
            request.Content = content;

            using var response = await CloudUploadHttpClient.SendAsync(request, ctk);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(ctk);
                throw new HttpRequestException($"Upload failed with status {response.StatusCode}: {errorContent}");
            }

            var responseText = await response.Content.ReadAsStringAsync(ctk);
            var match = System.Text.RegularExpressions.Regex.Match(responseText, "\"context\"\\s*:\\s*\"([^\"]*)\"");
            if (match.Success) context = match.Groups[1].Value;
            offset++;
        }
    }

    private static string GetMetadata(IReadOnlyDictionary<string, string> metadata, string key, string fallback)
    {
        return metadata.TryGetValue(key, out var value) && !string.IsNullOrWhiteSpace(value) ? value : fallback;
    }
}

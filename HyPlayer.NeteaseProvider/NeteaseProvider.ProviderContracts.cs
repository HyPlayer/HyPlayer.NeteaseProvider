using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.ApiContracts.Album;
using HyPlayer.NeteaseApi.ApiContracts.Artist;
using HyPlayer.NeteaseApi.ApiContracts.Cloud;
using HyPlayer.NeteaseApi.ApiContracts.Comment;
using HyPlayer.NeteaseApi.ApiContracts.DjChannel;
using HyPlayer.NeteaseApi.ApiContracts.Login;
using HyPlayer.NeteaseApi.ApiContracts.ListenTogether;
using HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Dual;
using HyPlayer.NeteaseApi.ApiContracts.Playlist;
using HyPlayer.NeteaseApi.ApiContracts.Recommend;
using HyPlayer.NeteaseApi.ApiContracts.Song;
using HyPlayer.NeteaseApi.ApiContracts.User;
using HyPlayer.NeteaseApi.ApiContracts.Utils;
using HyPlayer.NeteaseApi.ApiContracts.Video;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using HyPlayer.NeteaseApi.Models;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.NeteaseProvider.Models;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.Resources;
using HyPlayer.PlayCore.Abstraction.Models.SingleItems;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace HyPlayer.NeteaseProvider;

public partial class NeteaseProvider
{
    private const string CloudAudioBucket = "jd-musicrep-privatecloud-audio-public";
    private const string CloudCoverBucket = "yyimgs";
    private static readonly HttpClient CloudUploadHttpClient = new();

    private static string ComputeMd5Hex(byte[] bytes)
    {
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(bytes);
        var builder = new StringBuilder(hash.Length * 2);
        foreach (var value in hash)
            builder.Append(value.ToString("x2"));
        return builder.ToString();
    }

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
            var containers = await new NeteaseUser { ActualId = userId, Name = userId }.GetSubContainerAsync(ctk);
            var playlists = new List<NeteasePlaylist>();
            foreach (var subContainer in containers.OfType<NeteaseUserPlaylistSubContainer>())
            {
                var items = await subContainer.GetAllItemsAsync(ctk);
                playlists.AddRange(items.OfType<NeteasePlaylist>());
            }

            var created = playlists.FirstOrDefault(playlist => playlist.Name == name);
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

    public Task<ContainerBase?> GetCommentContainerAsync(CancellationToken ctk = default)
    {
        return Task.FromResult<ContainerBase?>(null);
    }

    public Task<ProviderPageResult<CommentBase>> GetCommentsAsync(string itemId, string typeId, int offset, int count, int sortType = 1, CancellationToken ctk = default)
    {
        var pageNo = count <= 0 ? 1 : (offset / count) + 1;
        return RequestCommentsPageAsync(itemId, typeId, pageNo, null, count, sortType, ctk);
    }

    public Task<ProviderPageResult<CommentBase>> GetCommentsPageAsync(string itemId, string typeId, string? pageToken, int count, int sortType = 1, CancellationToken ctk = default)
    {
        var (pageNo, cursor) = ParseCommentPageToken(pageToken);
        return RequestCommentsPageAsync(itemId, typeId, pageNo, cursor, count, sortType, ctk);
    }

    private async Task<ProviderPageResult<CommentBase>> RequestCommentsPageAsync(string itemId, string typeId, int pageNo, string? cursor, int count, int sortType, CancellationToken ctk)
    {
        var commentSortType = sortType switch
        {
            2 => CommentSortType.Hot,
            3 => CommentSortType.Time,
            _ => CommentSortType.Recommend
        };
        var result = await RequestAsync(NeteaseApis.CommentsApi,
            new CommentsRequest
            {
                ResourceId = itemId,
                ResourceType = TypeIdToSearchIdMapper.MapToResourceId(typeId),
                PageNo = pageNo,
                PageSize = count,
                CommentSortType = commentSortType,
                Cursor = cursor
            }, ctk);

        return result.Match(success =>
            {
                var hasMore = success.Data?.HasMore is true;
                var nextCursor = commentSortType == CommentSortType.Time ? success.Data?.Cursor : null;
                var canContinue = hasMore && (commentSortType != CommentSortType.Time || !string.IsNullOrEmpty(nextCursor));
                return new ProviderPageResult<CommentBase>
                {
                    Items = success.Data?.Comments?.Select(comment => (CommentBase)comment.MapToNeteaseComment()).ToList() ?? new List<CommentBase>(),
                    HasMore = hasMore,
                    NextOffset = hasMore && commentSortType != CommentSortType.Time ? pageNo * count : null,
                    NextPageToken = canContinue ? CreateCommentPageToken(pageNo + 1, nextCursor) : null,
                    TotalCount = success.Data is null ? null : (int?)success.Data.TotalCount
                };
            },
            _ => EmptyPage<CommentBase>());
    }

    private static string CreateCommentPageToken(int pageNo, string? cursor)
    {
        var page = pageNo.ToString(CultureInfo.InvariantCulture);
        return string.IsNullOrEmpty(cursor) ? page : $"{page}:{cursor}";
    }

    private static (int PageNo, string? Cursor) ParseCommentPageToken(string? pageToken)
    {
        if (pageToken is null || pageToken.Length == 0)
            return (1, null);

        var separator = pageToken.IndexOf(':');
        var page = separator < 0 ? pageToken : pageToken.Substring(0, separator);
        if (!int.TryParse(page, NumberStyles.None, CultureInfo.InvariantCulture, out var pageNo) || pageNo < 1)
            throw new ArgumentException("The comment page token is not valid.", nameof(pageToken));

        return (pageNo, separator < 0 ? null : pageToken.Substring(separator + 1));
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

    public async Task LikeProvidableItemAsync(string inProviderId, string? targetId, CancellationToken ctk = default)
    {
        await SetProvidableItemLikeStateAsync(inProviderId, targetId, true, ctk);
    }

    public async Task UnlikeProvidableItemAsync(string inProviderId, string? targetId, CancellationToken ctk = default)
    {
        await SetProvidableItemLikeStateAsync(inProviderId, targetId, false, ctk);
    }

    public async Task<List<string>> GetLikedProvidableIdsAsync(string typeId, CancellationToken ctk = default)
    {
        if (typeId != NeteaseTypeIds.SingleSong)
            return [];

        var result = await RequestAsync(NeteaseApis.LikelistApi, new LikelistRequest
        {
            Uid = LoginedUser?.ActualId!
        }, ctk);

        return result.Match(
            success => success.TrackIds?.Select(id => NeteaseTypeIds.SingleSong + id).ToList() ?? [],
            _ => []);
    }

    private async Task SetProvidableItemLikeStateAsync(string inProviderId, string? targetId, bool like, CancellationToken ctk)
    {
        var typeId = inProviderId.Length >= 2 ? inProviderId.Substring(0, 2) : string.Empty;
        var itemId = inProviderId.Length > 2 ? inProviderId.Substring(2) : inProviderId;
        if (typeId != NeteaseTypeIds.SingleSong)
            throw new NotSupportedException("NetEase item like is currently supported for single songs only.");
        if(targetId != null)
        {
            await RequestAsync(NeteaseApis.PlaylistTracksEditApi, new PlaylistTracksEditRequest()
            {
                PlaylistId = targetId,
                Id = itemId,
                IsAdd = like
            }, ctk);
        }
        else
        {
            await RequestAsync(NeteaseApis.LikeApi, new LikeRequest
            {
                TrackId = itemId,
                Like = like,
                UserId = LoginedUser?.ActualId!
            }, ctk);
        }
    }

    public Task<ProviderPageResult<CommentBase>> GetThreadedCommentsAsync(string itemId, string typeId, string commentId, int offset, int count, CancellationToken ctk = default)
    {
        return offset > 0
            ? Task.FromResult(EmptyPage<CommentBase>())
            : GetThreadedCommentsPageAsync(itemId, typeId, commentId, null, count, ctk);
    }

    public async Task<ProviderPageResult<CommentBase>> GetThreadedCommentsPageAsync(string itemId, string typeId, string commentId, string? pageToken, int count, CancellationToken ctk = default)
    {
        long time = 0;
        if (pageToken is { Length: > 0 }
            && !long.TryParse(pageToken, NumberStyles.None, CultureInfo.InvariantCulture, out time))
            throw new ArgumentException("The reply page token is not valid.", nameof(pageToken));

        var result = await RequestAsync(NeteaseApis.CommentFloorApi,
            new CommentFloorRequest
            {
                ResourceId = itemId,
                ResourceType = TypeIdToSearchIdMapper.MapToResourceId(typeId),
                ParentCommentId = commentId,
                Time = time,
                Limit = count
            }, ctk);

        return result.Match(success => new ProviderPageResult<CommentBase>
            {
                Items = success.Data?.Comments?.Select(comment => (CommentBase)comment.MapToNeteaseComment()).ToList() ?? new List<CommentBase>(),
                HasMore = success.Data?.HasMore is true,
                NextPageToken = success.Data?.HasMore is true
                    ? success.Data.Time.ToString(CultureInfo.InvariantCulture)
                    : null,
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
        if (containerId.StartsWith("hot", StringComparison.Ordinal)
            || containerId.StartsWith(NeteaseTypeIds.Artist, StringComparison.Ordinal))
        {
            var actualId = containerId.StartsWith("hot", StringComparison.Ordinal)
                ? containerId
                : $"hot{containerId.Substring(NeteaseTypeIds.Artist.Length)}";
            var artistContainer = new NeteaseArtistSubContainer { ActualId = actualId, Name = actualId };
            var (hasMore, items) = await artistContainer.GetProgressiveItemsListAsync(offset, count, ctk);
            return new ProviderPageResult<ProvidableItemBase>
            {
                Items = items,
                HasMore = hasMore,
                NextOffset = hasMore ? offset + count : null
            };
        }

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

    public async Task<CloudLibraryItemBase> UploadCloudLibraryItemAsync(
        ResourceBase resource,
        IReadOnlyDictionary<string, string> metadata,
        CancellationToken ctk = default)
    {
        var streamResult = await resource.GetResourceAsync(ctk: ctk);
        if (streamResult is not IResourceResultOf<Stream> streamResource)
            throw new InvalidOperationException("Cloud upload requires a stream resource.");

        using var stream = await streamResource.GetResourceAsync(ctk)
            ?? throw new InvalidOperationException("Cloud upload stream is empty.");
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        var bytes = memoryStream.ToArray();
        var md5 = ComputeMd5Hex(bytes);

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

    public async Task<RichMediaBase?> GetRichMediaAsync(string mediaId, string typeId, CancellationToken ctk = default)
    {
        if (typeId == NeteaseTypeIds.Mv)
        {
            var result = await RequestAsync(NeteaseApis.VideoDetailApi, new VideoDetailRequest { Id = mediaId }, ctk);
            return result.Match(success =>
                {
                    var data = success.Data?.Resource?.Data;
                    if (data is null) return null;
                    return new NeteaseMv
                    {
                        ActualId = data.Id ?? mediaId,
                        Name = data.Name ?? mediaId,
                        Description = data.Description,
                        CreatorName = data.ArtistName,
                        PublishTime = data.PublishTime,
                        Duration = data.Duration,
                        PlayCount = data.PlayCount,
                        SubCount = data.SubCount,
                        LikedCount = success.Data?.Resource?.LikedCount ?? 0,
                        AvailableQualities = data.Brs?.Select(item => item.Br).ToList() ?? [],
                        CoverUrl = null
                    } as RichMediaBase;
                },
                _ => null);
        }

        if (typeId == NeteaseTypeIds.MBlog)
        {
            var result = await RequestAsync(NeteaseApis.MlogDetailApi, new MlogDetailRequest { MlogId = mediaId }, ctk);
            return result.Match(success =>
                {
                    var resource = success.Data?.Resource;
                    if (resource is null) return null;
                    return new NeteaseMlog
                    {
                        ActualId = resource.Id ?? mediaId,
                        Name = resource.Content?.Title ?? mediaId,
                        Description = resource.Content?.Text,
                        CreatorName = resource.Profile?.Nickname,
                        PublishTime = resource.PublishTime,
                        LikedCount = resource.LikedCount,
                        Duration = resource.Content?.Video?.Duration ?? 0,
                        CoverUrl = resource.Content?.Video?.CoverUrl
                    } as RichMediaBase;
                },
                _ => null);
        }

        return null;
    }

    public async Task<ResourceBase?> GetRichMediaResourceAsync(string mediaId, string typeId, string? qualityId = null, CancellationToken ctk = default)
    {
        if (typeId == NeteaseTypeIds.Mv)
        {
            var result = await RequestAsync(NeteaseApis.VideoUrlApi,
                new VideoUrlRequest { Id = mediaId, Resolution = qualityId ?? "1080" }, ctk);
            return result.Match(success =>
                {
                    var url = success.Data?.Url;
                    return url is null || string.IsNullOrWhiteSpace(url)
                        ? null
                        : new NeteaseRichMediaResource { ResourceName = mediaId, Url = url } as ResourceBase;
                },
                _ => null);
        }

        if (typeId == NeteaseTypeIds.MBlog)
        {
            var result = await RequestAsync(NeteaseApis.MlogUrlApi,
                new MlogUrlRequest { Id = mediaId, Resolution = qualityId ?? "1080" }, ctk);
            return result.Match(success =>
                {
                    MlogUrlResponse.MlogUrlItem? item = null;
                    success.Data?.TryGetValue(mediaId, out item);
                    var url = item?.UrlInfo?.Url;
                    return url is null || string.IsNullOrWhiteSpace(url)
                        ? null
                        : new NeteaseRichMediaResource { ResourceName = mediaId, Url = url } as ResourceBase;
                },
                _ => null);
        }

        return null;
    }

    public async Task<ProviderPageResult<RichMediaBase>> GetRichMediaFeedAsync(string? typeId, int offset, int count, CancellationToken ctk = default)
    {
        var itemId = typeId?.Length > 2 ? typeId.Substring(2) : string.Empty;
        var songId = typeId?.StartsWith("song:", StringComparison.OrdinalIgnoreCase) is true ? itemId : null;
        var result = await RequestAsync(NeteaseApis.MlogRcmdFeedListApi,
            new MlogRcmdFeedListRequest { Id = itemId, SongId = songId, Limit = count }, ctk);

        return result.Match(success => new ProviderPageResult<RichMediaBase>
            {
                Items = success.Data?.Feeds?.Select(feed => feed.Resource?.BaseData).Where(item => item is not null).Select(item => (RichMediaBase)new NeteaseMlog
                {
                    ActualId = item!.Id,
                    Name = item.Title ?? item.OriginalTitle ?? item.Id ?? string.Empty,
                    Description = item.Description,
                    Duration = item.Duration,
                    CoverUrl = item.CoverUrl
                }).ToList() ?? [],
                HasMore = false
            },
            _ => EmptyPage<RichMediaBase>());
    }

    public async Task<Uri?> GetDynamicCoverAsync(string itemId, CancellationToken ctk = default)
    {
        var result = await RequestAsync(NeteaseApis.SongDynamicCoverApi, new SongDynamicCoverRequest { SongId = itemId }, ctk);
        return result.Match(success =>
            {
                if (success.Code != 200)
                    throw new InvalidOperationException(success.Message ?? $"NetEase dynamic cover request failed with code {success.Code}.");
                return Uri.TryCreate(success.Data?.VideoPlayUrl, UriKind.Absolute, out var uri) ? uri : null;
            },
            error => throw error);
    }

    public async Task<string> CreateListenTogetherRoomAsync(List<SingleSongBase> queue, CancellationToken ctk = default)
    {
        await EnsureDesktopClientVersionAsync(ctk);
        var result = await RequestAsync(NeteaseApis.ListenTogetherRoomCreateApi, new ListenTogetherRoomCreateRequest(), ctk);
        return result.Match(success =>
            {
                EnsureListenTogetherSuccess(success.Code, success.Message);
                return success.Data?.RoomInfo?.RoomId is { Length: > 0 } roomId
                    ? roomId
                    : throw new InvalidOperationException("NetEase did not return a listen-together room id.");
            },
            error => throw error);
    }

    public async Task<bool> CanJoinListenTogetherRoomAsync(string roomId, CancellationToken ctk = default)
    {
        await EnsureDesktopClientVersionAsync(ctk);
        var result = await RequestAsync(NeteaseApis.ListenTogetherRoomCheckApi,
            new ListenTogetherRoomCheckRequest { RoomId = roomId }, ctk);
        return result.Match(success => success.Code == 200 && success.Data?.Joinable is true, error => throw error);
    }

    public async Task SendListenTogetherPlaybackCommandAsync(
        string roomId,
        ProviderListenTogetherPlaybackCommand command,
        CancellationToken ctk = default)
    {
        var result = await RequestAsync(NeteaseApis.ListenTogetherPlayCommandApi,
            new ListenTogetherPlayCommandRequest
            {
                RoomId = roomId,
                CommandType = MapListenTogetherPlaybackCommand(command.CommandId),
                PlayStatus = command.IsPlaying
                    ? ListenTogetherHeartBeatRequest.ListenTogetherPlayStatus.Play
                    : ListenTogetherHeartBeatRequest.ListenTogetherPlayStatus.Pause,
                FormerSongId = command.FormerItemId ?? "-1",
                TargetSongId = command.TargetItemId ?? string.Empty,
                ClientSeq = command.ClientSeq,
                Progress = (long)Math.Max(0, command.Position.TotalMilliseconds)
            }, ctk);
        _ = result.Match(success =>
            {
                EnsureListenTogetherSuccess(success.Code, success.Message);
                return success.Data?.Result is false
                    ? throw new InvalidOperationException("NetEase rejected the listen-together playback command.")
                    : true;
            },
            error => throw error);
    }

    public async Task ReportListenTogetherQueueAsync(
        string roomId,
        ProviderListenTogetherQueueReport report,
        CancellationToken ctk = default)
    {
        var display = report.Queue.Select(GetTrackId).OfType<string>().Where(id => id.Length > 0).ToArray();
        var random = report.RandomQueue?.Select(GetTrackId).OfType<string>().Where(id => id.Length > 0).ToArray();
        var result = await RequestAsync(NeteaseApis.ListenTogetherSyncListReportApi,
            new ListenTogetherSyncListReportRequest
            {
                RoomId = roomId,
                CommandType = ListenTogetherSyncListReportRequest.ListenTogetherSyncListReportCommandType.Replace,
                PlayMode = MapListenTogetherPlayMode(report.PlayModeId),
                UserId = report.UserId ?? LoginedUser?.ActualId ?? string.Empty,
                ClientSeq = report.ClientSeq,
                AnchorPosition = report.AnchorPosition,
                AnchorSongId = report.AnchorItemId ?? string.Empty,
                DisplaySongList = display,
                RandomSongList = random is { Length: > 0 } ? random : display
            }, ctk);
        _ = result.Match(success =>
            {
                EnsureListenTogetherSuccess(success.Code, success.Message);
                return success.Data?.Result is false
                    ? throw new InvalidOperationException("NetEase rejected the listen-together queue report.")
                    : true;
            },
            error => throw error);
    }

    public async Task<ProviderListenTogetherStatus?> GetListenTogetherStatusAsync(string roomId, CancellationToken ctk = default)
    {
        var result = await RequestAsync(NeteaseApis.ListenTogetherStatusApi, new ListenTogetherStatusRequest(), ctk);
        return result.Match(success =>
            {
                EnsureListenTogetherSuccess(success.Code, success.Message);
                var roomInfo = success.Data?.RoomInfo;
                return new ProviderListenTogetherStatus
                {
                    IsInRoom = success.Data?.IsInRoom is true,
                    RoomId = roomInfo?.RoomId,
                    CreatorId = roomInfo?.CreatorId,
                    IsMultiRoom = roomInfo?.RoomType?.StartsWith("MULTI", StringComparison.OrdinalIgnoreCase) is true,
                    Users = MapListenTogetherUsers(roomInfo?.RoomUsers)
                };
            },
            error => throw error);
    }

    public async Task<ProviderListenTogetherStatus> JoinListenTogetherRoomAsync(
        string roomId,
        string inviterId,
        CancellationToken ctk = default)
    {
        await EnsureDesktopClientVersionAsync(ctk);
        var result = await RequestAsync(NeteaseApis.ListenTogetherInvitationAcceptApi,
            new ListenTogetherInvitationAcceptRequest { RoomId = roomId, InviterId = inviterId }, ctk);
        return result.Match(success =>
            {
                EnsureListenTogetherSuccess(success.Code, success.Message);
                var roomInfo = success.Data?.RoomInfo;
                return new ProviderListenTogetherStatus
                {
                    IsInRoom = true,
                    RoomId = roomInfo?.RoomId ?? roomId,
                    CreatorId = roomInfo?.CreatorId ?? inviterId,
                    Users = MapListenTogetherUsers(roomInfo?.RoomUsers)
                };
            },
            error => throw error);
    }

    public async Task<TimeSpan?> SendListenTogetherHeartbeatAsync(
        string roomId,
        ProviderListenTogetherHeartbeat heartbeat,
        CancellationToken ctk = default)
    {
        var result = await RequestAsync(NeteaseApis.ListenTogetherHeartBeatApi,
            new ListenTogetherHeartBeatRequest
            {
                RoomId = roomId,
                SongId = heartbeat.ItemId,
                PlayStatus = heartbeat.IsPlaying
                    ? ListenTogetherHeartBeatRequest.ListenTogetherPlayStatus.Play
                    : ListenTogetherHeartBeatRequest.ListenTogetherPlayStatus.Pause,
                Progress = (long)Math.Max(0, heartbeat.Position.TotalMilliseconds),
                PlaylistVersion = (int)Math.Min(Math.Max(heartbeat.PlaylistVersion, 0), int.MaxValue),
                UserId = heartbeat.UserId ?? LoginedUser?.ActualId ?? "0"
            }, ctk);
        return result.Match(success =>
            {
                EnsureListenTogetherSuccess(success.Code, success.Message);
                return success.Data?.NextHeartbeat is int seconds && seconds > 0
                    ? TimeSpan.FromSeconds(seconds)
                    : (TimeSpan?)null;
            },
            error => throw error);
    }

    public async Task<ProviderListenTogetherRoomState> GetListenTogetherRoomStateAsync(
        string roomId,
        CancellationToken ctk = default)
    {
        var result = await RequestAsync(NeteaseApis.ListenTogetherSyncListGetApi,
            new ListenTogetherSyncListGetRequest { RoomId = roomId }, ctk);
        return result.Match(success =>
            {
                EnsureListenTogetherSuccess(success.Code, success.Message);
                var playlist = success.Data?.Playlist;
                var command = success.Data?.PlayCommand;
                var display = CleanListenTogetherIds(playlist?.DisplayList?.Result);
                var random = CleanListenTogetherIds(playlist?.RandomList?.Result);
                return new ProviderListenTogetherRoomState
                {
                    DisplayItemIds = display,
                    RandomItemIds = random.Count > 0 ? random : display,
                    PlayModeId = playlist?.PlayMode switch
                    {
                        "ORDER_LOOP" => "seq",
                        "RANDOM" => "shn",
                        "SINGLE_LOOP" => "sgl",
                        _ => null
                    },
                    Versions = playlist?.Version?
                        .Where(version => !string.IsNullOrEmpty(version.UserId))
                        .Select(version => new ProviderListenTogetherQueueVersion
                        {
                            UserId = version.UserId!,
                            Version = version.Version ?? 0
                        })
                        .ToList() ?? [],
                    LastCommand = command?.CommandType is { Length: > 0 } commandType
                        ? new ProviderListenTogetherRemoteCommand
                        {
                            CommandId = MapListenTogetherRemoteCommand(commandType),
                            UserId = command.UserId,
                            FormerItemId = command.FormerSongId,
                            TargetItemId = command.TargetSongId,
                            Position = TimeSpan.FromMilliseconds(Math.Max(0, command.Progress ?? 0)),
                            IsPlaying = string.Equals(command.PlayStatus, "PLAY", StringComparison.OrdinalIgnoreCase),
                            ClientSeq = command.ClientSeq ?? 0,
                            ServerSeq = command.ServerSeq ?? 0
                        }
                        : null
                };
            },
            error => throw error);
    }

    public async Task<List<SingleSongBase>> GetListenTogetherQueueSongsAsync(
        IReadOnlyList<string> itemIds,
        CancellationToken ctk = default)
    {
        if (itemIds.Count == 0) return [];
        var songs = await GetSingleSongRangeByIds(itemIds.Distinct().ToList(), ctk);
        var byId = songs
            .Where(song => !string.IsNullOrEmpty(song.ActualId))
            .GroupBy(song => song.ActualId!)
            .ToDictionary(group => group.Key, group => (SingleSongBase)group.First());
        return itemIds.Select(id => byId.TryGetValue(id, out var song) ? song : null).OfType<SingleSongBase>().ToList();
    }

    public async Task EndListenTogetherRoomAsync(string roomId, CancellationToken ctk = default)
    {
        var result = await RequestAsync(NeteaseApis.ListenTogetherEndApi, new ListenTogetherEndRequest { RoomId = roomId }, ctk);
        _ = result.Match(success =>
            {
                EnsureListenTogetherSuccess(success.Code, success.Message);
                return success.Data?.Success is true
                    ? true
                    : throw new InvalidOperationException("NetEase did not confirm ending the listen-together room.");
            },
            error => throw error);
    }

    private async Task EnsureDesktopClientVersionAsync(CancellationToken ctk)
    {
        if (Interlocked.Exchange(ref _desktopClientVersionResolved, 1) != 0) return;
        var result = await RequestAsync(NeteaseApis.PcClientLatestVersionApi, new PcClientLatestVersionRequest(), ctk);
        if (result.IsSuccess &&
            result.Value?.Data is { AppVer: { Length: > 0 } appVer, BuildVer: { Length: > 0 } buildVer })
            DesktopClientVersion.Current = $"{appVer}.{buildVer}";
        else
            Interlocked.Exchange(ref _desktopClientVersionResolved, 0);
    }

    private static void EnsureListenTogetherSuccess(int code, string? message)
    {
        if (code != 200)
            throw new InvalidOperationException(string.IsNullOrWhiteSpace(message)
                ? $"NetEase listen-together request failed with code {code}."
                : message);
    }

    private static List<ProviderListenTogetherUser> MapListenTogetherUsers(
        List<ListenTogetherRoomCreateResponse.ListenTogetherRoomInfo.RoomUser>? users)
    {
        return users?.Select(user => new ProviderListenTogetherUser
        {
            UserId = user.UserId ?? string.Empty,
            Nickname = user.Nickname ?? string.Empty,
            AvatarUrl = user.AvatarUrl ?? string.Empty
        }).ToList() ?? [];
    }

    private static List<string> CleanListenTogetherIds(IEnumerable<string>? ids)
    {
        return ids?.Where(id => !string.IsNullOrWhiteSpace(id) && id != "-1").ToList() ?? [];
    }

    private static string MapListenTogetherRemoteCommand(string commandType)
    {
        return commandType.ToUpperInvariant() switch
        {
            "GOTO" => "goto",
            "PLAY" => "play",
            "PAUSE" => "pause",
            "SEEK" or "PROGRESS" => "seek",
            "NEXT" => "next",
            "PREV" => "previous",
            _ => commandType.ToLowerInvariant()
        };
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

    private static ListenTogetherPlayCommandRequest.ListenTogetherPlayCommandRequestCommandType MapListenTogetherPlaybackCommand(string commandId)
    {
        return commandId switch
        {
            "pause" => ListenTogetherPlayCommandRequest.ListenTogetherPlayCommandRequestCommandType.Pause,
            "previous" => ListenTogetherPlayCommandRequest.ListenTogetherPlayCommandRequestCommandType.Previous,
            "next" => ListenTogetherPlayCommandRequest.ListenTogetherPlayCommandRequestCommandType.Next,
            "goto" => ListenTogetherPlayCommandRequest.ListenTogetherPlayCommandRequestCommandType.Goto,
            "progress" or "seek" => ListenTogetherPlayCommandRequest.ListenTogetherPlayCommandRequestCommandType.Progress,
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
        var coverMd5 = ComputeMd5Hex(coverBytes);
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
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Upload failed with status {response.StatusCode}: {errorContent}");
            }

            var responseText = await response.Content.ReadAsStringAsync();
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

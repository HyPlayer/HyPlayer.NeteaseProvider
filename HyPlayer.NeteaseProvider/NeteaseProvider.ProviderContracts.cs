using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.ApiContracts.Album;
using HyPlayer.NeteaseApi.ApiContracts.Artist;
using HyPlayer.NeteaseApi.ApiContracts.Cloud;
using HyPlayer.NeteaseApi.ApiContracts.Comment;
using HyPlayer.NeteaseApi.ApiContracts.Login;
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
using HyPlayer.PlayCore.Abstraction.Models.SingleItems;

namespace HyPlayer.NeteaseProvider;

public partial class NeteaseProvider
{
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
                SessionInfo = success.Code == 200 ? CreateSessionInfo(LoginedUser) : null,
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
            200 => ProviderQrLoginStatus.Authorized,
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
}

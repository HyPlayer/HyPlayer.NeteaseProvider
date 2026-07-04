using HyPlayer.NeteaseApi;
using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.ApiContracts.Login;
using HyPlayer.NeteaseApi.ApiContracts.Playlist;
using HyPlayer.NeteaseApi.ApiContracts.Artist;
using HyPlayer.NeteaseApi.ApiContracts.Album;
using HyPlayer.NeteaseApi.ApiContracts.Video;
using HyPlayer.NeteaseApi.ApiContracts.User;
using HyPlayer.NeteaseApi.ApiContracts.Recommend;
using HyPlayer.NeteaseApi.ApiContracts.Song;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.NeteaseProvider.Models;
using HyPlayer.PlayCore.Abstraction;
using HyPlayer.PlayCore.Abstraction.Interfaces.Provider;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.Lyric;
using HyPlayer.PlayCore.Abstraction.Models.Resources;
using HyPlayer.PlayCore.Abstraction.Models.SingleItems;
using HyPlayer.NeteaseApi.Extensions;
using HyPlayer.NeteaseApi.ApiContracts.DjChannel;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace HyPlayer.NeteaseProvider;

public partial class NeteaseProvider : ProviderBase,
                               ILyricProvidable,
                               IMusicResourceProvidable,
                                 IProvableItemLikable,
                                 IProvidableItemProvidable,
                                IProvidableItemRangeProvidable,
                                ISearchableProvider,
                                 IAuthenticationProvidable,
                                IContainerManagementProvidable,
                                ISearchSuggestionProvidable,
                                 IContainerPageProvidable,
                                 IProviderKnownTypeIds,
                                 IQrAuthenticationProvidable,
                                 ICommentProvidable,
                                  IProvidableItemCommentProvidable,
                                   IListenTogetherProvidable,
                                  ICloudUploadProvidable,
                                  IRichMediaProvidable,
                                  IProvidableItemDynamicMetadataProvidable,
                                  IPersonalRadioProvidable,
                                  IProviderNetworkConfigurationProvidable,
                                  IContainerItemManagementProvidable,
                                  IProviderAdditionalConfigurationProvidable,
                                  IProviderSearchCategoryTypeIds,
                                  IUserLibraryTypeIds,
                                  IUserLibraryProvidable,
                                  IUserLibraryNavigationProvidable,
                                  IProviderSpecialContainerTypeIds,
                                  IResourceQualityTagProvidable
{
    public readonly NeteaseCloudMusicApiHandler Handler = new();

    public void ConfigureAdditionalParameters(AdditionalParameters additionalParameters)
    {
        Handler.Option.AdditionalParameters = additionalParameters;
    }

    public bool HasAdditionalConfiguration => Handler.Option.AdditionalParameters.HasValue();

    public string ExportAdditionalConfiguration()
    {
        return JsonSerializer.Serialize(Handler.Option.AdditionalParameters, Handler.Option.JsonSerializerOptions);
    }

    public void ImportAdditionalConfiguration(string configurationJson)
    {
        Handler.Option.AdditionalParameters = string.IsNullOrWhiteSpace(configurationJson)
            ? new AdditionalParameters()
            : JsonSerializer.Deserialize<AdditionalParameters>(configurationJson, Handler.Option.JsonSerializerOptions) ?? new AdditionalParameters();
    }

    public void ConfigureFakeCheckToken(bool enabled)
    {
        Handler.Option.FakeCheckToken = enabled;
    }

    public void ConfigureXRealIP(string? xRealIp)
    {
        Handler.Option.XRealIP = xRealIp;
    }

    public void ConfigureDegradeHttp(bool enabled)
    {
        Handler.Option.DegradeHttp = enabled;
    }

    public void ConfigureClientNetwork(string? clientIp, bool useInsecureHttp)
    {
        ConfigureXRealIP(clientIp);
        ConfigureDegradeHttp(useInsecureHttp);
    }

    public HttpClient ConfigureHttpClient(bool useProxy)
    {
        var handler = NeteaseCloudMusicApiHandler.HttpClientHandler;
        handler.UseProxy = useProxy;
        var client = new HttpClient(handler);
        Handler.HttpClient = client;
        return client;
    }

    public bool HasAdditionalCookies => Handler.Option.AdditionalParameters.Cookies.Count > 0;

    public Dictionary<string, string> GetRuntimeCookiesSnapshot()
    {
        var cookies = Handler.Option.Cookies.ToDictionary(pair => pair.Key, pair => pair.Value);
        foreach (var pair in Handler.Option.AdditionalParameters.Cookies)
        {
            if (pair.Value is null)
                cookies.Remove(pair.Key);
            else
                cookies[pair.Key] = pair.Value;
        }

        return cookies;
    }

    public void ClearRuntimeCookies()
    {
        Handler.Option.Cookies.Clear();
    }

    public void SetRuntimeCookie(string name, string value)
    {
        Handler.Option.Cookies[name] = value;
    }
    public override string Name => "网易云音乐";
    public override string Id => "ncm";

    public static NeteaseProvider Instance = null!;

    public NeteaseUser? LoginedUser { get; set; }

    public string SingleSongTypeId => NeteaseTypeIds.SingleSong;
    public string PlaylistTypeId => NeteaseTypeIds.Playlist;
    public string ArtistTypeId => NeteaseTypeIds.Artist;
    public string AlbumTypeId => NeteaseTypeIds.Album;
    public string UserTypeId => NeteaseTypeIds.User;
    public string? RadioChannelTypeId => NeteaseTypeIds.RadioChannel;
    public string? RichMediaTypeId => NeteaseTypeIds.Mv;
    public string SingleSongSearchTypeId => NeteaseTypeIds.SingleSong;
    public string AlbumSearchTypeId => NeteaseTypeIds.Album;
    public string ArtistSearchTypeId => NeteaseTypeIds.Artist;
    public string PlaylistSearchTypeId => NeteaseTypeIds.Playlist;
    public string UserSearchTypeId => NeteaseTypeIds.User;
    public string? RadioChannelSearchTypeId => NeteaseTypeIds.RadioChannel;
    public string? RichMediaSearchTypeId => NeteaseTypeIds.Mv;
    public string? ShortVideoSearchTypeId => NeteaseTypeIds.MBlog;
    public string? LyricSearchTypeId => NeteaseTypeIds.Lyric;
    public string CloudLibraryTypeId => NeteaseUserLibrarySubContainer.CloudKind;
    public string LikedSongsTypeId => NeteaseUserLibrarySubContainer.LikedSongsKind;
    public string RecentListeningHistoryTypeId => NeteaseUserLibrarySubContainer.ListeningHistoryRecentKind;
    public string AllListeningHistoryTypeId => NeteaseUserLibrarySubContainer.ListeningHistoryAllKind;
    public IReadOnlyDictionary<SpecialContainerType, string> SpecialContainerTypeIds { get; } =
        new Dictionary<SpecialContainerType, string>
        {
            [SpecialContainerType.RecommendedSongs] = NeteaseTypeIds.RecommendedSongs,
            [SpecialContainerType.RecommendedPlaylists] = NeteaseTypeIds.RecommendedPlaylists,
            [SpecialContainerType.Toplists] = NeteaseTypeIds.Chart,
            [SpecialContainerType.PlaylistCategory] = NeteaseTypeIds.PlaylistCategory,
            [SpecialContainerType.PersonalRadio] = NeteaseTypeIds.PersonalFm,
            [SpecialContainerType.ContextRecommendation] = NeteaseTypeIds.ContextRecommendation
        };

    public NeteaseProvider()
    {
        Instance = this;
    }

    public Task MovePersonalRadioItemToTrashAsync(string itemId, CancellationToken ctk = default)
    {
        return new NeteasePersonalFMContainer { ActualId = string.Empty, Name = "私人FM" }
            .MoveItemToTrashAsync(itemId, ctk);
    }

    public Task<IReadOnlyDictionary<string, ResourceQualityTag>> GetAvailableQualityTagsAsync(ResourceType type, CancellationToken ctk = default)
    {
        if (type != ResourceType.Audio)
            return Task.FromResult<IReadOnlyDictionary<string, ResourceQualityTag>>(new Dictionary<string, ResourceQualityTag>());

        IReadOnlyDictionary<string, ResourceQualityTag> tags = new Dictionary<string, ResourceQualityTag>
        {
            ["standard"] = new NeteaseMusicQualityTag("standard"),
            ["exhigh"] = new NeteaseMusicQualityTag("exhigh"),
            ["lossless"] = new NeteaseMusicQualityTag("lossless"),
            ["hires"] = new NeteaseMusicQualityTag("hires"),
            ["jyeffect"] = new NeteaseMusicQualityTag("jyeffect"),
            ["sky"] = new NeteaseMusicQualityTag("sky"),
            ["jymaster"] = new NeteaseMusicQualityTag("jymaster")
        };
        return Task.FromResult(tags);
    }

    public Task RemoveItemFromContainerAsync(string containerId, string itemId, CancellationToken ctk = default)
    {
        if (containerId == NeteaseUserLibrarySubContainer.CloudKind)
            return NeteaseUserLibrarySubContainer.DeleteCloudLibraryItemAsync(itemId, ctk);

        return NeteaseItemActions.RemoveSongFromPlaylistAsync(containerId, itemId, ctk);
    }

    public Task<ContainerBase?> GetCurrentUserLibraryContainerAsync(string libraryTypeId, CancellationToken ctk = default)
    {
        return Task.FromResult<ContainerBase?>(CreateUserLibraryContainer(libraryTypeId, LoginedUser?.ActualId));
    }

    public Task<ContainerBase?> GetUserLibraryContainerAsync(string userId, string libraryTypeId, CancellationToken ctk = default)
    {
        return Task.FromResult<ContainerBase?>(CreateUserLibraryContainer(libraryTypeId, userId));
    }

    public async Task<IReadOnlyList<ProviderLibraryNavigationGroup>> GetCurrentUserLibraryNavigationGroupsAsync(CancellationToken ctk = default)
    {
        if (LoginedUser is null)
            return [];

        var containers = await LoginedUser.GetSubContainerAsync(ctk);
        var groups = new List<ProviderLibraryNavigationGroup>();
        foreach (var container in containers.OfType<NeteaseUserPlaylistSubContainer>())
        {
            var items = await container.GetAllItemsAsync(ctk);
            var playlists = items.OfType<ContainerBase>().ToList();
            if (playlists.Count == 0)
                continue;

            if (container.Kind == NeteaseUserPlaylistSubContainer.CreatedKind)
            {
                groups.Add(new ProviderLibraryNavigationGroup
                {
                    Id = NeteaseUserLibrarySubContainer.LikedSongsKind,
                    Title = "我喜欢的音乐",
                    Items = [playlists[0]],
                    DisplayOrder = 0,
                    IsPinned = true
                });

                if (playlists.Count > 1)
                {
                    groups.Add(new ProviderLibraryNavigationGroup
                    {
                        Id = NeteaseUserPlaylistSubContainer.CreatedKind,
                        Title = "我创建的歌单",
                        Items = playlists.Skip(1).ToList(),
                        DisplayOrder = 10
                    });
                }
            }
            else if (container.Kind == NeteaseUserPlaylistSubContainer.SubscribedKind)
            {
                groups.Add(new ProviderLibraryNavigationGroup
                {
                    Id = NeteaseUserPlaylistSubContainer.SubscribedKind,
                    Title = "我收藏的歌单",
                    Items = playlists,
                    DisplayOrder = 20
                });
            }
        }

        return groups.OrderBy(group => group.DisplayOrder).ToList();
    }

    private static NeteaseUserLibrarySubContainer CreateUserLibraryContainer(string libraryTypeId, string? userId)
    {
        return new NeteaseUserLibrarySubContainer
        {
            ActualId = $"library-{libraryTypeId}{userId}",
            Name = "用户资料库",
            Kind = libraryTypeId,
            UserId = userId
        };
    }

    public override List<ProvidableTypeId> ProvidableTypeIds =>
        new()
        {
            new(NeteaseTypeIds.SingleSong, "歌曲", true),
            new(NeteaseTypeIds.Playlist, "歌单", true),
            new(NeteaseTypeIds.Artist, "歌手", true),
            new(NeteaseTypeIds.Album, "专辑", true),
            new(NeteaseTypeIds.User, "用户", true),
            new(NeteaseTypeIds.RadioProgram, "节目", true), // 电台节目
            new(NeteaseTypeIds.RadioChannel, "电台", true),
        };


    public async Task<Results<TResponse, ErrorResultBase>> RequestAsync<TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract,
        CancellationToken cancellationToken = default)
        where TError : ErrorResultBase
        where TActualRequest : ActualRequestBase
        where TRequest : RequestBase
        where TResponse : ResponseBase, new()
    {
        try
        {
            return await Handler.RequestAsync(contract, cancellationToken);
        }
        catch (Exception ex)
        {
            return Results<TResponse, ErrorResultBase>.CreateError(new ExceptionedErrorBase(-500, ex.Message, ex));
        }
    }

    public async Task<Results<TResponse, ErrorResultBase>> RequestAsync<TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, TRequest request,
        CancellationToken cancellationToken = default)
        where TError : ErrorResultBase
        where TActualRequest : ActualRequestBase
        where TRequest : RequestBase
        where TResponse : ResponseBase, new()
    {
        try
        {
            return await Handler.RequestAsync(contract, request, cancellationToken);
        }
        catch (Exception ex)
        {
            return Results<TResponse, ErrorResultBase>.CreateError(new ExceptionedErrorBase(-500, ex.Message, ex));
        }
    }
    
    public async Task<Results<TCustomResponse, ErrorResultBase>> RequestAsync<
        TCustomResponse, TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, TRequest? request,
        CancellationToken cancellationToken = default)
        where TError : ErrorResultBase
        where TActualRequest : ActualRequestBase
        where TRequest : RequestBase
        where TResponse : ResponseBase, new()
        where TCustomResponse : ResponseBase, new()
    {
        try
        {
            return await Handler.RequestAsync<TCustomResponse, TRequest, TResponse, TError, TActualRequest>(
                contract, request, cancellationToken);
        }
        catch (Exception ex)
        {
            return Results<TCustomResponse, ErrorResultBase>.CreateError(
                new ExceptionedErrorBase(-500, ex.Message, ex));
        }
    }

    public async Task<Results<TCustomResponse, ErrorResultBase>> RequestAsync<
        TCustomRequest, TCustomResponse, TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, bool differ, TCustomRequest? request,
        CancellationToken cancellationToken = default)
        where TError : ErrorResultBase
        where TActualRequest : ActualRequestBase
        where TRequest : RequestBase
        where TResponse : ResponseBase, new()
        where TCustomResponse : ResponseBase, new()
    {
        try
        {
            return await Handler
                .RequestAsync<TCustomRequest, TCustomResponse, TRequest, TResponse, TError, TActualRequest>(
                    contract, request, cancellationToken);
        }
        catch (Exception ex)
        {
            return Results<TCustomResponse, ErrorResultBase>.CreateError(
                new ExceptionedErrorBase(-500, ex.Message, ex));
        }
    }

    public async Task<Results<TResponse, ErrorResultBase>> RequestAsync<TCustomRequest, TRequest, TResponse, TError,
                                                                        TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, bool differ, TCustomRequest? request,
        ApiHandlerOption option, CancellationToken cancellationToken = default)
        where TError : ErrorResultBase
        where TActualRequest : ActualRequestBase
        where TRequest : RequestBase
        where TResponse : ResponseBase, new()
    {
        try
        {
            return await Handler.RequestAsync(
                contract, true, request, option, cancellationToken);
        }
        catch (Exception ex)
        {
            return Results<TResponse, ErrorResultBase>.CreateError(
                new ExceptionedErrorBase(-500, ex.Message, ex));
        }
    }

    public async Task<bool> LoginEmailAsync(string email, string password, bool isMd5 = false,
                                            CancellationToken ctk = default)
    {
        var request = new LoginEmailRequest()
        {
            Email = email,
        };
        if (isMd5)
            request.Md5Password = password;
        else
            request.Password = password;
        var result = await RequestAsync(NeteaseApis.LoginEmailApi, request, ctk);
        return result.Match(
            success =>
            {
                LoginedUser = success.Profile?.MapToNeteaseUser();
                return true;
            },
            _ => false
        );
    }

    public async Task<bool> LoginCellphoneAsync(string cellphone, string password, bool isMd5 = false,
                                                CancellationToken ctk = default)
    {
        var request = new LoginCellphoneRequest()
        {
            Cellphone = cellphone
        };
        if (isMd5)
            request.Md5Password = password;
        else
            request.Password = password;
        var result = await RequestAsync(NeteaseApis.LoginCellphoneApi, request, ctk);
        return result.Match(
            success =>
            {
                LoginedUser = success.Profile?.MapToNeteaseUser();
                return true;
            },
            _ => false
        );
    }


    public async Task<List<RawLyricInfo>> GetLyricInfoAsync(SingleSongBase song, CancellationToken ctk = default)
    {
        if (song.ActualId == null) throw new ArgumentNullException();
        var results = await RequestAsync(NeteaseApis.LyricApi, new LyricRequest() { Id = song.ActualId }, ctk);
        return results.Match(
            success => success.Map(),
            _ => new()
        ).Select(t => (RawLyricInfo)t).ToList();
    }

    public async Task<MusicResourceBase?> GetMusicResourceAsync(SingleSongBase song, ResourceQualityTag? qualityTag = null,
                                                                CancellationToken ctk = default)
    {
        var quality = "exhigh";
        if (qualityTag is NeteaseMusicQualityTag neteaseMusicQualityTag)
            quality = neteaseMusicQualityTag.Quality;
        var results = await RequestAsync(NeteaseApis.SongUrlApi,
                                         new SongUrlRequest
                                         {
                                             Id = song.ActualId,
                                             Level = quality
                                         }, ctk);

        NeteaseMusicResource? MatchSuccess(SongUrlResponse response)
        {
            if (response.Code is not 200) return null;
            if (response.SongUrls is not { Length: > 0 }) return null;
            var songUrl = response.SongUrls[0];
            var uri = CreateAbsoluteUriOrNull(songUrl.Url);
            if (uri is null) return null;

            return new NeteaseMusicResource
            {
                Md5 = songUrl.Md5,
                Size = songUrl.Size,
                BitRate = songUrl.BitRate,
                EncodeType = songUrl.EncodeType,
                Time = songUrl.Time,
                MusicType = songUrl.Type,
                Level = songUrl.Level,
                Uri = uri
            };
        }


        return results.Match(
            MatchSuccess,
            _ => null
        );
    }

    private static Uri? CreateAbsoluteUriOrNull(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return null;
        return Uri.TryCreate(url, UriKind.Absolute, out var uri) ? uri : null;
    }

    public async Task<ProvidableItemBase?> GetProvidableItemByIdAsync(string inProviderId,
                                                                      CancellationToken ctk = default)
    {
        var typeId = inProviderId.Substring(0, 2);
        var actualId = inProviderId.Substring(2);
        switch (typeId)
        {
            case NeteaseTypeIds.SingleSong:
                return await GetSingleSongById(actualId, ctk);
            case NeteaseTypeIds.Playlist:
                return await GetPlaylistById(actualId, ctk);
            case NeteaseTypeIds.Album:
                return await GetAlbumById(actualId, ctk);
            case NeteaseTypeIds.Artist:
                return await GetArtistById(actualId, ctk);
            case NeteaseTypeIds.User:
                return await GetUserById(actualId, ctk);
            case NeteaseTypeIds.RadioChannel:
                return await GetRadioChannelById(actualId, ctk);
            case NeteaseTypeIds.PersonalFm:
                return new NeteasePersonalFMContainer
                {
                    ActualId = actualId,
                    Name = "私人 FM"
                };
            case NeteaseTypeIds.RecommendedSongs:
                return new NeteaseRecommendSongContainer { ActualId = actualId, Name = "推荐歌曲" };
            case NeteaseTypeIds.RecommendedPlaylists:
                return new NeteaseRecommendPlaylistContainer { ActualId = actualId, Name = "推荐歌单" };
            case NeteaseTypeIds.Chart:
                return new NeteaseToplistContainer { ActualId = actualId, Name = "排行榜" };
            case NeteaseTypeIds.PlaylistCategory:
                return new NeteasePlaylistCategoryContainer
                {
                    ActualId = actualId,
                    Category = string.IsNullOrWhiteSpace(actualId) ? "官方" : actualId,
                    Name = string.IsNullOrWhiteSpace(actualId) ? "官方推荐歌单" : $"{actualId}歌单"
                };
            case NeteaseTypeIds.ContextRecommendation:
                return new NeteaseContextRecommendationContainer
                {
                    ActualId = actualId,
                    Name = "上下文推荐",
                    SeedItemId = actualId
                };
        }

        return null;
    }

    public async Task<List<ProvidableItemBase>> GetProvidableItemsRangeAsync(
        List<string> inProviderIds, CancellationToken ctk = default)
    {
        if (inProviderIds.Count == 0) return new List<ProvidableItemBase>();
        var grouped = inProviderIds.GroupBy(t => t.Substring(0, 2)).ToList();
        if (grouped.Count > 1)
        {
            var result = new List<ProvidableItemBase>();
            foreach (var group in grouped)
                result.AddRange(await GetProvidableItemsRangeAsync(group.ToList(), ctk));
            return result;
        }

        switch (grouped[0].Key)
        {
            case NeteaseTypeIds.SingleSong:
                return (await GetSingleSongRangeByIds(inProviderIds.Select(t => t.Substring(2)).ToList(), ctk))
                       .Select(t => (ProvidableItemBase)t).ToList();
            case NeteaseTypeIds.Playlist:
                return (await GetPlaylistRangeByIds(inProviderIds.Select(t => t.Substring(2)).ToList(), ctk))
                       .Select(t => (ProvidableItemBase)t).ToList();
            case NeteaseTypeIds.Album:
            case NeteaseTypeIds.Artist:
            case NeteaseTypeIds.RadioChannel:
            case NeteaseTypeIds.User:
                var result = new List<ProvidableItemBase>();
                foreach (var id in inProviderIds)
                {
                    var item = await GetProvidableItemByIdAsync(id, ctk);
                    if (item is not null) result.Add(item);
                }
                return result;
        }

        return new List<ProvidableItemBase>();
    }

    #region ProvidableItemGet

    public async Task<NeteaseSong?> GetSingleSongById(string id, CancellationToken cancellationToken = default)
    {
        var songResult = await RequestAsync(NeteaseApis.SongDetailApi,
                                            new SongDetailRequest()
                                            {
                                                Id = id
                                            }, cancellationToken);
        return songResult.Match(
            success => success.Songs?.FirstOrDefault()?.MapToNeteaseMusic(),
            _ => null
        );
    }

    public async Task<NeteasePlaylist?> GetPlaylistById(string id, CancellationToken cancellationToken = default)
    {
        var songResult = await RequestAsync(NeteaseApis.PlaylistDetailApi,
                                            new PlaylistDetailRequest()
                                            {
                                                Id = id
                                            }, cancellationToken);
        return songResult.Match(
            success => success.Playlists?.FirstOrDefault()?.MapToNeteasePlaylist(),
            _ => null
        );
    }

    public async Task<NeteaseAlbum?> GetAlbumById(string id, CancellationToken cancellationToken = default)
    {
        var albumResult = await RequestAsync(NeteaseApis.AlbumApi,
                                             new AlbumRequest
                                             {
                                                 Id = id
                                             }, cancellationToken);
        return albumResult.Match(
            success =>
            {
                var album = success.Album.MapToNeteaseAlbum();
                if (album is not null)
                {
                    album.Songs = success.Songs?
                                      .Select(song => (ProvidableItemBase)song.MapToNeteaseMusic())
                                      .ToList()
                                  ?? [];
                }

                return album;
            },
            _ => null
        );
    }

    public async Task<NeteaseArtist?> GetArtistById(string id, CancellationToken cancellationToken = default)
    {
        var artistResult = await RequestAsync(NeteaseApis.ArtistDetailApi,
                                              new ArtistDetailRequest
                                              {
                                                  ArtistId = id
                                              }, cancellationToken);
        return artistResult.Match(
            success => success.Artist is null
                ? null
                : new NeteaseArtist
                {
                    ActualId = success.Artist.Id,
                    Name = success.Artist.Name ?? "未知歌手"
                },
            _ => null
        );
    }

    public async Task<NeteaseRadioChannel?> GetRadioChannelById(string id, CancellationToken cancellationToken = default)
    {
        var radioResult = await RequestAsync(NeteaseApis.DjChannelDetailApi,
                                             new DjChannelDetailRequest
                                             {
                                                 Id = id
                                             }, cancellationToken);
        return radioResult.Match(
            success => success.RadioData?.MapToNeteaseRadioChannel(),
            _ => null
        );
    }

    public async Task<NeteaseUser?> GetUserById(string id, CancellationToken cancellationToken = default)
    {
        var userResult = await RequestAsync(NeteaseApis.UserDetailApi,
                                            new UserDetailRequest
                                            {
                                                UserId = id
                                            }, cancellationToken);
        return userResult.Match(
            success => success.Profile?.MapToNeteaseUser(),
            _ => null
        );
    }

    public async Task<List<NeteaseSong>> GetSingleSongRangeByIds(List<string> idList,
                                                                 CancellationToken cancellationToken = default)
    {
        var songResult = await RequestAsync(NeteaseApis.SongDetailApi,
                                            new SongDetailRequest()
                                            {
                                                IdList = idList
                                            }, cancellationToken);
        return songResult.Match(
            success => success.Songs?.Select(t => t.MapToNeteaseMusic()).ToList() ??
                       new List<NeteaseSong>(),
            _ => new List<NeteaseSong>()
        );
    }

    public async Task<List<NeteasePlaylist>> GetPlaylistRangeByIds(List<string> idList,
                                                                   CancellationToken cancellationToken = default)
    {
        var songResult = await RequestAsync(NeteaseApis.PlaylistDetailApi,
                                            new PlaylistDetailRequest()
                                            {
                                                IdList = idList
                                            }, cancellationToken);
        return songResult.Match(
            success => success.Playlists?.Select(t => t.MapToNeteasePlaylist()).ToList() ??
                       new List<NeteasePlaylist>(),
            _ => new List<NeteasePlaylist>()
        );
    }

    #endregion

    public Task<ContainerBase> SearchProvidableItemsAsync(string keyword, string typeId,
                                                                 CancellationToken ctk = default)
    {
        return Task.FromResult(new NeteaseSearchContainer
        {
            Name = "搜索结果",
            ActualId = keyword,
            SearchTypeId = TypeIdToSearchIdMapper.MapToResourceId(typeId),
            SearchKeyword = keyword,
        } as ContainerBase);
    }

    public Task<ContainerBase?> GetStoredItemsAsync(string typeId, CancellationToken ctk = default)
    {
        switch (typeId)
        {
            case NeteaseTypeIds.Playlist:
                return Task.FromResult(LoginedUser as ContainerBase);
            default:
                throw new NotImplementedException();
        }
    }

}

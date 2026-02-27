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
using HyPlayer.PlayCore.Abstraction.Models.Lyric;
using HyPlayer.PlayCore.Abstraction.Models.Resources;
using HyPlayer.PlayCore.Abstraction.Models.SingleItems;
using HyPlayer.NeteaseApi.Extensions;
using HyPlayer.NeteaseApi.ApiContracts.DjChannel;

namespace HyPlayer.NeteaseProvider;

public class NeteaseProvider : ProviderBase,
                               ILyricProvidable,
                               IMusicResourceProvidable,
                               IProvableItemLikable,
                               IProvidableItemProvidable,
                               IProvidableItemRangeProvidable,
                               ISearchableProvider,
                               IRecommendationProvidable
{
    public readonly NeteaseCloudMusicApiHandler Handler = new();
    public override string Name => "网易云音乐";
    public override string Id => "ncm";

    public static NeteaseProvider Instance = null!;

    public NeteaseUser? LoginedUser { get; set; }

    public NeteaseProvider()
    {
        Instance = this;
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

    public async Task<MusicResourceBase?> GetMusicResourceAsync(SingleSongBase song, ResourceQualityTag qualityTag,
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
            return new NeteaseMusicResource
            {
                Md5 = response.SongUrls[0].Md5,
                Size = response.SongUrls[0].Size,
                BitRate = response.SongUrls[0].BitRate,
                EncodeType = response.SongUrls[0].EncodeType,
                Time = response.SongUrls[0].Time,
                MusicType = response.SongUrls[0].Type,
                Level = response.SongUrls[0].Level,
                Uri = new Uri(response.SongUrls[0].Url ?? string.Empty)
            };
        }


        return results.Match(
            MatchSuccess,
            _ => null
        );
    }

    public async Task LikeProvidableItemAsync(string inProviderId, string? targetId, CancellationToken ctk = default)
    {
        if (inProviderId.StartsWith(NeteaseTypeIds.SingleSong))
        {
            if (targetId is null)
            {
                await RequestAsync(
                    NeteaseApis.LikeApi,
                    new LikeRequest
                    {
                        TrackId = inProviderId.Substring(2),
                        Like = true,
                        UserId = LoginedUser?.ActualId!
                    }, ctk);
            }
            else
            {
                await RequestAsync(
                    NeteaseApis.PlaylistTracksEditApi,
                    new PlaylistTracksEditRequest()
                    {
                        IsAdd = true,
                        PlaylistId = targetId,
                        Id = inProviderId.Substring(2)
                    }, ctk);
            }
        }
        else if (inProviderId.StartsWith(NeteaseTypeIds.Playlist))
        {
            await RequestAsync(NeteaseApis.PlaylistSubscribeApi,
                               new PlaylistSubscribeRequest()
                               {
                                   IsSubscribe = true,
                                   PlaylistId = inProviderId.Substring(2)
                               }, ctk);
        }
        else if (inProviderId.StartsWith(NeteaseTypeIds.Artist))
        {
            await RequestAsync(NeteaseApis.ArtistSubscribeApi,
                new ArtistSubscribeRequest()
                {
                    ArtistId = inProviderId.Substring(2)
                }, ctk);
        }
        else if (inProviderId.StartsWith(NeteaseTypeIds.Album))
        {
            await RequestAsync(NeteaseApis.AlbumSubscribeApi,
                new AlbumSubscribeRequest()
                {
                    Id = inProviderId.Substring(2),
                    IsSubscribe = true
                }, ctk);
        }
        else if (inProviderId.StartsWith(NeteaseTypeIds.Mv))
        {
            await RequestAsync(NeteaseApis.VideoSubscribeApi,
                new VideoSubscribeRequest()
                {
                    MvId = inProviderId.Substring(2)
                }, ctk);
        }
        else if (inProviderId.StartsWith(NeteaseTypeIds.RadioChannel))
        {
            await RequestAsync(NeteaseApis.DjChannelSubscribeApi,
                new DjChannelSubscribeRequest()
                {
                    Id = inProviderId.Substring(2),
                    IsSubscribe = true
                }, ctk);
        }
        else if (inProviderId.StartsWith(NeteaseTypeIds.User))
        {
            await RequestAsync(NeteaseApis.UserFollowApi,
                new UserFollowRequest()
                {
                    Id = inProviderId.Substring(2)
                }, ctk);
        }
    }

    public async Task UnlikeProvidableItemAsync(string inProviderId, string? targetId, CancellationToken ctk = default)
    {
        if (inProviderId.StartsWith(NeteaseTypeIds.SingleSong))
        {
            if (targetId is null)
            {
                await RequestAsync(
                    NeteaseApis.LikeApi,
                    new LikeRequest
                    {
                        TrackId = inProviderId.Substring(2),
                        Like = false,
                        UserId = LoginedUser?.ActualId!
                    }, ctk);
            }
            else
            {
                await RequestAsync(
                    NeteaseApis.PlaylistTracksEditApi,
                    new PlaylistTracksEditRequest()
                    {
                        IsAdd = false,
                        PlaylistId = targetId,
                        Id = inProviderId.Substring(2)
                    }, ctk);
            }
        }
        else if (inProviderId.StartsWith(NeteaseTypeIds.Playlist))
        {
            await RequestAsync(NeteaseApis.PlaylistSubscribeApi,
                               new PlaylistSubscribeRequest()
                               {
                                   IsSubscribe = false,
                                   PlaylistId = inProviderId.Substring(2)
                               }, ctk);
        }

        else if (inProviderId.StartsWith(NeteaseTypeIds.Artist))
        {
            var id = inProviderId.Substring(2);
            if (long.TryParse(id, out var lid))
            {
                await RequestAsync(NeteaseApis.ArtistUnsubscribeApi,
                    new ArtistUnsubscribeRequest()
                    {
                        ArtistIds = [lid]
                    }, ctk);
            }
        }
        else if (inProviderId.StartsWith(NeteaseTypeIds.Album))
        {
            await RequestAsync(NeteaseApis.AlbumSubscribeApi,
                new AlbumSubscribeRequest()
                {
                    Id = inProviderId.Substring(2),
                    IsSubscribe = false
                }, ctk);
        }
        else if (inProviderId.StartsWith(NeteaseTypeIds.Mv))
        {
            await RequestAsync(NeteaseApis.VideoUnsubscribeApi,
                new VideoUnsubscribeRequest()
                {
                    IdList = [ inProviderId.Substring(2)]
                }, ctk);
        }
        else if (inProviderId.StartsWith(NeteaseTypeIds.RadioChannel))
        {
            await RequestAsync(NeteaseApis.DjChannelSubscribeApi,
                new DjChannelSubscribeRequest()
                {
                    Id = inProviderId.Substring(2),
                    IsSubscribe = false
                }, ctk);
        }
        else if (inProviderId.StartsWith(NeteaseTypeIds.User))
        {
            await RequestAsync(NeteaseApis.UserUnfollowApi,
                new UserUnfollowRequest()
                {
                    Id = inProviderId.Substring(2)
                }, ctk);
        }
    }

    public async Task<List<string>> GetLikedProvidableIdsAsync(string typeId, CancellationToken ctk = default)
    {
        var result = await RequestAsync(NeteaseApis.LikelistApi, new LikelistRequest()
        {
            Uid = LoginedUser?.ActualId!
        }, ctk);
        return result.Match(
            success => success.TrackIds?.ToList() ?? new List<string>(),
            _ => new List<string>());
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
        }

        throw new NotImplementedException();
    }

    public async Task<List<ProvidableItemBase>> GetProvidableItemsRangeAsync(
        List<string> inProviderIds, CancellationToken ctk = default)
    {
        if (inProviderIds.Count == 0) return new List<ProvidableItemBase>();
        var grouped = inProviderIds.GroupBy(t => t.Substring(0, 2)).ToList();
        if (grouped.Count <= 1) throw new NotImplementedException();
        switch (grouped[0].Key)
        {
            case NeteaseTypeIds.SingleSong:
                return (await GetSingleSongRangeByIds(inProviderIds.Select(t => t.Substring(2)).ToList(), ctk))
                       .Select(t => (ProvidableItemBase)t).ToList();
            case NeteaseTypeIds.Playlist:
                return (await GetPlaylistRangeByIds(inProviderIds.Select(t => t.Substring(2)).ToList(), ctk))
                       .Select(t => (ProvidableItemBase)t).ToList();
        }

        throw new NotImplementedException();
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
            success => success.Songs?[0].MapToNeteaseMusic(),
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
            success => success.Playlists?[0].MapToNeteasePlaylist(),
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

    public Task<ContainerBase> GetRecommendationAsync(string? typeId = null, CancellationToken ctk = default)
    {
        // ReSharper disable once ConvertIfStatementToSwitchStatement
        if (typeId == NeteaseTypeIds.Playlist) // 推荐歌单
            return Task.FromResult(
                new NeteaseActionGettableContainer(async () =>
                {
                    return (await RequestAsync(
                            NeteaseApis.RecommendPlaylistsApi,
                            new RecommendPlaylistsRequest(), ctk))
                        .Match(success => success.Recommends?.Select(
                                   t => (ProvidableItemBase)
                                       t.MapToNeteasePlaylist()).ToList() ?? new List<ProvidableItemBase>(),
                               error => throw error);
                })
                {
                    Name = "推荐歌单",
                    ActualId = "rcpl"
                } as ContainerBase);
        if (typeId == NeteaseTypeIds.SingleSong) // 推荐歌曲
            return Task.FromResult(new NeteaseActionGettableContainer(async () =>
            {
                return (await RequestAsync(
                        NeteaseApis.RecommendSongsApi,
                        new RecommendSongsRequest(), ctk))
                    .Match(success => success.Data?.DailySongs?.Select(
                               t => (ProvidableItemBase)
                                   t.MapToNeteaseMusic()).ToList() ?? new List<ProvidableItemBase>(),
                           error => throw error);
            })
            {
                Name = "推荐歌曲",
                ActualId = "rcsg"
            } as ContainerBase);
        if (typeId == NeteaseTypeIds.Chart) // 排行榜
            return Task.FromResult(new NeteaseActionGettableContainer(async () =>
                   {
                       return (await RequestAsync(
                               NeteaseApis.ToplistApi,
                               new ToplistRequest(), ctk))
                           .Match(success => success.List?.Select(
                                      t => (ProvidableItemBase)
                                          t.MapToNeteasePlaylist()).ToList() ?? new List<ProvidableItemBase>(),
                                  error => throw error);
                   })
            {
                Name = "排行榜",
                ActualId = "chart"
            } as ContainerBase);
        if (typeId?.StartsWith(NeteaseTypeIds.PlaylistCategory) is true)
        {
            return Task.FromResult(new NeteaseActionGettableProgressiveContainer(async (start, count) =>
                   {
                       return (true, (await RequestAsync(
                                   NeteaseApis.PlaylistCategoryListApi,
                                   new PlaylistCategoryListRequest
                                   {
                                       Category = typeId.Substring(2),
                                       Limit = count
                                   }, ctk))
                               .Match(success => success.Playlists?.Select(
                                          t => (ProvidableItemBase)
                                              t.MapToNeteasePlaylist()).ToList() ?? new List<ProvidableItemBase>(),
                                      error => throw error));
                   })
            {
                Name = "官方推荐歌单",
                ActualId = typeId.Substring(2),
                MaxProgressiveCount = 15
            } as ContainerBase);
        }

        if (typeId == NeteaseTypeIds.PersonalFm)
        {
            return Task.FromResult(new NeteasePersonalFMContainer
            {
                Name = "私人FM",
                ActualId = ""
            } as ContainerBase);
        }

        throw new ArgumentException(typeId);
    }
}
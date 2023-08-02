using HyPlayer.NeteaseApi;
using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.NeteaseProvider.Models;
using HyPlayer.PlayCore.Abstraction;
using HyPlayer.PlayCore.Abstraction.Interfaces.Provider;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Lyric;
using HyPlayer.PlayCore.Abstraction.Models.Resources;
using HyPlayer.PlayCore.Abstraction.Models.SingleItems;
using Kengwang.Toolkit;

namespace HyPlayer.NeteaseProvider;

public class NeteaseProvider : ProviderBase,
                               ILyricProvidable,
                               IMusicResourceProvidable,
                               IProvableItemLikable,
                               IProvidableItemProvidable,
                               IProvidableItemRangeProvidable,
                               ISearchableProvider,
                               IStoredItemsProvidable,
                               IRecommendationProvidable,
                               IProvidableItemUpdatable
{
    public ApiHandlerOption Option { get; set; } = new();
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
            new("sg", "歌曲", true),
            new ("pl", "歌单", true),
            new ("ar","歌手",true),
            new("al","专辑",true),
            new ("us","用户",true),
            new("rd","节目",true),// 电台节目
            new ("dj","电台",true),
            new ("se","搜索结果", false),
            new ("ag","歌曲容器", false)
        };
    

    public async Task<Results<TResponse, ErrorResultBase>> RequestAsync<TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract,
        CancellationToken cancellationToken = default)
        where TError : ErrorResultBase
        where TActualRequest : ActualRequestBase
        where TRequest : RequestBase
    {
        try
        {
            return await Handler.RequestAsync(contract, Option, cancellationToken);
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
    {
        try
        {
            return await Handler.RequestAsync(contract, request, Option, cancellationToken);
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
        where TError : ErrorResultBase where TActualRequest : ActualRequestBase where TRequest : RequestBase
    {
        try
        {
            return await Handler.RequestAsync<TCustomResponse, TRequest, TResponse, TError, TActualRequest>(
                contract, request, Option, cancellationToken);
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
        where TError : ErrorResultBase where TActualRequest : ActualRequestBase where TRequest : RequestBase
    {
        try
        {
            return await Handler
                .RequestAsync<TCustomRequest, TCustomResponse, TRequest, TResponse, TError, TActualRequest>(
                    contract, true, request, Option, cancellationToken);
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
        where TError : ErrorResultBase where TActualRequest : ActualRequestBase where TRequest : RequestBase
    {
        try
        {
            return await Handler.RequestAsync(
                contract, true, request, Option, cancellationToken);
        }
        catch (Exception ex)
        {
            return Results<TResponse, ErrorResultBase>.CreateError(
                new ExceptionedErrorBase(-500, ex.Message, ex));
        }
    }

    public async Task<bool> LoginEmail(string email, string password, bool isMd5 = false)
    {
        var request = new LoginEmailRequest()
                      {
                          Email = email,
                      };
        if (isMd5)
            request.Md5Password = password;
        else
            request.Password = password;
        var result = await RequestAsync(NeteaseApis.LoginEmailApi, request);
        return result.Match(
            success =>
            {
                LoginedUser = success.Profile?.MapToNeteaseUser();
                return true;
            },
            _ => false
        );
    }

    public async Task<bool> LoginCellphone(string cellphone, string password, bool isMd5 = false)
    {
        var request = new LoginCellphoneRequest()
                      {
                          Cellphone = cellphone
                      };
        if (isMd5)
            request.Md5Password = password;
        else
            request.Password = password;
        var result = await RequestAsync(NeteaseApis.LoginCellphoneApi, request);
        return result.Match(
            success =>
            {
                LoginedUser = success.Profile?.MapToNeteaseUser();
                return true;
            },
            _ => false
        );
    }

    public async Task<List<RawLyricInfo>> GetLyricInfo(SingleSongBase song)
    {
        var results = await RequestAsync(NeteaseApis.LyricApi, new LyricRequest() { Id = song.Id });
        return results.Match(
            success => success.Map(),
            _ => new()
        ).Select(t => (RawLyricInfo)t).ToList();
    }

    public async Task<MusicResourceBase?> GetMusicResource(SingleSongBase song, ResourceQualityTag qualityTag)
    {
        var quality = "exhigh";
        if (qualityTag is NeteaseMusicQualityTag neteaseMusicQualityTag)
            quality = neteaseMusicQualityTag.Quality;
        var results = await RequestAsync(NeteaseApis.SongUrlApi,
                                         new SongUrlRequest
                                         {
                                             Id = song.Id,
                                             Level = quality
                                         });

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
                       Url = response.SongUrls[0].Url,
                   };
        }


        return results.Match(
            MatchSuccess,
            _ => null
        );
    }

    public async Task LikeProvidableItem(string inProviderId, string? targetId)
    {
        if (inProviderId.StartsWith("sg"))
        {
            if (targetId is null)
            {
                await RequestAsync(
                    NeteaseApis.LikeApi,
                    new LikeRequest
                    {
                        TrackId = inProviderId.Substring(2),
                        Like = true
                    });
            }
            else
            {
                await RequestAsync(
                    NeteaseApis.PlaylistTracksEditApi,
                    new PlaylistTracksEditRequest()
                    {
                        IsAdd = true,
                        PlaylistId = targetId,
                        TrackId = inProviderId.Substring(2)
                    }
                );
            }
        }
        else if (inProviderId.StartsWith("pl"))
        {
            await RequestAsync(NeteaseApis.PlaylistSubscribeApi,
                               new PlaylistSubscribeRequest()
                               {
                                   IsSubscribe = true,
                                   PlaylistId = inProviderId.Substring(2)
                               });
        }
        // TODO
    }

    public async Task UnlikeProvidableItem(string inProviderId, string? targetId)
    {
        if (inProviderId.StartsWith("sg"))
        {
            if (targetId is null)
            {
                await RequestAsync(
                    NeteaseApis.LikeApi,
                    new LikeRequest
                    {
                        TrackId = inProviderId.Substring(2),
                        Like = false
                    });
            }
            else
            {
                await RequestAsync(
                    NeteaseApis.PlaylistTracksEditApi,
                    new PlaylistTracksEditRequest()
                    {
                        IsAdd = false,
                        PlaylistId = targetId,
                        TrackId = inProviderId.Substring(2)
                    }
                );
            }
        }
        else if (inProviderId.StartsWith("pl"))
        {
            await RequestAsync(NeteaseApis.PlaylistSubscribeApi,
                               new PlaylistSubscribeRequest()
                               {
                                   IsSubscribe = true,
                                   PlaylistId = inProviderId.Substring(2)
                               });
        }
        // TODO
    }

    public async Task<List<string>> GetLikedProvidableIds(string typeId)
    {
        var result = await RequestAsync(NeteaseApis.LikelistApi, new LikelistRequest()
                                                                 {
                                                                     Uid = LoginedUser?.Id!
                                                                 });
        return result.Match(
            success => success.TrackIds.ToList(),
            _ => new List<string>());
    }

   
    
    public async Task<ProvidableItemBase?> GetProvidableItemById(string inProviderId)
    {
        var typeId = inProviderId.Substring(0, 2);
        var actualId = inProviderId.Substring(2);
        switch (typeId)
        {
            case "sg":
                var sg = new NeteaseSong
                         {
                             ActualId = actualId,
                             Artists = null,
                             Name = string.Empty
                         };
                return await UpdateProvidableItemInfo(sg);
            case "pl":
                var pl = new NeteasePlaylist()
                         {
                             ActualId = actualId,
                             Name = string.Empty
                         };
                return await UpdateProvidableItemInfo(pl);
        }
        throw new NotImplementedException();
    }

    public async Task<List<ProvidableItemBase>> GetProvidableItemsRange(List<string> inProviderIds)
    {
        var grouped = inProviderIds.GroupBy(t => t.Substring(0, 2)).ToList();
        if (grouped.Count == 1)
        {
            // 一种的话直接来吧
            // 之后可以对此处逻辑进行拆解
            switch (grouped[0].Key)
            {
                case "sg":
                    var songResult = await RequestAsync(NeteaseApis.SongDetailApi,
                                                        new SongDetailRequest()
                                                        {
                                                            IdList = inProviderIds.Select(t => t.Substring(2)).ToList()
                                                        });
                    return songResult.Match(
                        success => success.Songs?.Select(t => (ProvidableItemBase)t.MapToNeteaseMusic()).ToList() ??
                                   new List<ProvidableItemBase>(),
                        _ => new List<ProvidableItemBase>()
                    );
            }
        }

        throw new NotImplementedException();
    }

    public async Task<ContainerBase?> SearchProvidableItems(string keyword, string typeId)
    {
        return new NeteaseSearchContainer
               {
                   Name = "搜索结果",
                   ActualId = keyword,
                   SearchTypeId = TypeIdToSearchIdMapper.MapToResourceId(typeId),
                   SearchKeyword = keyword,
               };
    }

    public async Task<ContainerBase?> GetStoredItems(string typeId)
    {
        throw new NotImplementedException();
    }

    public async Task<ContainerBase?> GetRecommendation(string? typeId = null)
    {
        switch (typeId)
        {
            case "pl": // 推荐歌单
                return new NeteaseActionGettableContainer(async () =>
                       {
                           return (await RequestAsync(
                                   NeteaseApis.RecommendPlaylistsApi,
                                   new RecommendPlaylistsRequest()))
                               .Match(success => success.Recommends?.Select(
                                          t => (ProvidableItemBase)
                                              t.MapToNeteasePlaylist()).ToList() ?? new List<ProvidableItemBase>(),
                                      error => throw error);
                       })
                       {
                           Name = "推荐歌单",
                           ActualId = "rcpl"
                       };
            case "sg": // 推荐歌曲
                return new NeteaseActionGettableContainer(async () =>
                       {
                           return (await RequestAsync(
                                   NeteaseApis.RecommendSongsApi,
                                   new RecommendSongsRequest()))
                               .Match(success => success.Data?.DailySongs?.Select(
                                          t => (ProvidableItemBase)
                                              t.MapToNeteaseMusic()).ToList() ?? new List<ProvidableItemBase>(),
                                      error => throw error);
                       })
                       {
                           Name = "推荐歌曲",
                           ActualId = "rcsg"
                       };
            case "ct": // 排行榜
                return new NeteaseActionGettableContainer(async () =>
                       {
                           return (await RequestAsync(
                                   NeteaseApis.ToplistApi,
                                   new ToplistRequest()))
                               .Match(success => success.List?.Select(
                                          t => (ProvidableItemBase)
                                              t.MapToNeteasePlaylist()).ToList() ?? new List<ProvidableItemBase>(),
                                      error => throw error);
                       })
                       {
                           Name = "排行榜",
                           ActualId = "chart"
                       };
            default:
                throw new NotImplementedException();
        }
    }

    public async Task<ProvidableItemBase?> UpdateProvidableItemInfo(ProvidableItemBase providableItem)
    {
        var actualId = providableItem.ActualId;
        var typeId = providableItem.TypeId;
        // 之后可以对此处逻辑进行拆解
        switch (typeId)
        {
            case "sg":
                var songResult = await RequestAsync(NeteaseApis.SongDetailApi,
                                                    new SongDetailRequest()
                                                    {
                                                        Id = actualId
                                                    });
                return songResult.Match(
                    success => success.Songs?[0].MapToNeteaseMusic(),
                    _ => null
                );
            case "pl":
                var playlistResult = await RequestAsync(
                    NeteaseApis.PlaylistDetailApi,
                    new PlaylistDetailRequest
                    {
                        Id = actualId
                    });
                return playlistResult.Match(
                    success => success.Playlist?.MapToNeteasePlaylist(),
                    _ => null
                );
        }

        throw new NotImplementedException();
    }
}
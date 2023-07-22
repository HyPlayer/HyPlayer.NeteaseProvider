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
using HyPlayer.PlayCore.Abstraction.Models.Songs;
using Kengwang.Toolkit;

namespace HyPlayer.NeteaseProvider;

public class NeteaseProvider : ProviderBase,
                               ILyricProvidable,
                               IMusicResourceProvidable,
                               IProvableItemLikable,
                               IProvidableItemProvidable,
                               IProvidableItemRangeProvidable,
                               ISearchableProvider
{
    public ApiHandlerOption Option { get; set; } = new();
    private readonly NeteaseCloudMusicApiHandler _handler = new();
    public override string Name => "网易云音乐";
    public override string Id => "ncm";

    public static NeteaseProvider Instance = null!;

    public NeteaseUser? LoginedUser { get; set; }

    public NeteaseProvider()
    {
        Instance = this;
    }

    public override Dictionary<string, string> TypeIdToNameDictionary
        => new()
           {
               { "sg", "歌曲" },
               { "pl", "歌单" },
               { "ar", "歌手" },
               { "al", "专辑" },
               { "us", "用户" },
               { "rd", "电台" },
           };

    public Task<Results<TResponse, ErrorResultBase>> RequestAsync<TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract)
        where TError : ErrorResultBase
        where TActualRequest : ActualRequestBase
        where TRequest : RequestBase
    {
        try
        {
            return _handler.RequestAsync(contract, Option);
        }
        catch (Exception ex)
        {
            return Task.FromResult(Results<TResponse, ErrorResultBase>.CreateError(new ExceptionedErrorBase(-500, ex.Message, ex)));
        }
        
    }

    public Task<Results<TResponse, ErrorResultBase>> RequestAsync<TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, TRequest request)
        where TError : ErrorResultBase
        where TActualRequest : ActualRequestBase
        where TRequest : RequestBase
    {
        try
        {
            return _handler.RequestAsync(contract, request, Option);
        }
        catch (Exception ex)
        {
            return Task.FromResult(Results<TResponse, ErrorResultBase>.CreateError(new ExceptionedErrorBase(-500, ex.Message, ex)));
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
            error => false
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
            error => false
        );
    }

    public async Task<List<RawLyricInfo>> GetLyricInfo(SingleSongBase song)
    {
        var results = await RequestAsync(NeteaseApis.LyricApi, new LyricRequest() { Id = song.Id });
        return results.Match(
            success => success.Map(),
            error => new()
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
                    new()
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
                               new()
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
                    new()
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
                               new()
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
            error => new List<string>());
    }

    public async Task<ProvidableItemBase?> GetProvidableItemById(string inProviderId)
    {
        var typeId = inProviderId.Substring(0, 2);
        // 之后可以对此处逻辑进行拆解
        switch (typeId)
        {
            case "ns":
                var songResult = await RequestAsync(NeteaseApis.SongDetailApi,
                                                    new()
                                                    {
                                                        Id = inProviderId.Substring(2)
                                                    });
                return songResult.Match(
                    success => success.Songs?[0].MapToNeteaseMusic(),
                    error => null
                );
                break;
            case "pl":
                var playlistResult = await RequestAsync(
                    NeteaseApis.PlaylistDetailApi,
                    new PlaylistDetailRequest
                    {
                        Id = inProviderId.Substring(2)
                    });
                return playlistResult.Match(
                    success => success.Playlist?.MapToNeteasePlaylist(),
                    error => null
                );
                break;
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
                case "ns":
                    var songResult = await RequestAsync(NeteaseApis.SongDetailApi,
                                                        new()
                                                        {
                                                            IdList = inProviderIds.Select(t => t.Substring(2)).ToList()
                                                        });
                    return songResult.Match(
                        success => success.Songs?.Select(t => (ProvidableItemBase)t.MapToNeteaseMusic()).ToList() ??
                                   new List<ProvidableItemBase>(),
                        error => new List<ProvidableItemBase>()
                    );
            }
        }

        throw new NotImplementedException();
    }

    public async Task<ContainerBase?> SearchProvidableItems(string keyword, string typeId)
    {
        throw new NotImplementedException();
    }
}
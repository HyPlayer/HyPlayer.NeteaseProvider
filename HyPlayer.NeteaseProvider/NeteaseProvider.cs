using System.Collections.ObjectModel;
using HyPlayer.NeteaseProvider.ApiContracts;
using HyPlayer.NeteaseProvider.Bases;
using HyPlayer.NeteaseProvider.Extensions;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.NeteaseProvider.Models;
using HyPlayer.NeteaseProvider.Requests;
using HyPlayer.NeteaseProvider.Responses;
using HyPlayer.PlayCore.Abstraction;
using HyPlayer.PlayCore.Abstraction.Interfaces.Provider;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Lyric;
using HyPlayer.PlayCore.Abstraction.Models.Resources;
using HyPlayer.PlayCore.Abstraction.Models.Songs;

namespace HyPlayer.NeteaseProvider;

public class NeteaseProvider : ProviderBase,
                               ILyricProvidable,
                               IMusicResourceProvidable,
                               IProvableItemLikable,
                               IProvidableItemProvidable,
                               IProvidableItemRangeProvidable,
                               ISearchableProvider
{
    public ProviderOption Option { get; set; } = new();
    private readonly NeteaseCloudMusicApiHandler _handler = new();
    public override string Name => "网易云音乐";
    public override string Id => "ncm";

    public Task<Results<TResponse, ErrorResultBase>> RequestAsync<TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract)
        where TError : ErrorResultBase
        where TActualRequest : ActualRequestBase
        where TRequest : RequestBase
    {
        return _handler.RequestAsync(contract, Option);
    }

    public Task<Results<TResponse, ErrorResultBase>> RequestAsync<TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, TRequest request)
        where TError : ErrorResultBase
        where TActualRequest : ActualRequestBase
        where TRequest : RequestBase
    {
        return _handler.RequestAsync(contract, request, Option);
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
        throw new NotImplementedException();
    }

    public async Task UnlikeProvidableItem(string inProviderId, string? targetId)
    {
        throw new NotImplementedException();
    }

    public async Task<ReadOnlyCollection<string>> GetLikedProvidableIds(string typeId)
    {
        throw new NotImplementedException();
    }

    public async Task<ProvidableItemBase> GetProvidableItemById(string inProviderId)
    {
        throw new NotImplementedException();
    }

    public async Task<ReadOnlyCollection<ProvidableItemBase>> GetProvidableItemsRange(List<string> inProviderIds)
    {
        throw new NotImplementedException();
    }

    public async Task<ContainerBase> SearchProvidableItems(string keyword, string typeId)
    {
        throw new NotImplementedException();
    }
}
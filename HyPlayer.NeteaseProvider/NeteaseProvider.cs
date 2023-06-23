using System.Collections.ObjectModel;
using HyPlayer.NeteaseProvider.Bases;
using HyPlayer.NeteaseProvider.Extensions;
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
        throw new NotImplementedException();
    }

    public async Task<MusicResourceBase?> GetMusicResource(SingleSongBase song)
    {
        throw new NotImplementedException();
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
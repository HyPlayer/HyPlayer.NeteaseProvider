using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.ApiContracts.Playlist;
using HyPlayer.NeteaseApi.ApiContracts.Recommend;
using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.PlayCore.Abstraction.Interfaces.PlayListContainer;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;

namespace HyPlayer.NeteaseProvider.Models;

public sealed class NeteaseRecommendPlaylistContainer : LinerContainerBase
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.Playlist;

    public override async Task<List<ProvidableItemBase>> GetAllItemsAsync(CancellationToken ctk = default)
    {
        return (await NeteaseProvider.Instance.RequestAsync(
                NeteaseApis.RecommendPlaylistsApi,
                new RecommendPlaylistsRequest(), ctk))
            .Match(success => success.Recommends?.Select(t => (ProvidableItemBase)t.MapToNeteasePlaylist()).ToList() ?? [],
                error => throw error);
    }
}

public sealed class NeteaseRecommendSongContainer : LinerContainerBase
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.SingleSong;

    public override async Task<List<ProvidableItemBase>> GetAllItemsAsync(CancellationToken ctk = default)
    {
        return (await NeteaseProvider.Instance.RequestAsync(
                NeteaseApis.RecommendSongsApi,
                new RecommendSongsRequest(), ctk))
            .Match(success => success.Data?.DailySongs?.Select(t => (ProvidableItemBase)t.MapToNeteaseMusic()).ToList() ?? [],
                error => throw error);
    }
}

public sealed class NeteaseToplistContainer : LinerContainerBase
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.Chart;

    public override async Task<List<ProvidableItemBase>> GetAllItemsAsync(CancellationToken ctk = default)
    {
        return (await NeteaseProvider.Instance.RequestAsync(
                NeteaseApis.ToplistApi,
                new ToplistRequest(), ctk))
            .Match(success => success.List?.Select(t => (ProvidableItemBase)t.MapToNeteasePlaylist()).ToList() ?? [],
                error => throw error);
    }
}

public sealed class NeteasePlaylistCategoryContainer : LinerContainerBase, IProgressiveLoadingContainer
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.PlaylistCategory;

    public required string Category { get; init; }
    public int MaxProgressiveCount { get; init; } = 15;

    public override async Task<List<ProvidableItemBase>> GetAllItemsAsync(CancellationToken ctk = default)
    {
        return (await GetProgressiveItemsListAsync(0, MaxProgressiveCount, ctk)).Item2;
    }

    public async Task<(bool, List<ProvidableItemBase>)> GetProgressiveItemsListAsync(int start, int count, CancellationToken ctk = default)
    {
        return (true, (await NeteaseProvider.Instance.RequestAsync(
                    NeteaseApis.PlaylistCategoryListApi,
                    new PlaylistCategoryListRequest
                    {
                        Category = Category,
                        Limit = count
                    }, ctk))
                .Match(success => success.Playlists?.Select(t => (ProvidableItemBase)t.MapToNeteasePlaylist()).ToList() ?? [],
                    error => throw error));
    }
}

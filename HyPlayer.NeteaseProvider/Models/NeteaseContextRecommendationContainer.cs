using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.ApiContracts.Playlist;
using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;

namespace HyPlayer.NeteaseProvider.Models;

public sealed class NeteaseContextRecommendationContainer : LinerContainerBase
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.SingleSong;

    public required string SeedItemId { get; init; }
    public int Count { get; init; } = 10;

    public override async Task<List<ProvidableItemBase>> GetAllItemsAsync(CancellationToken ctk = default)
    {
        var result = await NeteaseProvider.Instance.RequestAsync(NeteaseApis.PlaymodeIntelligenceListApi,
            new PlaymodeIntelligenceListRequest
            {
                SongId = SeedItemId,
                PlaylistId = string.Empty,
                StartMusicId = SeedItemId,
                Count = Count
            }, ctk);

        return result.Match(
            success => success.Data?.Select(item => item.SongInfo).Where(song => song is not null).Select(song => (ProvidableItemBase)song!.MapToNeteaseMusic()).ToList() ?? [],
            _ => []);
    }
}

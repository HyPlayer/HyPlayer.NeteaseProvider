using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.ApiContracts.Playlist;
using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;

namespace HyPlayer.NeteaseProvider.Models;

public sealed class NeteaseContextRecommendationContainer : LinerContainerBase
{
    private const char PayloadSeparator = '|';

    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.SingleSong;

    public string PlaylistId { get; init; } = string.Empty;
    public required string SeedItemId { get; init; }
    public required string StartMusicId { get; init; }
    public int Count { get; init; } = 10;

    public static string CreateActualId(string? playlistId, string seedItemId, string? startMusicId = null)
    {
        return string.Join(PayloadSeparator.ToString(),
            playlistId ?? string.Empty,
            seedItemId,
            string.IsNullOrWhiteSpace(startMusicId) ? seedItemId : startMusicId);
    }

    public static NeteaseContextRecommendationContainer CreateFromActualId(string actualId)
    {
        var parts = actualId.Split(new[] { PayloadSeparator }, 3);
        var hasPayload = parts.Length == 3;
        var seedItemId = hasPayload ? parts[1] : actualId;
        var startMusicId = hasPayload && !string.IsNullOrWhiteSpace(parts[2]) ? parts[2] : seedItemId;

        return new NeteaseContextRecommendationContainer
        {
            ActualId = actualId,
            Name = "上下文推荐",
            PlaylistId = hasPayload ? parts[0] : string.Empty,
            SeedItemId = seedItemId,
            StartMusicId = startMusicId
        };
    }

    public override async Task<List<ProvidableItemBase>> GetAllItemsAsync(CancellationToken ctk = default)
    {
        var result = await NeteaseProvider.Instance.RequestAsync(NeteaseApis.PlaymodeIntelligenceListApi,
            new PlaymodeIntelligenceListRequest
            {
                SongId = SeedItemId,
                PlaylistId = PlaylistId,
                StartMusicId = StartMusicId,
                Count = Count
            }, ctk);

        return result.Match(
            success => success.Data?.Select(item => item.SongInfo).Where(song => song is not null).Select(song => (ProvidableItemBase)song!.MapToNeteaseMusic()).ToList() ?? [],
            _ => []);
    }
}

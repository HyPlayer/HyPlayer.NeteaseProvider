using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.PlayCore.Abstraction.Interfaces.ProvidableItem;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.Resources;
using HyPlayer.PlayCore.Abstraction.Models.SingleItems;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseSong : SingleSongBase, IHasTranslation, IHasCover
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.SingleSong;

    public string[]? Alias { get; set; }
    public bool HasCopyright { get; set; }
    public string? MvId { get; set; }
    public string? CdName { get; set; }
    public int TrackNumber { get; set; }
    public string? CoverUrl { get; set; }

    public required List<PersonBase>? Artists { get; init; }

    public override Task<List<PersonBase>?> GetCreatorsAsync(CancellationToken ctk = default)
    {
        return Task.FromResult(Artists);
    }

    public string? Translation { get; set; }

    public Task<ResourceResultBase> GetCoverAsync(ImageResourceQualityTag? qualityTag = null, CancellationToken ctk = default)
    {
        if (qualityTag is NeteaseImageResourceQualityTag neteaseImageResourceQualityTag)
        {
            var result = new NeteaseImageResourceResult()
            {
                ExternalException = null,
                ResourceStatus = ResourceStatus.Success,
                Uri = new Uri($"{CoverUrl}?{neteaseImageResourceQualityTag}")
            };
            return Task.FromResult(result as ResourceResultBase);
        }
        else
        {
            var result = new NeteaseImageResourceResult()
            {
                ExternalException = null,
                ResourceStatus = ResourceStatus.Success,
                Uri = new Uri($"{CoverUrl}")
            };
            return Task.FromResult(result as ResourceResultBase);
        }
    }
}
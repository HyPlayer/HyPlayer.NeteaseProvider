using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.PlayCore.Abstraction.Interfaces.ProvidableItem;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.Resources;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseVideo : ProvidableItemBase, IHasCover, IHasCreators
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.MBlog;
    public string? CoverUrl { get; set; }
    public long Duration { get; set; }
    public long PlayCount { get; set; }
    public List<string>? CreatorList { get; init; }
    public List<PersonBase>? Creators { get; init; }

    public Task<List<PersonBase>?> GetCreatorsAsync(CancellationToken ctk = default)
    {
        return Task.FromResult(Creators);
    }

    public Task<ResourceResultBase> GetCoverAsync(ImageResourceQualityTag? qualityTag = null, CancellationToken ctk = default)
    {
        return Task.FromResult<ResourceResultBase>(NeteaseImageResourceResult.FromUrl(CoverUrl, qualityTag));
    }
}
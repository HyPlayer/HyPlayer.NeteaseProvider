using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.PlayCore.Abstraction.Interfaces.ProvidableItem;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.Resources;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseMv : RichMediaBase, IHasCover
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.Mv;
    public string? CoverUrl { get; set; }
    public string? CreatorName { get; set; }
    public string? PublishTime { get; set; }
    public long PlayCount { get; set; }
    public long LikedCount { get; set; }
    public long SubCount { get; set; }
    public List<int> AvailableQualities { get; set; } = [];
    public async Task<ResourceResultBase> GetCoverAsync(ImageResourceQualityTag? qualityTag = null, CancellationToken ctk = default)
    {
        if (qualityTag is NeteaseImageResourceQualityTag neteaseImageResourceQualityTag)
        {
            var result = new NeteaseImageResourceResult()
            {
                ExternalException = null,
                ResourceStatus = ResourceStatus.Success,
                Uri = new Uri($"{CoverUrl}?{neteaseImageResourceQualityTag.ToString()}")
            };
            return result;
        }
        else
        {
            var result = new NeteaseImageResourceResult()
            {
                ExternalException = null,
                ResourceStatus = ResourceStatus.Success,
                Uri = new Uri($"{CoverUrl}")
            };
            return result;
        }
    }
}

public class NeteaseMlog : RichMediaBase, IHasCover
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.MBlog;
    public string? CoverUrl { get; set; }
    public string? CreatorName { get; set; }
    public long PublishTime { get; set; }
    public long LikedCount { get; set; }

    public Task<ResourceResultBase> GetCoverAsync(ImageResourceQualityTag? qualityTag = null, CancellationToken ctk = default)
    {
        return Task.FromResult<ResourceResultBase>(new NeteaseImageResourceResult
        {
            ExternalException = null,
            ResourceStatus = ResourceStatus.Success,
            Uri = string.IsNullOrWhiteSpace(CoverUrl) ? null : new Uri(CoverUrl)
        });
    }
}

public sealed class NeteaseRichMediaResource : ResourceBase
{
    public override ResourceType Type => ResourceType.Video;
    public required string Url { get; init; }

    public override Task<ResourceResultBase> GetResourceAsync(ResourceQualityTag? qualityTag = null, CancellationToken ctk = default)
    {
        return Task.FromResult<ResourceResultBase>(new NeteaseRichMediaResourceResult
        {
            ExternalException = null,
            ResourceStatus = ResourceStatus.Success,
            Uri = new Uri(Url)
        });
    }
}

public sealed class NeteaseRichMediaResourceResult : ResourceResultBase, IResourceResultOf<Uri?>
{
    public override Exception? ExternalException { get; init; }
    public override required ResourceStatus ResourceStatus { get; init; }
    public required Uri? Uri { get; init; }

    public Task<Uri?> GetResourceAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Uri);
    }
}

using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Resources;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseImageResource : ImageResourceBase
{
    public override Task<ResourceResultBase> GetResourceAsync(ResourceQualityTag? qualityTag = null, CancellationToken ctk = default)
    {
        return Task.FromResult<ResourceResultBase>(new NeteaseImageResourceResult
        {
            ExternalException = null,
            ResourceStatus = ResourceStatus.Success,
            Uri = new Uri(Uri!, $"?{qualityTag}")
        });
    }
}
public class NeteaseImageResourceResult : ResourceResultBase, IResourceResultOf<Uri?>
{
    public override Exception? ExternalException { get; init; }
    public override required ResourceStatus ResourceStatus { get; init; }
    public required Uri? Uri { get; init; }
    public Task<Uri?> GetResourceAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Uri);
    }
}
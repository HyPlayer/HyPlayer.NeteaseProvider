using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Resources;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseMusicResource : MusicResourceBase
{
    public string? Md5 { get; set; }
    public long Size { get; set; }
    public string? BitRate { get; set; }
    public string? EncodeType { get; set; }
    public long? Time { get; set; }
    public string? MusicType { get; set; }
    public string? Level { get; set; }

    public override Task<ResourceResultBase> GetResourceAsync(ResourceQualityTag? qualityTag = null, CancellationToken ctk = default)
    {
        return Task.FromResult(new NeteaseMusicResourceResult { ResourceStatus = ResourceStatus.Success, ExternalException = null } as ResourceResultBase);
    }
}
public class NeteaseMusicResourceResult : ResourceResultBase
{
    public override Exception? ExternalException { get; init; }
    public override required ResourceStatus ResourceStatus { get; init; }
}
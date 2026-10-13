using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Resources;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseMusicResource : MusicResourceBase, IAudioGainMetadata, IAudioTrialMetadata
{
    public string? Md5 { get; set; }
    public long Size { get; set; }
    public string? BitRate { get; set; }
    public string? EncodeType { get; set; }
    public long? Time { get; set; }
    public string? MusicType { get; set; }
    public string? Level { get; set; }
    public double? GainDb { get; set; }
    public double? Peak { get; set; }
    public bool IsTrial { get; set; }

    public bool SupportsAudioGain => Level?.ToLowerInvariant() is
        "standard" or "higher" or "exhigh" or "lossless" or "hires" or "jymaster";

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

using HyPlayer.PlayCore.Abstraction.Models.Resources;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseMusicQualityTag : ResourceQualityTag
{
    public string Quality { get; set; }
    public override string StableKey => string.IsNullOrWhiteSpace(Quality) ? "default" : Quality;

    public NeteaseMusicQualityTag(string qualityString)
    {
        Quality = qualityString;
    }
}

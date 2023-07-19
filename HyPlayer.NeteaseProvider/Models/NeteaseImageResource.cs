using HyPlayer.PlayCore.Abstraction.Models.Resources;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseImageResource : ImageResourceBase
{
    public override async Task<object?> GetResource(ResourceQualityTag? qualityTag = null, Type? awaitingType = null)
    {
        if (awaitingType == typeof(string))
        {
            return Url;
        }

        return null;
    }
    
}
using System.Collections.ObjectModel;
using HyPlayer.PlayCore.Abstraction.Interfaces.ProvidableItem;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.Lyric;
using HyPlayer.PlayCore.Abstraction.Models.Resources;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseRawLyricInfo : RawLyricInfo
{
    public required string LyricText { get; set; }
  
    public override Task<object?> GetResource(ResourceQualityTag? qualityTag = null)
    {
        return Task.FromResult<object?>(LyricText);
    }

    public bool IsWord { get; set; } = false;

    public override LyricType LyricType => LyricTypeActual;
    
    public required LyricType LyricTypeActual { get; set; }

    public LyricAuthorInfo Author { get; set; }
    
    public class LyricAuthorInfo : ProvidableItemBase
    {
        public override string ProviderId => "ncm";
        public override string TypeId => "us";
        
    }
}
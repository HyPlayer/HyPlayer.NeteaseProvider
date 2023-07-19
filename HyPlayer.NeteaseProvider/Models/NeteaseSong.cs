using HyPlayer.PlayCore.Abstraction.Interfaces.ProvidableItem;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.Songs;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseSong : SingleSongBase, IHasTranslation
{
    public override string ProviderId => "ncm";
    public override string TypeId => "sg";

    public string[]? Alias { get; set; }
    public bool HasCopyright { get; set; }
    public string? MvId { get; set; }
    public string? CdName { get; set; }
    public int TrackNumber { get; set; }
    

    public required List<PersonBase>? Artists { get; init; }

    public override Task<List<PersonBase>?> GetCreators()
    {
        return Task.FromResult(Artists);
    }

    public string? Translation { get; set; }
}
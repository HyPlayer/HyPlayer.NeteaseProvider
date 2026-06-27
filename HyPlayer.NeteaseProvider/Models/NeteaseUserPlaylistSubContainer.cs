using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseUserPlaylistSubContainer : LinerContainerBase
{
    public const string CreatedKind = "created-playlists";
    public const string SubscribedKind = "subscribed-playlists";

    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.Playlist;

    public string Kind { get; init; } = string.Empty;
    public List<NeteasePlaylist> Playlists { get; init; } = new();

    public override Task<List<ProvidableItemBase>> GetAllItemsAsync(CancellationToken ctk = default)
    {
        return Task.FromResult(Playlists.Select(t => (ProvidableItemBase)t).ToList());
    }
}

using HyPlayer.PlayCore.Abstraction.Models.Containers;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseAlbum : AlbumBase
{
    public override string ProviderId => "ncm";
    public override string TypeId => "al";
    public string? PictureUrl { get; set; }
}
using HyPlayer.NeteaseProvider.Bases;

namespace HyPlayer.NeteaseProvider.Requests;

public class PlaylistDetailRequest : RequestBase
{
    public required string Id { get; set; }
}
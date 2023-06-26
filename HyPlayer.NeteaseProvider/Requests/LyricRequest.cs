using HyPlayer.NeteaseProvider.Bases;

namespace HyPlayer.NeteaseProvider.Requests;

public class LyricRequest : RequestBase
{
    public required string Id { get; set; }
}
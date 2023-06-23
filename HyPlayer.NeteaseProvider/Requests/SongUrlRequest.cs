using HyPlayer.NeteaseProvider.Bases;

namespace HyPlayer.NeteaseProvider.Requests;

public class SongUrlRequest : RequestBase
{
    public string? Id { get; set; }
    public string[]? IdList { get; set; }
    public required string Level { get; set; }
}
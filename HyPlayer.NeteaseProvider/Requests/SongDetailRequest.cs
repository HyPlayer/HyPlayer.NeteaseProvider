using HyPlayer.NeteaseProvider.Bases;

namespace HyPlayer.NeteaseProvider.Requests;

public class SongDetailRequest : RequestBase
{
    public List<string>? IdList { get; set; }
    public string? Id { get; set; }
}
namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseLyricSearchItem : NeteaseSong
{
    public IReadOnlyList<string> MatchedLyrics { get; init; } = [];
}
using System.Text.Json.Serialization;
using HyPlayer.NeteaseProvider.Bases;

namespace HyPlayer.NeteaseProvider.Responses;

public class LyricResponse : CodedResponseBase
{

    [JsonPropertyName("transUser")] public LyricUserInfo? TranslationUser { get; set; }
    [JsonPropertyName("lyricUser")] public LyricUserInfo? LyricUser { get; set; }
    [JsonPropertyName("lrc")] public LyricInfo? Lyric { get; set; }
    [JsonPropertyName("klyric")] public LyricInfo? OldKaraokLyric { get; set; }
    [JsonPropertyName("tlyric")] public LyricInfo? TranslationLyric { get; set; }
    [JsonPropertyName("romalrc")] public LyricInfo? RomajiLyric { get; set; }

    [JsonPropertyName("yrc")] public LyricInfo? YunLyric { get; set; }
    [JsonPropertyName("ytlrc")] public LyricInfo? YunTranslationLyric { get; set; }
    [JsonPropertyName("yromalrc")] public LyricInfo? YunRomajiLyric { get; set; }
    
    

    public class LyricInfo
    {
        [JsonPropertyName("version")] public int Version { get; set; }
        [JsonPropertyName("lyric")] public string? Lyric { get; set; }
    }

    public class LyricUserInfo
    {
        [JsonPropertyName("id")] public string? LyricId { get; set; }
        [JsonPropertyName("userid")] public string? UserId { get; set; }
        [JsonPropertyName("nickname")] public string? Nickname { get; set; }
        [JsonPropertyName("uptime")] public long UpdateTime { get; set; }
    }
}
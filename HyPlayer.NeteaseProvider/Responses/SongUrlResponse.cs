using System.Text.Json.Serialization;
using HyPlayer.NeteaseProvider.Bases;

namespace HyPlayer.NeteaseProvider.Responses;

public class SongUrlResponse : CodedResponseBase
{
    [JsonPropertyName("data")] public SongUrlItem[]? SongUrls { get; set; }

    public class SongUrlItem
    {
        [JsonPropertyName("code")] public int Code { get; set; }
        [JsonPropertyName("id")] public required string Id { get; set; }
        [JsonPropertyName("url")] public string? Url { get; set; }
        [JsonPropertyName("br")] public string? BitRate { get; set; }
        [JsonPropertyName("size")] public long Size { get; set; }
        [JsonPropertyName("md5")] public string? Md5 { get; set; }
        [JsonPropertyName("type")] public string? Type { get; set; }
        [JsonPropertyName("level")] public string? Level { get; set; }
        [JsonPropertyName("encodeType")] public string? EncodeType { get; set; }
        [JsonPropertyName("time")] public long Time { get; set; }
    }
}
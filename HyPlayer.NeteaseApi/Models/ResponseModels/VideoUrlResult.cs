using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.Models.ResponseModels;

public class VideoUrlResult
{
    [JsonPropertyName("tag")] public string? Tag { get; set; }
    [JsonPropertyName("url")] public string? Url { get; set; }
    [JsonPropertyName("size")] public long Size { get; set; }
    [JsonPropertyName("duration")] public float Duration { get; set; } // fuck, who uses float to store time length? netease does!
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
    [JsonPropertyName("container")] public string? Container { get; set; }

    public class VideoUrlResultTagSign
    {
        [JsonPropertyName("br")] public int Br { get; set; }
        [JsonPropertyName("type")] public string? Type { get; set; }
        [JsonPropertyName("tagSign")] public string? TagSign { get; set; }
        [JsonPropertyName("mvtype")] public string? MvType { get; set; }
        [JsonPropertyName("resolution")] public int Resolution { get; set; }
    }
}
using System.Text.Json.Serialization;
using HyPlayer.NeteaseProvider.Bases;

namespace HyPlayer.NeteaseProvider.ActualRequests;

public class LyricActualRequest : EApiActualRequestBase
{
    [JsonPropertyName("id")] public required string Id { get; set; }
    [JsonPropertyName("cp")] public bool Cp => false;
    [JsonPropertyName("tv")] public int Tv => 0;
    [JsonPropertyName("lv")] public int Lv => 0;
    [JsonPropertyName("rv")] public int Rv => 0;
    [JsonPropertyName("kv")] public int Kv => 0;
    [JsonPropertyName("yv")] public int Yv => 0;
    [JsonPropertyName("ytv")] public int Ytv => 0;
    [JsonPropertyName("yrv")] public int Yrv => 0;
}
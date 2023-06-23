using System.Text.Json.Serialization;
using HyPlayer.NeteaseProvider.Bases;

namespace HyPlayer.NeteaseProvider.ActualRequests;

public class SongUrlActualRequest : EApiActualRequestBase
{
    [JsonPropertyName("ids")] public required string Ids { get; set; }
    [JsonPropertyName("level")] public required string Level { get; set; }
    [JsonPropertyName("encodeType")] public string EncodeType => "flac";
    [JsonPropertyName("immerseType")] public string ImmerseType => "c51";
}
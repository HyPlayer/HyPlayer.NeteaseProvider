using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseProvider.Bases;

public class EApiActualRequestBase : ActualRequestBase
{
    [JsonPropertyName("header")]
    public string? Header { get; set; }
}
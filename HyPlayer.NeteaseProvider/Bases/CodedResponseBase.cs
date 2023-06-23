using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseProvider.Bases;

public class CodedResponseBase
{
    [JsonPropertyName("code")] public int Code { get; set; }
}
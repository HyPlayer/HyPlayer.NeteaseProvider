using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseProvider.Bases;

public class WeApiActualRequestBase : ActualRequestBase
{
    [JsonPropertyName("csrf_token")]
    public string? CsrfToken { get; set; }
}
using System.Text.Json.Serialization;
using HyPlayer.NeteaseProvider.Bases;

namespace HyPlayer.NeteaseProvider.ActualRequests;

public class LoginEmailActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("username")] public required string Username { get; set; }
    [JsonPropertyName("password")] public required string Md5Password { get; set; }
    [JsonPropertyName("rememberLogin")] public bool RememberLogin => true;
}
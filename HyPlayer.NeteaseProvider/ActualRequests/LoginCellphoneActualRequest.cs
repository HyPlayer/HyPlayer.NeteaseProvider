using System.Text.Json.Serialization;
using HyPlayer.NeteaseProvider.Bases;

namespace HyPlayer.NeteaseProvider.ActualRequests;

public class LoginCellphoneActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("phone")] public string Phone { get; set; }
    [JsonPropertyName("countrycode")] public string CountryCode { get; set; } = "86";
    [JsonPropertyName("password")] public string Md5Password { get; set; }
    [JsonPropertyName("rememberLogin")] public bool RememberLogin => true;
}
using System.Text.Json.Serialization;
using HyPlayer.NeteaseProvider.Bases;

namespace HyPlayer.NeteaseProvider.Responses;

public class LoginResponse : CodedResponseBase
{
    [JsonPropertyName("loginType")] public int LoginType { get; set; }
    [JsonPropertyName("message")] public string Message { get; set; }
    [JsonPropertyName("profile")] public ProfileData Profile { get; set; }

    public class ProfileData
    {
        [JsonPropertyName("vipType")] public int VipType { get; set; }
        [JsonPropertyName("nickname")] public string Nickname { get; set; }
        [JsonPropertyName("birthday")] public long Birthday { get; set; }
        [JsonPropertyName("gender")] public int Gender { get; set; }
        [JsonPropertyName("avatarUrl")] public string AvatarUrl { get; set; }
        [JsonPropertyName("backgroundUrl")] public string BackgroundUrl { get; set; }
        [JsonPropertyName("signature")] public string Signature { get; set; }
        [JsonPropertyName("followed")] public bool? Followed { get; set; }
    }
}
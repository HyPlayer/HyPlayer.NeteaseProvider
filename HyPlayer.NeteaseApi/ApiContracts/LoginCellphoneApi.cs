using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Extensions;
using Kengwang.Toolkit;

namespace HyPlayer.NeteaseApi.ApiContracts;

public class LoginCellphoneApi : WeApiContractBase<LoginCellphoneRequest, LoginResponse, ErrorResultBase,
    LoginCellphoneActualRequest>
{
    public override string Url => "https://music.163.com/weapi/login/cellphone";
    public override HttpMethod Method => HttpMethod.Post;

    public override Dictionary<string, string> Cookies => new() { { "os", "pc" }, { "appver", "2.9.8" } };

    public override string UserAgent => "pc";

    public override Task MapRequest(LoginCellphoneRequest? request)
    {
        if (request is null) return Task.CompletedTask;
        var md5Password = string.IsNullOrEmpty(request.Md5Password)
            ? request.Password!.ToByteArrayUtf8().ComputeMd5().ToHexStringLower()
            : request.Md5Password!;
        ActualRequest = new LoginCellphoneActualRequest
                        {
                            Phone = request.Cellphone,
                            CountryCode = request.CountryCode,
                            Md5Password = md5Password
                        };
        return Task.CompletedTask;
    }
    
    public override async Task<Results<LoginResponse, ErrorResultBase>> ProcessResponseAsync(HttpResponseMessage response, ApiHandlerOption option)
    {
        return (await base.ProcessResponseAsync(response, option))
            .Match(
                (success) => success.Code != 200 ? new ErrorResultBase(success.Code, success.Message) : success,
                Results<LoginResponse, ErrorResultBase>.CreateError
            );
    }
}

public class LoginCellphoneActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("phone")] public required string Phone { get; set; }
    [JsonPropertyName("countrycode")] public string CountryCode { get; set; } = "86";
    [JsonPropertyName("password")] public required string Md5Password { get; set; }
    [JsonPropertyName("rememberLogin")] public bool RememberLogin => true;
}

public class LoginCellphoneRequest : RequestBase
{
    public required string Cellphone { get; set; }
    public string CountryCode { get; set; } = "86";
    public string? Password { get; set; }
    public string? Md5Password { get; set; }
}

public class LoginResponse : CodedResponseBase
{
    [JsonPropertyName("loginType")] public int LoginType { get; set; }
    [JsonPropertyName("message")] public string? Message { get; set; }
    [JsonPropertyName("profile")] public ProfileData? Profile { get; set; }

    public class ProfileData
    {
        [JsonPropertyName("userId")] public string? UserId { get; set; }
        [JsonPropertyName("vipType")] public int VipType { get; set; }
        [JsonPropertyName("nickname")] public string? Nickname { get; set; }
        [JsonPropertyName("birthday")] public long Birthday { get; set; }
        [JsonPropertyName("gender")] public int Gender { get; set; }
        [JsonPropertyName("avatarUrl")] public string? AvatarUrl { get; set; }
        [JsonPropertyName("backgroundUrl")] public string? BackgroundUrl { get; set; }
        [JsonPropertyName("signature")] public string? Signature { get; set; }
        [JsonPropertyName("followed")] public bool? Followed { get; set; }
    }
}
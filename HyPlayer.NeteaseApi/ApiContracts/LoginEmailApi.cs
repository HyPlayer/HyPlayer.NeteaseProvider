using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Extensions;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    /// <summary>
    /// 邮箱登录
    /// </summary>
    public static LoginEmailApi LoginEmailApi => new();
}

public class LoginEmailApi : EApiContractBase<LoginEmailRequest, LoginResponse, ErrorResultBase,
    LoginEmailActualRequest>
{
    public override string IdentifyRoute => "/login";
    public override string Url => "https://interface.163.com/eapi/w/login";
    public override string ApiPath => "/api/w/login";
    public override HttpMethod Method => HttpMethod.Post;
    public override string UserAgent => "pc";

    public override Task MapRequest(LoginEmailRequest? request)
    {
        if (request is null) return Task.CompletedTask;
        var md5Password = string.IsNullOrEmpty(request.Md5Password)
            ? request.Password!.ToByteArrayUtf8().ComputeMd5().ToHexStringLower()
            : request.Md5Password!;
        ActualRequest = new LoginEmailActualRequest
        {
            Username = request?.Email!,
            Md5Password = md5Password,
        };
        return Task.CompletedTask;
    }



    public override async Task<Results<LoginResponse, ErrorResultBase>> ProcessResponseAsync(
        HttpResponseMessage response, ApiHandlerOption option, CancellationToken cancellationToken = default)
    {
        return (await base.ProcessResponseAsync(response, option, cancellationToken).ConfigureAwait(false))
            .Match(
                (success) => success.Code != 200 ? new ErrorResultBase(success.Code, success.Message) : success,
                Results<LoginResponse, ErrorResultBase>.CreateError
            );
    }
}

public class LoginEmailActualRequest : EApiActualRequestBase
{
    [JsonPropertyName("https")] public bool Https => true;
    [JsonPropertyName("type")] public int Type => 0;
    [JsonPropertyName("username")] public required string Username { get; set; }
    [JsonPropertyName("password")] public required string Md5Password { get; set; }
    [JsonPropertyName("rememberLogin")] public bool RememberLogin => true;
}

public class LoginEmailRequest : RequestBase
{
    /// <summary>
    /// 邮箱
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// 密码 (MD5)
    /// </summary>
    public string? Md5Password { get; set; }
}
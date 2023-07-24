﻿using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Extensions;
using Kengwang.Toolkit;

namespace HyPlayer.NeteaseApi.ApiContracts;

public class LoginEmailApi : WeApiContractBase<LoginEmailRequest, LoginResponse, ErrorResultBase,
    LoginEmailActualRequest>
{
    public override string Url => "https://music.163.com/weapi/login";
    public override HttpMethod Method => HttpMethod.Post;
    public override string UserAgent => "pc";
    public override Dictionary<string, string> Cookies => new() { { "os", "pc" }, { "appver", "2.9.8" } };

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

    public override async Task<Results<LoginResponse, ErrorResultBase>> ProcessResponseAsync(HttpResponseMessage response, ApiHandlerOption option)
    {
        return (await base.ProcessResponseAsync(response, option))
            .Match(
            (success) => success.Code != 200 ? new ErrorResultBase(success.Code, success.Message) : success,
            Results<LoginResponse, ErrorResultBase>.CreateError
        );
    }
}

public class LoginEmailActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("username")] public required string Username { get; set; }
    [JsonPropertyName("password")] public required string Md5Password { get; set; }
    [JsonPropertyName("rememberLogin")] public bool RememberLogin => true;
}

public class LoginEmailRequest : RequestBase
{
    public required string Email { get; set; }
    public string? Password { get; set; }
    public string? Md5Password { get; set; }
}
﻿using HyPlayer.NeteaseProvider.ActualRequests;
using HyPlayer.NeteaseProvider.Bases;
using HyPlayer.NeteaseProvider.Bases.ApiContractBases;
using HyPlayer.NeteaseProvider.Extensions;
using HyPlayer.NeteaseProvider.Requests;
using HyPlayer.NeteaseProvider.Responses;

namespace HyPlayer.NeteaseProvider.ApiContracts;

public class LoginEmailApi : WeApiContractBase<LoginEmailRequest, LoginResponse, ErrorResultBase,
    LoginEmailActualRequest>
{
    public override string Url => "https://music.163.com/weapi/login";
    public override HttpMethod Method => HttpMethod.Post;
    public override string UserAgent => "pc";
    public override Dictionary<string, string> Cookies => new() { { "os", "pc" }, { "appver", "2.9.8" } };

    public override Task MapRequest(LoginEmailRequest request)
    {
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

    public override async Task<Results<LoginResponse, ErrorResultBase>> ProcessResponseAsync(HttpResponseMessage response, ProviderOption option)
    {
        return (await base.ProcessResponseAsync(response, option))
            .Match(
            (success) => success.Code != 200 ? new ErrorResultBase(success.Code, success.Message) : success,
            Results<LoginResponse, ErrorResultBase>.CreateError
        );
    }
}
using HyPlayer.NeteaseProvider.ActualRequests;
using HyPlayer.NeteaseProvider.Bases;
using HyPlayer.NeteaseProvider.Bases.ApiContractBases;
using HyPlayer.NeteaseProvider.Extensions;
using HyPlayer.NeteaseProvider.Requests;
using HyPlayer.NeteaseProvider.Responses;

namespace HyPlayer.NeteaseProvider.ApiContracts;

public class LoginEmailApi : WeApiContractBase<LoginEmailRequest, LoginEmailResponse, ErrorResultBase,
    LoginEmailActualRequest>
{
    public override string Url => "https://music.163.com/weapi/login";
    public override HttpMethod Method => HttpMethod.Post;
    public override string UserAgent => "pc";
    public override Dictionary<string, string> Cookies => new() { { "os", "pc" }, { "appver", "2.9.8" } };

    public override Task MapRequest(LoginEmailRequest? request)
    {
        ActualRequest = new LoginEmailActualRequest
                        {
                            Username = request?.Email!,
                            Md5Password = request?.Password.ToByteArrayUtf8().ComputeMd5().ToHexStringLower()!
                        };
        return Task.CompletedTask;
    }

    public override async Task<Results<LoginEmailResponse, ErrorResultBase>> ProcessResponseAsync(HttpResponseMessage response, ProviderOption option)
    {
        return (await base.ProcessResponseAsync(response, option))
            .Match(
            (success) => success.Code != 200 ? new ErrorResultBase(success.Code, success.Message) : success,
            Results<LoginEmailResponse, ErrorResultBase>.CreateError
        );
    }
}
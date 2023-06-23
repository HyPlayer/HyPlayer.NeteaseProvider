using HyPlayer.NeteaseProvider.ActualRequests;
using HyPlayer.NeteaseProvider.Bases;
using HyPlayer.NeteaseProvider.Bases.ApiContractBases;
using HyPlayer.NeteaseProvider.Extensions;
using HyPlayer.NeteaseProvider.Requests;
using HyPlayer.NeteaseProvider.Responses;

namespace HyPlayer.NeteaseProvider.ApiContracts;

public class LoginCellphoneApi : WeApiContractBase<LoginCellphoneRequest, LoginResponse, ErrorResultBase,
    LoginCellphoneActualRequest>
{
    public override string Url => "https://music.163.com/weapi/login/cellphone";
    public override HttpMethod Method => HttpMethod.Post;

    public override Dictionary<string, string> Cookies => new() { { "os", "pc" }, { "appver", "2.9.8" } };

    public override string UserAgent => "pc";

    public override Task MapRequest(LoginCellphoneRequest request)
    {
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
    
    public override async Task<Results<LoginResponse, ErrorResultBase>> ProcessResponseAsync(HttpResponseMessage response, ProviderOption option)
    {
        return (await base.ProcessResponseAsync(response, option))
            .Match(
                (success) => success.Code != 200 ? new ErrorResultBase(success.Code, success.Message) : success,
                Results<LoginResponse, ErrorResultBase>.CreateError
            );
    }
}
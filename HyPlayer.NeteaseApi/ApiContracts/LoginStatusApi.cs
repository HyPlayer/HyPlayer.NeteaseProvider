using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static LoginStatusApi LoginStatusApi => new();
}

public class LoginStatusApi : EApiContractBase<LoginStatusRequest, LoginStatusResponse, ErrorResultBase, LoginStatusActualRequest>
{
    public override string IdentifyRoute => "/login/status";
    public override string Url => "https://interface.music.163.com/eapi/w/nuser/account/get";
    public override string ApiPath => "/api/w/nuser/account/get";
    public override HttpMethod Method => HttpMethod.Post;
    public override Task MapRequest(LoginStatusRequest? request)
    {
        ActualRequest = new LoginStatusActualRequest();
        return Task.CompletedTask;
    }
}

public class LoginStatusRequest : RequestBase
{

}

public class LoginStatusResponse : CodedResponseBase
{
    [JsonPropertyName("profile")] public UserInfoDto? Profile { get; set; }
    [JsonPropertyName("account")] public AccountInfoDto? Account { get; set; }

    public class AccountInfoDto
    {
        [JsonPropertyName("id")] public string? Id { get; set; }
        [JsonPropertyName("userName")] public string? UserName { get; set; }
        [JsonPropertyName("type")] public int Type { get; set; }
        [JsonPropertyName("status")] public int Status { get; set; }
        [JsonPropertyName("whitelistAuthority")] public int WhitelistAuthority { get; set; }
        [JsonPropertyName("createTime")] public long CreateTime { get; set; }
        [JsonPropertyName("vipType")] public int VipType { get; set; }
        [JsonPropertyName("ban")] public int Ban { get; set; }
        [JsonPropertyName("anonimousUser")] public bool AnonymousUser { get; set; }
    }
}

public class LoginStatusActualRequest : EApiActualRequestBase
{

}
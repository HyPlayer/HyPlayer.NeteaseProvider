using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static LoginQrCodeUnikeyApi LoginQrCodeUnikeyApi => new();
}

public class LoginQrCodeUnikeyApi : EApiContractBase<LoginQrCodeUnikeyRequest, LoginQrCodeUnikeyResponse, ErrorResultBase, LoginQrCodeUnikeyActualRequest>
{
    public override string IdentifyRoute => "/login/qr/unikey";
    public override string Url => "https://interface.music.163.com/eapi/login/qrcode/unikey";
    public override HttpMethod Method => HttpMethod.Post;

    public override async Task MapRequest(LoginQrCodeUnikeyRequest? request)
    {
        ActualRequest = new LoginQrCodeUnikeyActualRequest();
        await Task.CompletedTask;
    }

    public override string ApiPath => "/api/login/qrcode/unikey";
}

public class LoginQrCodeUnikeyRequest : RequestBase
{

}

public class LoginQrCodeUnikeyResponse : CodedResponseBase
{
    [JsonPropertyName("unikey")]
    public string Unikey { get; set; }
}

public class LoginQrCodeUnikeyActualRequest : EApiActualRequestBase
{
    [JsonPropertyName("type")]
    public string Type => "3";
}
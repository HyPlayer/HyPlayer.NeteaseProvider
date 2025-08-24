using HyPlayer.NeteaseApi.ApiContracts.Login;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static LoginQrCodeUnikeyApi LoginQrCodeUnikeyApi => new();
    }
}


namespace HyPlayer.NeteaseApi.ApiContracts.Login
{
    public class LoginQrCodeUnikeyApi : EApiContractBase<LoginQrCodeUnikeyRequest, LoginQrCodeUnikeyResponse,
        ErrorResultBase, LoginQrCodeUnikeyActualRequest>
    {
        public override string IdentifyRoute => "/login/qr/unikey";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/login/qrcode/unikey";
        public override HttpMethod Method => HttpMethod.Post;

        public override async Task MapRequest(ApiHandlerOption option)
        {
            ActualRequest = new LoginQrCodeUnikeyActualRequest();
            await Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/login/qrcode/unikey";
    }

    public class LoginQrCodeUnikeyRequest : RequestBase
    {

    }

    public class LoginQrCodeUnikeyResponse : CodedResponseBase
    {
        [JsonPropertyName("unikey")] public string? Unikey { get; set; }
    }

    public class LoginQrCodeUnikeyActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("type")] public string Type => "3";
    }
}
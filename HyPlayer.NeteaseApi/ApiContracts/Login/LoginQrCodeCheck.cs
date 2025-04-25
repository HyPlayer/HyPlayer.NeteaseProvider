using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Login;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static LoginQrCodeCheckApi LoginQrCodeCheckApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Login
{
    public class LoginQrCodeCheckApi : EApiContractBase<LoginQrCodeCheckRequest, LoginQrCodeCheckResponse,
        ErrorResultBase, LoginQrCodeCheckActualRequest>
    {
        public override string IdentifyRoute => "/login/qr/check";

        public override string Url { get; protected set; } =
            "https://interface.music.163.com/eapi/login/qrcode/client/login";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest()
        {
            if (Request is null) return Task.CompletedTask;
            ActualRequest = new LoginQrCodeCheckActualRequest
            {
                Key = Request.Unikey
            };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/login/qrcode/client/login";
    }

    public class LoginQrCodeCheckRequest : RequestBase
    {
        public required string Unikey { get; set; }
    }

    public class LoginQrCodeCheckResponse : CodedResponseBase
    {

    }

    public class LoginQrCodeCheckActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("key")] public required string Key { get; set; }

        [JsonPropertyName("type")] public int Type => 3;
    }
}
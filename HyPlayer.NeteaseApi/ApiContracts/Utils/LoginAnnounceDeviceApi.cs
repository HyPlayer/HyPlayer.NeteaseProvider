using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Utils;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using HyPlayer.NeteaseApi.Extensions;

namespace HyPlayer.NeteaseApi.ApiContracts
{

public static partial class NeteaseApis
{
    public static LoginAnnounceDeviceApi LoginAnnounceDeviceApi => new();
}
}


namespace HyPlayer.NeteaseApi.ApiContracts.Utils
{

    public class LoginAnnounceDeviceApi : EApiContractBase<LoginAnnounceDeviceRequest, LoginAnnounceDeviceResponse, ErrorResultBase, LoginAnnounceDeviceActualRequest>
    {
        public override string IdentifyRoute => "/login/anon/device";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/login/anon/device";
        public override HttpMethod Method => HttpMethod.Post;
        public override Task MapRequest(ApiHandlerOption option)
        {
            Request ??= new LoginAnnounceDeviceRequest();
            option.Cookies["deviceId"] = NeteaseUtils.GetDeviceId(Request.Imei, Request.Mac, Request.AndroidId, Request.LocalId);
            option.Cookies["mobilename"] = Request.DeviceName;
            option.Cookies["os"] = Request.OS;
            option.Cookies["channel"] = Request.Channel;
            option.Cookies["deviceType"] = Request.DeviceType;
            return Task.CompletedTask;
        }
        
        public override string ApiPath { get; protected set; } = "/api/login/anon/device";
    }

    public class LoginAnnounceDeviceRequest : RequestBase
    {

        public string? Imei { get; set; }
        public string Mac { get; set; } = "02:00:00:00:00:00";
        public string? AndroidId { get; set; }
        public string? LocalId { get; set; }
        public string DeviceName { get; set; } = "car";
        public string OS { get; set; } = "andrcar";
        public string Channel { get; set; } = "release";
        public string DeviceType { get; set; } = "andrcar";
        
    }

    public class LoginAnnounceDeviceResponse : CodedResponseBase
    {
        public class LoginAnnounceDeviceResponseData
        {
            [JsonPropertyName("userId")] public string? Id { get; set; }
        }
        [JsonPropertyName("data")] public LoginAnnounceDeviceResponseData? Data { get; set; }
    }

    public class LoginAnnounceDeviceActualRequest : EApiActualRequestBase
    {
        
    }
}
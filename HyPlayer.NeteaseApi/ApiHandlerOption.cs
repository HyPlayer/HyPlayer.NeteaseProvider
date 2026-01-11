using HyPlayer.NeteaseApi.Extensions.JsonSerializer;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts;

namespace HyPlayer.NeteaseApi;

public class ApiHandlerOption
{
    public Dictionary<string, string> Cookies { get; } = new();

    // ReSharper disable once InconsistentNaming
    public string? XRealIP { get; set; } = null;
    public bool UseProxy { get; set; } = false;
    public IWebProxy? Proxy { get; set; } = null;
    public string? UserAgent { get; set; } = null;
    public bool DegradeHttp { get; set; } = false;
    /// <summary>
    /// 启用需要使用CheckToken的接口, 当 AdditionalParameters 被设定时不进行检查
    /// </summary>
    public bool BypassCheckTokenApi { get; set; } = false;

    public AdditionalParameters AdditionalParameters { get; set; } = new();

    public bool FakeCheckToken { get; set; }

    public JsonSerializerOptions JsonSerializerOptions =
        new(JsonSerializerOptions.Web)
        {
            NumberHandling = JsonNumberHandling.WriteAsString |
                             JsonNumberHandling.AllowReadingFromString,
            AllowTrailingCommas = true,
            Converters = { new NumberToStringConverter(), new JsonBooleanConverter(), new JsonObjectStringConverter() },
            TypeInfoResolver = NeteaseApiContractJsonContext.Default

        };
    public static readonly JsonSerializerOptions JsonSerializerOptionsOnlyTypeInfo =
        new(JsonSerializerOptions.Default)
        {
            TypeInfoResolver = NeteaseApiContractJsonContext.Default
        };
    public static readonly JsonSerializerOptions JsonSerializerOptionsWebOnlyTypeInfo =
        new(JsonSerializerOptions.Web)
        {
            TypeInfoResolver = NeteaseApiContractJsonContext.Default
        };
}

public class AdditionalParameters
{
    public Dictionary<string, string?> Cookies { get; set; } = [];
    public Dictionary<string, string?> Headers { get; set; } = [];
    public Dictionary<string, string?> EApiHeaders { get; set; } = [];
    public Dictionary<string, string?> DataTokens { get; set; } = [];
    public OpenAPIConfigData? OpenAPIConfig { get; set; } = null;
    public bool HasValue()
    {
        if (Cookies.Count > 0 ||
            Headers.Count > 0 ||
            EApiHeaders.Count > 0 ||
            DataTokens.Count > 0) return true;
        return false;
    }


    public class OpenAPIConfigData
    {
        public string? AppId { get; set; }
        public string? AppSecret { get; set; }
        public string? RsaPrivateKey { get; set; }
        public DeviceInfoData? DeviceInfo { get; set; }

        public class DeviceInfoData
        {
            public string? Channel { get; set; }
            public string? DeviceId { get; set; }
            public string? DeviceType { get; set; }
            public string? AppVer { get; set; }
            public string? OS { get; set; }
            public string? OSVer { get; set; }
            public string? Brand { get; set; }
            public string? Model { get; set; }
            public string? ClientIp { get; set; }
        }
    }
}
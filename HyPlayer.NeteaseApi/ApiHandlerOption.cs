using HyPlayer.NeteaseApi.Extensions.JsonSerializer;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

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
            Converters = { new NumberToStringConverter(), new JsonBooleanConverter(), new JsonObjectStringConverter() }
        };
}

public class AdditionalParameters
{
    public Dictionary<string, string?> Cookies { get; set; } = [];
    public Dictionary<string, string?> Headers { get; set; } = [];
    public Dictionary<string, string?> EApiHeaders { get; set; } = [];
    public Dictionary<string, string?> DataTokens { get; set; } = [];
    public bool HasValue()
    {
        if (Cookies.Count > 0 ||
            Headers.Count > 0 ||
            EApiHeaders.Count > 0 ||
            DataTokens.Count > 0) return true;
        return false;
    }
}
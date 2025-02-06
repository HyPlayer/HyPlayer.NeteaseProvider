using HyPlayer.NeteaseApi.Extensions.JsonSerializer;
using HyPlayer.NeteaseApi.Serialization;
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
    public AdditionalParameters AdditionalParameters { get; set; } = new ();

    public JsonSerializerOptions JsonSerializerOptions =
        new(JsonSerializerOptions.Default)
        {
            NumberHandling = JsonNumberHandling.WriteAsString |
                             JsonNumberHandling.AllowReadingFromString,
            AllowTrailingCommas = true,
            Converters = { new NumberToStringConverter(), new JsonBooleanConverter() }
        };
    public JsonSerializerOptions JsonDeserializerOptions =
        new(JsonSerializerOptions.Default)
        {
            NumberHandling = JsonNumberHandling.WriteAsString |
                             JsonNumberHandling.AllowReadingFromString,
            AllowTrailingCommas = true,
            Converters = { new NumberToStringConverter(), new JsonBooleanConverter() },
            TypeInfoResolver= JsonSerializeContext.Default
        };
}

public class AdditionalParameters {
    public Dictionary<string, string?> Cookies { get; set; } = [];
    public Dictionary<string, string?> Headers { get; set; } = [];
    public Dictionary<string, string?> EApiHeaders { get; set; } = [];
    public Dictionary<string, string?> DataTokens { get; set; } = [];
}
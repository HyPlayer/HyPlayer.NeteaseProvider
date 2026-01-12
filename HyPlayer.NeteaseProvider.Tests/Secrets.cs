using HyPlayer.NeteaseApi;
using HyPlayer.NeteaseApi.Extensions.JsonSerializer;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseProvider.Tests;

public static class Secrets
{
    static string secret = """    
PUT YOUR SECRET HERE
""";
    public static JsonSerializerOptions defaultOptions = new JsonSerializerOptions(JsonSerializerDefaults.General)
    {
        Converters = { new NumberToStringConverter() },
        TypeInfoResolver = JsonContext.Default
    };
    public static AdditionalParameters AdditionalParameters = JsonSerializer.Deserialize<AdditionalParameters>(secret, defaultOptions)!;
}

[JsonSerializable(typeof(AdditionalParameters))]
[JsonSourceGenerationOptions(WriteIndented = true)]
public partial class JsonContext : JsonSerializerContext
{

}
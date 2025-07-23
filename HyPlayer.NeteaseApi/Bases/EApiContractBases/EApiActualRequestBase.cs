using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.Bases.EApiContractBases;

public class EApiActualRequestBase : ActualRequestBase
{
    [JsonPropertyName("header")] public string? Header { get; set; }

    [JsonPropertyName("e_r")] public bool Error => true;
}

public class CacheKeyEApiActualRequest : EApiActualRequestBase
{
    [JsonPropertyName("cache_key")] public string? CacheKey { get; set; }
}

public interface IFakeCheckTokenApi
{

}
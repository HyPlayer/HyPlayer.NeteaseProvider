using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.Bases.OpenApiContractBases;

public abstract class OpenApiActualRequestBase : ActualRequestBase
{
    [JsonIgnore]
    public string? AccessToken
    { get; set; }
}

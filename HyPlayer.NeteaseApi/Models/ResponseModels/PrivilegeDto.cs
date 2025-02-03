using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.Models.ResponseModels;

public class PrivilegeDto
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("fee")] public int Fee { get; set; }
    [JsonPropertyName("payed")] public int Payed { get; set; }
    [JsonPropertyName("st")] public int St { get; set; }
    [JsonPropertyName("pl")] public int Pl { get; set; }
    [JsonPropertyName("dl")] public int Dl { get; set; }
    [JsonPropertyName("sp")] public int Sp { get; set; }
    [JsonPropertyName("cp")] public int Cp { get; set; }
    [JsonPropertyName("subp")] public int Subp { get; set; }
    [JsonPropertyName("cs")] public bool Cs { get; set; }
    [JsonPropertyName("maxbr")] public int MaxBr { get; set; }
    [JsonPropertyName("fl")] public int Fl { get; set; }
    [JsonPropertyName("toast")] public bool Toast { get; set; }
    [JsonPropertyName("flag")] public int Flag { get; set; }
    [JsonPropertyName("preSell")] public bool PreSell { get; set; }
    [JsonPropertyName("playMaxbr")] public int PlayMaxBr { get; set; }
    [JsonPropertyName("downloadMaxbr")] public int DownloadMaxBr { get; set; }
    [JsonPropertyName("maxBrLevel")] public string? MaxBrLevel { get; set; }
    [JsonPropertyName("playMaxBrLevel")] public string? PlayMaxBrLevel { get; set; }
    [JsonPropertyName("downloadMaxBrLevel")] public string? DownloadMaxBrLevel { get; set; }
    [JsonPropertyName("plLevel")] public string? PlayLevel { get; set; }
    [JsonPropertyName("dlLevel")] public string? DownloadLevel { get; set; }

}
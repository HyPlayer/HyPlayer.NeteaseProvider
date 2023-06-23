using System.Text.Json.Serialization;
using HyPlayer.NeteaseProvider.Bases;

namespace HyPlayer.NeteaseProvider.ActualRequests;

public class SongDetailActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("c")] public required string Ids { get; set; }
}
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.Models.ResponseModels;

public class MVDto
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("playCount")] public long PlayCount { get; set; }
    [JsonPropertyName("artistName")] public string? ArtistName { get; set; }
    [JsonPropertyName("artistId")] public string? ArtistId { get; set; }
    [JsonPropertyName("duration")] public long Duration { get; set; }
    [JsonPropertyName("briefDesc")] public string? BriefDescription { get; set; }
    [JsonPropertyName("desc")] public string? Description { get; set; }
    [JsonPropertyName("transNames")] public string[]? TransNames { get; set; }
    [JsonPropertyName("cover")] public string? Cover { get; set; }
    [JsonPropertyName("alias")] public string[]? Alias { get; set; }
    [JsonPropertyName("artists")] public ArtistDto[]? Artists { get; set; }

    public class MVArtistDto
    {
        [JsonPropertyName("id")] public string? Id { get; set; }
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("alias")] public string[]? Alias { get; set; }
        [JsonPropertyName("transNames")] public string[]? TransNames { get; set; }
    }
}
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.Models.ResponseModels;

public class ArtistSongDto
{
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("alias")] public string[]? Alias { get; set; }
    [JsonPropertyName("duration")] public long Duration { get; set; }
    [JsonPropertyName("transNames")] public string[]? Translations { get; set; }
    [JsonPropertyName("mvid")] public string? MvId { get; set; }
    [JsonPropertyName("no")] public int TrackNumber { get; set; }
    [JsonPropertyName("album")] public AlbumDto? Album { get; set; }
    [JsonPropertyName("artists")] public ArtistDto[]? Artists { get; set; }
    [JsonPropertyName("privilege")] public PrivilegeDto? Privilege { get; set; }
}
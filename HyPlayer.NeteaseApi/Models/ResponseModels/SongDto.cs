﻿using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.Models.ResponseModels;

public class SongDto
{
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("alias")] public string[]? Alias { get; set; }
    [JsonPropertyName("duration")] public long Duration { get; set; }
    [JsonPropertyName("transName")] public string? Translation { get; set; }
    [JsonPropertyName("mvid")] public string? MvId { get; set; }
    [JsonPropertyName("disc")] public string? CdName { get; set; }
    [JsonPropertyName("no")] public int TrackNumber { get; set; }
    [JsonPropertyName("album")] public AlbumDto? Album { get; set; }
    [JsonPropertyName("ar")] public ArtistDto[]? Artists { get; set; }

}
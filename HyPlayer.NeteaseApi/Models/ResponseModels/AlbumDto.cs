using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.Models.ResponseModels;

public class AlbumDto
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("picUrl")] public string? PictureUrl { get; set; }
    [JsonPropertyName("size")] public long Size { get; set; }
    [JsonPropertyName("publishTime")] public long PublishTime { get; set; }
    [JsonPropertyName("company")] public string? Company { get; set; }
    [JsonPropertyName("briefDesc")] public string? BriefDescription { get; set; }
    [JsonPropertyName("alias")] public string[]? Alias  { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("transName")] public string? Translation { get; set; }
    [JsonPropertyName("subType")] public string? SubType { get; set; }
    [JsonPropertyName("artists")] public ArtistDto[]? Artists { get; set; }
}
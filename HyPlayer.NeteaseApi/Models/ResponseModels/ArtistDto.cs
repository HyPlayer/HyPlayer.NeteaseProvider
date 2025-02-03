using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.Models.ResponseModels;

public class ArtistDto
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("alias")] public string[]? Alias { get; set; }
    [JsonPropertyName("followed")] public bool Followed { get; set; }
    [JsonPropertyName("picUrl")] public string? PicUrl { get; set; }
    [JsonPropertyName("img1v1Url")] public string? Img1v1Url { get; set; }
    [JsonPropertyName("briefDesc")] public string? BriefDesc { get; set; }
    [JsonPropertyName("trans")] public string? Translation { get; set; }
    [JsonPropertyName("musicSize")] public int MusicSize { get; set; }
    [JsonPropertyName("albumSize")] public int AlbumSize { get; set; }
    [JsonPropertyName("mvSize")] public int MvSize { get; set; }
    [JsonPropertyName("transNames")] public string[]? TransNames { get; set; }
}
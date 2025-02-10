using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.Models.ResponseModels;

public class VideoDto
{
    [JsonPropertyName("vid")] public string? Id { get; set; }
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("creator")] public VideoCreatorDto[]? Artists { get; set; }
    [JsonPropertyName("durationms")] public long Duration { get; set; }
    [JsonPropertyName("coverUrl")] public string? CoverUrl { get; set; }
    [JsonPropertyName("playTime")] public long PlayTime { get; set; }

    public class VideoCreatorDto
    {
        [JsonPropertyName("userId")] public string? UserId { get; set; }
        [JsonPropertyName("userName")] public string? UserName { get; set; }
    }
}
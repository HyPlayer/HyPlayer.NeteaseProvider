using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.Models.ResponseModels;

public class DjRadioProgramDto
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("picUrl")] public string? PictureUrl { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("coverUrl")] public string? CoverUrl { get; set; }
    [JsonPropertyName("duration")] public long Duration { get; set; }
    [JsonPropertyName("dj")] public UserInfoDto? Owner { get; set; }
    [JsonPropertyName("radio")] public DjRadioChannelDto? Radio { get; set; }
    [JsonPropertyName("mainSong")] public SongDto? MainSong { get; set; }
    [JsonPropertyName("buyed")] public bool Bought { get; set; }
    [JsonPropertyName("listenerCount")] public long ListenerCount { get; set; }
    [JsonPropertyName("subscribedCount")] public long SubscribedCount { get; set; }
    [JsonPropertyName("commentCount")] public long CommentCount { get; set; }
    [JsonPropertyName("shareCount")] public long ShareCount { get; set; }
    [JsonPropertyName("likedCount")] public long LikedCount { get; set; }
    [JsonPropertyName("createTime")] public long CreateTime { get; set; }
    [JsonPropertyName("serialNum")] public int SerialNum { get; set; }
}
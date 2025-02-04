using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.Models.ResponseModels;

public class CommentDto
{
    [JsonPropertyName("user")] public UserInfoDto? User { get; set; }
    [JsonPropertyName("content")] public string? Content { get; set; }
    [JsonPropertyName("richContent")] public string? RichContent { get; set; }
    [JsonPropertyName("commentId")] public long CommentId { get; set; }
    [JsonPropertyName("likedCount")] public int LikedCount { get; set; }
    [JsonPropertyName("liked")] public bool Liked { get; set; }
    [JsonPropertyName("owner")] public bool Owner { get; set; }
    [JsonPropertyName("time")] public long Time { get; set; }
    [JsonPropertyName("timeStr")] public string? TimeStr { get; set; }
    [JsonPropertyName("parentCommentId")] public string? ParentCommentId { get; set; }
    [JsonPropertyName("beReplied")] public BeRepliedCommentDto[]? BeReplied { get; set; }


    public class CommentIpLocation
    {
        [JsonPropertyName("ip")] public string? Ip { get; set; }
        [JsonPropertyName("location")] public string? Location { get; set; }
        [JsonPropertyName("userId")] public string? UserId { get; set; }
    }
}

public class BeRepliedCommentDto
{
    [JsonPropertyName("user")] public UserInfoDto? User { get; set; }
    [JsonPropertyName("beRepliedCommentId")] public string? BeRepliedCommentId { get; set; }
    [JsonPropertyName("content")] public string? Content { get; set; }
    [JsonPropertyName("richContent")] public string? RichContent { get; set; }
    [JsonPropertyName("ipLocation")] public CommentDto.CommentIpLocation? IpLocation { get; set; }
    
}
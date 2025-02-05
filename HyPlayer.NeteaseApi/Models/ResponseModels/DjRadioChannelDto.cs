using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.Models.ResponseModels;

public class DjRadioChannelDto
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("subed")] public bool Subscribed { get; set; }
    [JsonPropertyName("programCount")] public int ProgramCount { get; set; }
    [JsonPropertyName("createTime")] public long CreateTime { get; set; }
    [JsonPropertyName("desc")] public string? Description { get; set; }
    [JsonPropertyName("subCount")] public long SubscribedCount { get; set; }
    [JsonPropertyName("picUrl")] public string? CoverUrl { get; set; }
    [JsonPropertyName("categoryId")] public int CategoryId { get; set; }
    [JsonPropertyName("category")] public string? Category { get; set; }
    [JsonPropertyName("secondCategoryId")] public int SecondCategoryId { get; set; }
    [JsonPropertyName("secondCategory")] public string? SecondCategory { get; set; }
    [JsonPropertyName("likedCount")] public long LikedCount { get; set; }
    [JsonPropertyName("commentCount")] public long CommentCount { get; set; }
    [JsonPropertyName("shareCount")] public long ShareCount { get; set; }
    [JsonPropertyName("playCount")] public long PlayCount { get; set; }
    [JsonPropertyName("rcmdText")] public string? RecommendText { get; set; }
    [JsonPropertyName("price")] public float Price { get; set; }
    [JsonPropertyName("buyed")] public bool Bought { get; set; }
    [JsonPropertyName("lastProgramCreateTime")] public long LastProgramCreateTime { get; set; }
    [JsonPropertyName("lastProgramId")] public string? LastProgramId { get; set; }
    [JsonPropertyName("lastProgramName")] public string? LastProgramName { get; set; }
    [JsonPropertyName("hightQuality")] public bool IsHighQuality { get; set; }
    
}

public class DjVoiceChannelDto
{
    [JsonPropertyName("voiceListId")] public string? Id { get; set; }
    [JsonPropertyName("voiceListName")] public string? Name { get; set; }
    [JsonPropertyName("coverUrl")] public string? CoverUrl { get; set; }
    [JsonPropertyName("userId")] public string? UserId { get; set; }
    [JsonPropertyName("userName")] public string? UserName { get; set; }
    [JsonPropertyName("voiceCount")] public int VoiceCount { get; set; }
    [JsonPropertyName("createTime")] public long CreateTime { get; set; }
    [JsonPropertyName("lastProgramCreateTime")] public long LastProgramCreateTime { get; set; }
    [JsonPropertyName("categoryId")] public int CategoryId { get; set; }
    [JsonPropertyName("category")] public string? Category { get; set; }
    [JsonPropertyName("secondCategoryId")] public int SecondCategoryId { get; set; }
    [JsonPropertyName("secondCategory")] public string? SecondCategory { get; set; }
    [JsonPropertyName("desc")] public string? Description { get; set; }
    [JsonPropertyName("fee")] public bool Fee { get; set; }
    [JsonPropertyName("tag")] public string? Tag { get; set; }
    [JsonPropertyName("voiceName")] public string? LastVoiceName { get; set; }
    [JsonPropertyName("voiceId")] public string? LastVoiceId { get; set; }
    [JsonPropertyName("lastProgramId")] public string? LastProgramId { get; set; }
    [JsonPropertyName("playCount")] public long PlayCount { get; set; }
    [JsonPropertyName("subCount")] public long SubscribedCount { get; set; }
}

public class DjRadioChannelWithDjDto : DjRadioChannelDto
{
    [JsonPropertyName("dj")] public UserInfoDto? DjData { get; set; }
}
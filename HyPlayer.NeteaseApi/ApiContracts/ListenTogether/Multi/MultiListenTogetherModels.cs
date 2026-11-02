using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Multi;

public class MultiListenTogetherSnapshot
{
    [JsonPropertyName("roomId")] public string? RoomId { get; set; }
    [JsonPropertyName("multiRoomInfoDTO")] public MultiListenTogetherRoomInfo? RoomInfo { get; set; }
    [JsonPropertyName("multiLtRoomUserAgg")] public MultiListenTogetherUserAgg? Users { get; set; }
    [JsonPropertyName("roomPlaySongInfo")] public MultiListenTogetherPlayInfo? PlayInfo { get; set; }
}

public class MultiListenTogetherRoomInfo
{
    [JsonPropertyName("roomId")] public string? RoomId { get; set; }
    [JsonPropertyName("creatorId")] public string? CreatorId { get; set; }
    [JsonPropertyName("roomClosed")] public bool? RoomClosed { get; set; }
}

public class MultiListenTogetherUserAgg
{
    [JsonPropertyName("onlineNums")] public int? OnlineCount { get; set; }
    [JsonPropertyName("onlineUserInfos")] public List<MultiListenTogetherUser>? OnlineUsers { get; set; }
}

public class MultiListenTogetherUser
{
    [JsonPropertyName("uid")] public string? UserId { get; set; }
    [JsonPropertyName("nickname")] public string? Nickname { get; set; }
    [JsonPropertyName("avatar")] public string? AvatarUrl { get; set; }
}

public class MultiListenTogetherPlayInfo
{
    [JsonPropertyName("startTime")] public long? StartTime { get; set; }
    [JsonPropertyName("playedTime")] public long? PlayedTime { get; set; }
    [JsonPropertyName("songDuration")] public long? SongDuration { get; set; }
    [JsonPropertyName("playSong")] public MultiListenTogetherSong? PlaySong { get; set; }
    [JsonPropertyName("nextSongs")] public List<MultiListenTogetherSong>? NextSongs { get; set; }
    [JsonPropertyName("version")] public long? Version { get; set; }
}

public class MultiListenTogetherSong
{
    [JsonPropertyName("songId")] public string? SongId { get; set; }
    [JsonPropertyName("songBizId")] public string? SongBizId { get; set; }
    [JsonPropertyName("songRcmdUid")] public string? RecommenderId { get; set; }
}

public class MultiListenTogetherPermission
{
    [JsonPropertyName("addSong")] public bool? AddSong { get; set; }
    [JsonPropertyName("switchSong")] public bool? SwitchSong { get; set; }
}

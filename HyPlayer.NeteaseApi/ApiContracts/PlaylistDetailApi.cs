using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts;

public class PlaylistDetailApi : RawApiContractBase<PlaylistDetailRequest, PlaylistDetailResponse, ErrorResultBase,
    PlaylistDetailActualRequest>
{
    public override string Url => "https://music.163.com/api/v6/playlist/detail";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest(PlaylistDetailRequest request)
    {
        ActualRequest = new()
                        {
                            {"id", request.Id},
                            {"n", "100000"},
                            {"s", "8"}
                        };
        return Task.CompletedTask;
    }
}

public class PlaylistDetailActualRequest : RawApiActualRequestBase
{
    
}

public class PlaylistDetailRequest : RequestBase
{
    public required string Id { get; set; }
}

public class PlaylistDetailResponse : CodedResponseBase
{
    [JsonPropertyName("privileges")] public SongDetailResponse.PrivilegeItem[]? Privileges { get; set; }
    [JsonPropertyName("playlist")] public PlayListData? Playlist { get; set; }

    public class PlayListData
    {
        [JsonPropertyName("id")] public required string Id { get; set; }
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("description")] public string? Description { get; set; }
        [JsonPropertyName("tags")] public string[]? Tags { get; set; }
        [JsonPropertyName("coverImgUrl")] public string? CoverUrl { get; set; }
        [JsonPropertyName("titleImageUrl")] public string? TitleImageUrl { get; set; }
        [JsonPropertyName("subscribedCount")] public int SubscribedCount { get; set; }
        [JsonPropertyName("playCount")] public int PlayCount { get; set; }
        [JsonPropertyName("trackCount")] public int TrackCount { get; set; }
        [JsonPropertyName("tracks")] public SongDetailResponse.SongItem[]? Tracks { get; set; }
        [JsonPropertyName("trackIds")] public TrackIdItem[]? TrackIds { get; set; }
        [JsonPropertyName("updateTime")] public long UpdateTime { get; set; }
        [JsonPropertyName("createTime")] public long CreateTime { get; set; }
        [JsonPropertyName("creator")] public LoginResponse.ProfileData? Creator { get; set; }
        [JsonPropertyName("subscribed")] public bool? Subscribed { get; set; }

        public class TrackIdItem
        {
            [JsonPropertyName("id")] public required string Id { get; set; }
            [JsonPropertyName("rcmdReason")] public string? RecommendReason { get; set; }
        }
    }
}
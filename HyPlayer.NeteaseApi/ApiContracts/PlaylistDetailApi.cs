using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    /// <summary>
    /// 歌单详情
    /// </summary>
    public static PlaylistDetailApi PlaylistDetailApi = new();
}

public class PlaylistDetailApi : RawApiContractBase<PlaylistDetailRequest, PlaylistDetailResponse, ErrorResultBase,
    PlaylistDetailActualRequest>
{
    public override string Url => "https://music.163.com/api/v6/playlist/detail";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest(PlaylistDetailRequest? request)
    {
        if (request is not null)
            ActualRequest = new()
                            {
                                { "id", request.Id },
                                { "n", "100000" },
                                { "s", "8" }
                            };
        return Task.CompletedTask;
    }
}

public class PlaylistDetailActualRequest : RawApiActualRequestBase
{
}

public class PlaylistDetailRequest : RequestBase
{
    /// <summary>
    /// 歌单 ID
    /// </summary>
    public required string Id { get; set; }
}

public class PlaylistDetailResponse : CodedResponseBase
{
    [JsonPropertyName("privileges")] public PrivilegeDto[]? Privileges { get; set; }
    [JsonPropertyName("playlist")] public PlayListData? Playlist { get; set; }

    public class PlayListData : PlaylistDto
    {
        [JsonPropertyName("tags")] public string[]? Tags { get; set; }

        [JsonPropertyName("titleImageUrl")] public string? TitleImageUrl { get; set; }

        [JsonPropertyName("tracks")] public EmittedSongDto[]? Tracks { get; set; }
        [JsonPropertyName("trackIds")] public TrackIdItem[]? TrackIds { get; set; }

        [JsonPropertyName("createTime")] public long CreateTime { get; set; }

        public class TrackIdItem
        {
            [JsonPropertyName("id")] public required string Id { get; set; }
            [JsonPropertyName("rcmdReason")] public string? RecommendReason { get; set; }
        }
    }
}
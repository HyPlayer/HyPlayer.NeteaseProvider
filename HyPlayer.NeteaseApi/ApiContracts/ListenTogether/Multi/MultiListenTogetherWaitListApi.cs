using HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Multi;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static MultiListenTogetherWaitListApi MultiListenTogetherWaitListApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Multi
{
    public class MultiListenTogetherWaitListApi : DesktopEApiContractBase<MultiListenTogetherWaitListRequest,
        MultiListenTogetherWaitListResponse, ErrorResultBase, MultiListenTogetherWaitListActualRequest>
    {
        public override string IdentifyRoute => "/listentogether/multi/match/wait/song/list";

        public override string Url { get; protected set; } =
            "https://interface3.music.163.com/eapi/listen/together/multi/match/wait/song/list";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new MultiListenTogetherWaitListActualRequest
                {
                    RoomId = Request.RoomId,
                    Page = $"{{\"size\":{Request.Size},\"cursor\":\"{Request.Cursor}\"}}"
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/listen/together/multi/match/wait/song/list";
    }

    public class MultiListenTogetherWaitListRequest : RequestBase
    {
        public required string RoomId { get; set; }
        public int Size { get; set; } = 50;
        public string Cursor { get; set; } = string.Empty;
    }

    public class MultiListenTogetherWaitListResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public MultiListenTogetherWaitListResponseData? Data { get; set; }

        public class MultiListenTogetherWaitListResponseData
        {
            [JsonPropertyName("songLists")] public List<WaitSong>? Songs { get; set; }
            [JsonPropertyName("version")] public long? Version { get; set; }
        }

        public class WaitSong
        {
            [JsonPropertyName("songInfo")] public WaitSongInfo? SongInfo { get; set; }
            [JsonPropertyName("rcmdUid")] public string? RecommenderId { get; set; }
            [JsonPropertyName("nickname")] public string? Nickname { get; set; }
            [JsonPropertyName("rcmdType")] public int? RecommendType { get; set; }
            [JsonPropertyName("status")] public int? Status { get; set; }
        }

        public class WaitSongInfo
        {
            [JsonPropertyName("resourceId")] public string? ResourceId { get; set; }
            [JsonPropertyName("bizId")] public string? BizId { get; set; }
            [JsonPropertyName("title")] public string? Title { get; set; }
            [JsonPropertyName("artistName")] public List<string>? ArtistNames { get; set; }
            [JsonPropertyName("coverUrl")] public string? CoverUrl { get; set; }
        }
    }

    public class MultiListenTogetherWaitListActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("roomId")] public required string RoomId { get; set; }
        [JsonPropertyName("page")] public required string Page { get; set; }
    }
}

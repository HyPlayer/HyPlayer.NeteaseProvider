using HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Dual;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static ListenTogetherHeartBeatApi ListenTogetherHeartBeatApi => new();
    }
}


namespace HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Dual
{

    public class ListenTogetherHeartBeatApi : EApiContractBase<ListenTogetherHeartBeatRequest, ListenTogetherHeartBeatResponse, ErrorResultBase, ListenTogetherHeartBeatActualRequest>
    {
        public override string IdentifyRoute => "/listentogether/heatbeat";
        public override string Url { get; protected set; } = "https://interface3.music.163.com/eapi/listen/together/heartbeat";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new ListenTogetherHeartBeatActualRequest
                {
                    PlayStatus = Request.PlayStatus switch
                    {
                        ListenTogetherHeartBeatRequest.ListenTogetherPlayStatus.Pause => "PAUSED",
                        _ => "PLAY"
                    },
                    RoomId = Request.RoomId,
                    Progress = Request.Progress.ToString(),
                    PlaylistVersion = $"[{{\"userId\":{Request.UserId},\"version\":{Request.PlaylistVersion}}}]",
                    SongId = Request.SongId
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/listen/together/heartbeat";
    }

    public class ListenTogetherHeartBeatRequest : RequestBase
    {
        public required ListenTogetherPlayStatus PlayStatus { get; set; }
        public required string RoomId { get; set; }
        public required long Progress { get; set; }
        public required int PlaylistVersion { get; set; }
        public required string UserId { get; set; }
        public required string SongId { get; set; }

        public enum ListenTogetherPlayStatus
        {
            Play,
            Pause
        }
    }

    public class ListenTogetherHeartBeatResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public ListenTogetherHeartBeatResponseData? Data { get; set; }

        public class ListenTogetherHeartBeatResponseData
        {
            [JsonPropertyName("result")] public bool Result { get; set; }
            [JsonPropertyName("timeSpan")] public int NextHeartbeat { get; set; }
        }
    }

    public class ListenTogetherHeartBeatActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("playStatus")] public required string PlayStatus { get; set; }
        [JsonPropertyName("roomId")] public required string RoomId { get; set; }
        [JsonPropertyName("progress")] public required string Progress { get; set; }
        [JsonPropertyName("playlistVersion")] public required string PlaylistVersion { get; set; }
        [JsonPropertyName("songId")] public required string SongId { get; set; }
    }
}
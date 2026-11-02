using HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Multi;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static MultiListenTogetherHeartbeatApi MultiListenTogetherHeartbeatApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Multi
{
    public class MultiListenTogetherHeartbeatApi : DesktopEApiContractBase<MultiListenTogetherHeartbeatRequest,
        MultiListenTogetherHeartbeatResponse, ErrorResultBase, MultiListenTogetherHeartbeatActualRequest>
    {
        public override string IdentifyRoute => "/listentogether/multi/match/heartbeat";

        public override string Url { get; protected set; } =
            "https://interface3.music.163.com/eapi/listen/together/multi/match/heartbeat";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new MultiListenTogetherHeartbeatActualRequest { RoomId = Request.RoomId };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/listen/together/multi/match/heartbeat";
    }

    public class MultiListenTogetherHeartbeatRequest : RequestBase
    {
        public required string RoomId { get; set; }
    }

    public class MultiListenTogetherHeartbeatResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public MultiListenTogetherHeartbeatResponseData? Data { get; set; }

        public class MultiListenTogetherHeartbeatResponseData
        {
            [JsonPropertyName("roomPlaySongInfo")] public MultiListenTogetherPlayInfo? PlayInfo { get; set; }
            [JsonPropertyName("heartBeatDuration")] public int? HeartbeatSeconds { get; set; }
            [JsonPropertyName("roomInfoDTO")] public MultiListenTogetherRoomInfo? RoomInfo { get; set; }
        }
    }

    public class MultiListenTogetherHeartbeatActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("roomId")] public required string RoomId { get; set; }
    }
}

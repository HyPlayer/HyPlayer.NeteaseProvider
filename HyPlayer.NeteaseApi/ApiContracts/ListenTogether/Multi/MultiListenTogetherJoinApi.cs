using HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Multi;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static MultiListenTogetherJoinApi MultiListenTogetherJoinApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Multi
{
    public class MultiListenTogetherJoinApi : DesktopEApiContractBase<MultiListenTogetherJoinRequest,
        MultiListenTogetherJoinResponse, ErrorResultBase, MultiListenTogetherJoinActualRequest>
    {
        public override string IdentifyRoute => "/listentogether/multi/match/ack";

        public override string Url { get; protected set; } =
            "https://interface3.music.163.com/eapi/listen/together/multi/match/ack";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new MultiListenTogetherJoinActualRequest
                {
                    RoomId = Request.RoomId,
                    InviterId = Request.InviterId,
                    CheckToken = Request.CheckToken
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/listen/together/multi/match/ack";
    }

    public class MultiListenTogetherJoinRequest : RequestBase
    {
        public required string RoomId { get; set; }
        public required string InviterId { get; set; }
        public required string CheckToken { get; set; }
    }

    public class MultiListenTogetherJoinResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public MultiListenTogetherJoinResponseData? Data { get; set; }

        public class MultiListenTogetherJoinResponseData
        {
            [JsonPropertyName("success")] public bool? Success { get; set; }
            [JsonPropertyName("failedMessage")] public string? FailedMessage { get; set; }
            [JsonPropertyName("roomId")] public string? RoomId { get; set; }
            [JsonPropertyName("multiLtRoomSnapshot")] public MultiListenTogetherSnapshot? Snapshot { get; set; }
            [JsonPropertyName("multiRoomPermission")] public MultiListenTogetherPermission? Permission { get; set; }
        }
    }

    public class MultiListenTogetherJoinActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("roomId")] public required string RoomId { get; set; }
        [JsonPropertyName("agree")] public bool Agree => true;
        [JsonPropertyName("inviterUid")] public required string InviterId { get; set; }
        [JsonPropertyName("checkToken")] public required string CheckToken { get; set; }
    }
}

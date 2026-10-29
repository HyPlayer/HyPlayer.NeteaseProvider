using HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Multi;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static MultiListenTogetherExitApi MultiListenTogetherExitApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Multi
{
    public class MultiListenTogetherExitApi : DesktopEApiContractBase<MultiListenTogetherExitRequest,
        MultiListenTogetherExitResponse, ErrorResultBase, MultiListenTogetherExitActualRequest>
    {
        public override string IdentifyRoute => "/listentogether/multi/match/exit";

        public override string Url { get; protected set; } =
            "https://interface3.music.163.com/eapi/listen/together/multi/match/exit";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new MultiListenTogetherExitActualRequest { RoomId = Request.RoomId };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/listen/together/multi/match/exit";
    }

    public class MultiListenTogetherExitRequest : RequestBase
    {
        public required string RoomId { get; set; }
    }

    public class MultiListenTogetherExitResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public MultiListenTogetherExitResponseData? Data { get; set; }

        public class MultiListenTogetherExitResponseData
        {
            [JsonPropertyName("success")] public bool? Success { get; set; }
        }
    }

    public class MultiListenTogetherExitActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("roomId")] public required string RoomId { get; set; }
        [JsonPropertyName("exitType")] public string ExitType => "NORMAL_END";
    }
}

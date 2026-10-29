using HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Multi;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static MultiListenTogetherStatusApi MultiListenTogetherStatusApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Multi
{
    public class MultiListenTogetherStatusApi : DesktopEApiContractBase<MultiListenTogetherStatusRequest,
        MultiListenTogetherStatusResponse, ErrorResultBase, MultiListenTogetherStatusActualRequest>
    {
        public override string IdentifyRoute => "/listentogether/multi/match/status/get";

        public override string Url { get; protected set; } =
            "https://interface3.music.163.com/eapi/listen/together/multi/match/status/get";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/listen/together/multi/match/status/get";
    }

    public class MultiListenTogetherStatusRequest : RequestBase
    {
    }

    public class MultiListenTogetherStatusResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public MultiListenTogetherStatusResponseData? Data { get; set; }

        public class MultiListenTogetherStatusResponseData
        {
            [JsonPropertyName("status")] public string? Status { get; set; }
            [JsonPropertyName("multiLtRoomSnapshot")] public MultiListenTogetherSnapshot? Snapshot { get; set; }
            [JsonPropertyName("multiRoomPermission")] public MultiListenTogetherPermission? Permission { get; set; }
        }
    }

    public class MultiListenTogetherStatusActualRequest : EApiActualRequestBase
    {
    }
}

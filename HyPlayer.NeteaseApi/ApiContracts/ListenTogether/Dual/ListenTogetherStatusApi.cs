using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Category;
using HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Dual;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts
{

public static partial class NeteaseApis
{
    public static ListenTogetherStatusApi ListenTogetherStatusApi => new();
}
}


namespace HyPlayer.NeteaseApi.ApiContracts.Category
{

    public class ListenTogetherStatusApi : EApiContractBase<ListenTogetherStatusRequest, ListenTogetherStatusResponse, ErrorResultBase, ListenTogetherStatusActualRequest>
    {
        public override string IdentifyRoute => "/listentogether/status/get";
        public override string Url { get; protected set; } = "https://interface3.music.163.com/eapi/listen/together/status/get";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/listen/together/status/get";
    }

    public class ListenTogetherStatusRequest : RequestBase
    {

    }

    public class ListenTogetherStatusResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public ListenTogetherStatusResponseData? Data { get; set; }

        public class ListenTogetherStatusResponseData
        {
            [JsonPropertyName("inRoom")] public bool IsInRoom { get; set; }
            [JsonPropertyName("roomInfo")] public ListenTogetherRoomCreateResponse.ListenTogetherRoomInfo? RoomInfo { get; set; }
            [JsonPropertyName("status")] public string? Status { get; set; }
        }
    }

    public class ListenTogetherStatusActualRequest : EApiActualRequestBase
    {

    }
}
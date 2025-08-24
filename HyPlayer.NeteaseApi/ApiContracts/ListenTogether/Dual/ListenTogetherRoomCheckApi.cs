using HyPlayer.NeteaseApi.ApiContracts.ListenTogether;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static ListenTogetherRoomCheckApi ListenTogetherRoomCheckApi => new();
    }
}


namespace HyPlayer.NeteaseApi.ApiContracts.ListenTogether
{

    public class ListenTogetherRoomCheckApi : EApiContractBase<ListenTogetherRoomCheckRequest, ListenTogetherRoomCheckResponse, ErrorResultBase, ListenTogetherRoomCheckActualRequest>
    {
        public override string IdentifyRoute => "/listentogether/room/check";
        public override string Url { get; protected set; } = "https://interface3.music.163.com/eapi/listen/together/room/check";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new ListenTogetherRoomCheckActualRequest
                {
                    RoomId = Request.RoomId
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/listen/together/room/check";
    }

    public class ListenTogetherRoomCheckRequest : RequestBase
    {
        public required string RoomId { get; set; }
    }

    public class ListenTogetherRoomCheckResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public ListenTogetherRoomCheckResponseData? Data { get; set; }

        public class ListenTogetherRoomCheckResponseData
        {
            [JsonPropertyName("joinable")] public bool Joinable { get; set; }
            [JsonPropertyName("type")] public string? Type { get; set; }
            [JsonPropertyName("status")] public string? Status { get; set; }
        }
    }

    public class ListenTogetherRoomCheckActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("roomId")] public required string RoomId { get; set; }
    }
}
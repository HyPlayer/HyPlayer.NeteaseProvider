using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.ListenTogether;
using HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Dual;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static ListenTogetherEndApi ListenTogetherEndApi => new();
    }
}


namespace HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Dual
{
    public class ListenTogetherEndApi : EApiContractBase<ListenTogetherEndRequest, ListenTogetherEndResponse,
        ErrorResultBase, ListenTogetherEndActualRequest>
    {
        public override string IdentifyRoute => "/listentogether/end";

        public override string Url { get; protected set; } =
            "https://interface.music.163.com/eapi/listen/together/end/v2";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new ListenTogetherEndActualRequest
                {
                    RoomId = Request.RoomId
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/listen/together/end/v2";
    }

    public class ListenTogetherEndRequest : RequestBase
    {
        public required string RoomId { get; set; }
    }

    public class ListenTogetherEndResponse : CodedResponseBase
    {
        
    }

    public class ListenTogetherEndActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("roomId")] public required string RoomId { get; set; }
    }
}
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts.DjChannel
{
    public class DjChannelSubscribeApi : EApiContractBase<DjChannelSubscribeRequest, DjChannelSubscribeResponse, ErrorResultBase, DjChannelSubscribeActualRequest>
    {
        public override string ApiPath { get; protected set; } = " /api/djradio/sub";

        public override string IdentifyRoute => "/djchannel/subscribe";

        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/djradio/";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
            {
                ActualRequest = new DjChannelSubscribeActualRequest
                {
                    Id = Request.Id
                };
                Url += Request.IsSubscribe ? "sub" : "unsub";
            }

            return Task.CompletedTask;
        }
    }

    public class DjChannelSubscribeActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("id")] public required string Id { get; set; }
    }

    public class DjChannelSubscribeResponse : CodedResponseBase
    {
    }

    public class DjChannelSubscribeRequest : RequestBase
    {
        public required string Id { get; set; }
        public bool IsSubscribe { get; set; }
    }
}

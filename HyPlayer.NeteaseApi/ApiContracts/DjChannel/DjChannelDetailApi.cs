using HyPlayer.NeteaseApi.ApiContracts.DjChannel;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        /// <summary>
        /// 电台详情
        /// </summary>
        public static DjChannelDetailApi DjChannelDetailApi => new();
    }
}


namespace HyPlayer.NeteaseApi.ApiContracts.DjChannel
{

    public class DjChannelDetailApi : EApiContractBase<DjChannelDetailRequest, DjChannelDetailResponse, ErrorResultBase
        , DjChannelDetailActualRequest>
    {
        public override string IdentifyRoute => "/dj/detail";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/djradio/v3/get";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request?.Id is not null)
                ActualRequest = new DjChannelDetailActualRequest
                {
                    Id = Request.Id
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/djradio/v3/get";
    }

    public class DjChannelDetailRequest : RequestBase
    {
        /// <summary>
        /// 电台 ID
        /// </summary>
        public required string Id { get; set; }
    }

    public class DjChannelDetailResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public DjRadioChannelWithDjDto? RadioData { get; set; }
    }

    public class DjChannelDetailActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("id")] public required string Id { get; set; }
    }
}
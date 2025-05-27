using HyPlayer.NeteaseApi.ApiContracts.Cloud;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static CloudPubApi CloudPubApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Cloud
{

    public class
        CloudPubApi : EApiContractBase<CloudPubRequest, CloudPubResponse, ErrorResultBase, CloudPubActualRequest>
    {
        public override string IdentifyRoute => "/cloud/pub";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/cloud/pub/v2";
        public override string ApiPath { get; protected set; } = "/api/cloud/pub/v2";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new CloudPubActualRequest
                {
                    SongId = Request.SongId
                };
            return Task.CompletedTask;
        }

    }

    public class CloudPubRequest : RequestBase
    {
        public required string SongId { get; set; }
    }

    public class CloudPubResponse : CodedResponseBase
    {
        [JsonPropertyName("privateCloud")] public CloudMusicDto? PrivateCloud { get; set; }
    }

    public class CloudPubActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("songid")] public required string SongId { get; set; }
    }
}
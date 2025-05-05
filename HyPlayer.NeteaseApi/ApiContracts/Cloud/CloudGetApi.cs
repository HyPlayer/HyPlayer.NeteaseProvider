using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Cloud;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static CloudGetApi CloudGetApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Cloud
{

    public class
        CloudGetApi : EApiContractBase<CloudGetRequest, CloudGetResponse, ErrorResultBase, CloudGetActualRequest>
    {

        public override string IdentifyRoute => "/cloud/get";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/v1/cloud/get";
        public override string ApiPath { get; protected set; } = "/api/v1/cloud/get";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new CloudGetActualRequest
                {
                    Offset = Request.Offset,
                    Limit = Request.Limit
                };
            return Task.CompletedTask;
        }

    }

    public class CloudGetRequest : RequestBase
    {
        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = 200;
    }

    public class CloudGetResponse : CodedResponseBase
    {
        [JsonPropertyName("count")] public int Count { get; set; }
        [JsonPropertyName("size")] public long Size { get; set; }
        [JsonPropertyName("maxSize")] public long MaxSize { get; set; }
        [JsonPropertyName("hasMore")] public bool HasMore { get; set; }
        [JsonPropertyName("data")] public CloudMusicDto[]? Songs { get; set; }
    }

    public class CloudGetActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("offset")] public int Offset { get; set; } = 0;

        [JsonPropertyName("limit")] public int Limit { get; set; } = 200;
    }
}
using HyPlayer.NeteaseApi.ApiContracts.Album;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static AlbumSublistApi AlbumSublistApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Album
{
    public class AlbumSublistApi : EApiContractBase<AlbumSublistRequest, AlbumSublistResponse, ErrorResultBase,
        AlbumSublistActualRequest>
    {
        public override string IdentifyRoute => "/album/sublist";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/album/sublist";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new AlbumSublistActualRequest
                {
                    Limit = Request.Limit,
                    Offset = Request.Offset
                };

            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/album/sublist";
    }

    public class AlbumSublistRequest : RequestBase
    {

        public int Limit { get; set; } = 25;
        public int Offset { get; set; } = 0;
    }

    public class AlbumSublistResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public AlbumSublistResponseData[]? Data { get; set; }

        public class AlbumSublistResponseData : AlbumDto
        {
            [JsonPropertyName("subTime")] public long SubTime { get; set; }
        }

        [JsonPropertyName("count")] public int Count { get; set; }
        [JsonPropertyName("hasMore")] public bool HasMore { get; set; }
    }



    public class AlbumSublistActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("limit")] public int Limit { get; set; } = 25;
        [JsonPropertyName("offset")] public int Offset { get; set; }
        [JsonPropertyName("total")] public bool Total => true;
    }
}
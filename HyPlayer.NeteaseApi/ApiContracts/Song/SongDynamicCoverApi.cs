using HyPlayer.NeteaseApi.ApiContracts.Song;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static SongDynamicCoverApi SongDynamicCoverApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Song
{
    public class SongDynamicCoverApi : EApiContractBase<SongDynamicCoverRequest, SongDynamicCoverResponse, ErrorResultBase, SongDynamicCoverActualRequest>
    {
        public override string IdentifyRoute => "/song/dynamic/cover";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/songplay/dynamic-cover";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new SongDynamicCoverActualRequest
                {
                    SongId = Request.SongId
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/songplay/dynamic-cover";
    }

    public class SongDynamicCoverRequest : RequestBase
    {
        public required string SongId { get; set; }
    }

    public class SongDynamicCoverResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public SongDynamicCoverResponseData? Data { get; set; }

        public class SongDynamicCoverResponseData
        {
            [JsonPropertyName("videoPlayUrl")] public string? VideoPlayUrl { get; set; }
        }
    }

    public class SongDynamicCoverActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("songId")] public required string SongId { get; set; }
    }
}

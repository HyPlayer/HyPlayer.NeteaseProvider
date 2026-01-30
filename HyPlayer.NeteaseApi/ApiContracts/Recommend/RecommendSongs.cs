using HyPlayer.NeteaseApi.ApiContracts.Recommend;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases.WeApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        /// <summary>
        /// 推荐歌曲
        /// </summary>
        public static RecommendSongsApi RecommendSongsApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Recommend
{

    public class RecommendSongsApi : WeApiContractBase<RecommendSongsRequest, RecommendSongsResponse, ErrorResultBase,
        RecommendSongsActualRequest>
    {
        public override string IdentifyRoute => "/recommend/songs";
        public override string Url { get; protected set; } = "https://music.163.com/api/v3/discovery/recommend/songs";
        public override HttpMethod Method => HttpMethod.Post;

        public override Dictionary<string, string> Cookies => new() { { "os", "ios" } };

        public override string? UserAgent => "ios";

        public override Task MapRequest(ApiHandlerOption option)
        {
            return Task.CompletedTask;
        }
    }

    public class RecommendSongsRequest : RequestBase
    {

    }

    public class RecommendSongsResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public RecommendSongsData? Data { get; set; }

        public class RecommendSongsData
        {
            [JsonPropertyName("dailySongs")] public RecommendSongItem[]? DailySongs { get; set; }
        }

        public class RecommendSongItem : EmittedSongDtoWithPrivilege
        {
            [JsonPropertyName("recommendReason")] public string? RecommendReason { get; set; }
        }
    }

    public class RecommendSongsActualRequest : WeApiActualRequestBase
    {

    }
}
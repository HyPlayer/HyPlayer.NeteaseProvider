using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Recommend;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        /// <summary>
        /// 推荐歌单
        /// </summary>
        public static RecommendPlaylistsApi RecommendPlaylistsApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Recommend
{
    public class RecommendPlaylistsApi : WeApiContractBase<RecommendPlaylistsRequest, RecommendPlaylistsResponse,
        ErrorResultBase, RecommendPlaylistsActualRequest>
    {
        public override string IdentifyRoute => "/recommend/resource";

        public override string Url { get; protected set; } =
            "https://music.163.com/weapi/v1/discovery/recommend/resource";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest()
        {
            return Task.CompletedTask;
        }
    }

    public class RecommendPlaylistsRequest : RequestBase
    {

    }

    public class RecommendPlaylistsResponse : CodedResponseBase
    {
        [JsonPropertyName("recommend")] public RecommendPlaylistDto[]? Recommends { get; set; }
    }

    public class RecommendPlaylistsActualRequest : WeApiActualRequestBase
    {

    }
}
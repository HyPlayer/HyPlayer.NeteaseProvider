using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Song;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        /// <summary>
        /// 喜欢歌曲
        /// </summary>
        public static LikeApi LikeApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Song
{
    public class LikeApi : EApiContractBase<LikeRequest, LikeResponse, ErrorResultBase, LikeActualRequest>
    {
        public override string IdentifyRoute => "/like";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/song/like";
        public override string ApiPath { get; protected set; } = "/api/song/like";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new LikeActualRequest
                {
                    TrackId = Request.TrackId,
                    UserId = Request.UserId,
                    Like = Request.Like,
                };
            return Task.CompletedTask;
        }
    }

    public class LikeActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("trackId")] public required string TrackId { get; set; }
        [JsonPropertyName("userid")] public required string UserId { get; set; }
        [JsonPropertyName("like")] public bool Like { get; set; } = true;
    }

    public class LikeRequest : RequestBase
    {
        /// <summary>
        /// 歌曲 ID
        /// </summary>
        public required string TrackId { get; set; }

        /// <summary>
        /// 用户 ID
        /// </summary>
        public required string UserId { get; set; }

        /// <summary>
        /// 是否喜欢
        /// </summary>
        public bool Like { get; set; } = true;
    }

    public class LikeResponse : CodedResponseBase
    {
    }
}
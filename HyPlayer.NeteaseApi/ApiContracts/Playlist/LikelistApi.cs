using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Playlist;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        /// <summary>
        /// 喜欢歌曲列表
        /// </summary>
        public static LikelistApi LikelistApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Playlist
{

    public class LikelistApi : WeApiContractBase<LikelistRequest, LikelistResponse, ErrorResultBase,
        LikelistActualRequest>
    {
        public override string IdentifyRoute => "/likelist";
        public override string Url { get; protected set; } = "https://music.163.com/weapi/song/like/get";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest()
        {
            if (Request is not null)
                ActualRequest = new LikelistActualRequest
                {
                    Uid = Request.Uid
                };
            return Task.CompletedTask;
        }
    }

    public class LikelistRequest : RequestBase
    {
        /// <summary>
        /// 用户 ID
        /// </summary>
        public required string Uid { get; set; }
    }

    public class LikelistResponse : CodedResponseBase
    {
        [JsonPropertyName("ids")] public string[]? TrackIds { get; set; }
        [JsonPropertyName("checkPoint")] public long CheckPoint { get; set; }
    }

    public class LikelistActualRequest : WeApiActualRequestBase
    {
        [JsonPropertyName("uid")] public required string Uid { get; set; }
    }
}
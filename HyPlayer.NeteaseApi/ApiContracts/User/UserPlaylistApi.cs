using HyPlayer.NeteaseApi.ApiContracts.User;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        /// <summary>
        /// 喜欢歌曲
        /// </summary>
        public static UserPlaylistApi UserPlaylistApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.User
{
    public class UserPlaylistApi : EApiContractBase<UserPlaylistRequest, UserPlaylistResponse, ErrorResultBase,
        UserPlaylistActualRequest>
    {
        public override string IdentifyRoute => "/user/playlist";
        public override string Url { get; protected set; } = "https://music.163.com/api/user/playlist";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new UserPlaylistActualRequest
                {
                    Uid = Request.Uid,
                    Limit = Request.Limit,
                    Offset = Request.Offset
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/user/playlist";
    }

    public class UserPlaylistRequest : RequestBase
    {
        /// <summary>
        /// 用户 ID
        /// </summary>
        public string? Uid { get; set; }

        /// <summary>
        /// 获取数目
        /// </summary>
        public int Limit { get; set; } = 1000;

        /// <summary>
        /// 起始位置
        /// </summary>
        public int Offset { get; set; } = 0;
    }

    public class UserPlaylistResponse : CodedResponseBase
    {
        [JsonPropertyName("playlist")] public PlaylistDto[]? Playlists { get; set; }
    }

    public class UserPlaylistActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("uid")] public string? Uid { get; set; }
        [JsonPropertyName("limit")] public int Limit { get; set; } = 1000;
        [JsonPropertyName("offset")] public int Offset { get; set; }
    }
}
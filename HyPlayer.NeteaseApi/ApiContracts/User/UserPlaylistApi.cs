using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.User;

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
    public class UserPlaylistApi : WeApiContractBase<UserPlaylistRequest, UserPlaylistResponse, ErrorResultBase,
        UserPlaylistActualRequest>
    {
        public override string IdentifyRoute => "/user/playlist";
        public override string Url { get; protected set; } = "https://music.163.com/api/user/playlist";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest()
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
        public int Limit { get; set; } = 30;

        /// <summary>
        /// 起始位置
        /// </summary>
        public int Offset { get; set; } = 0;
    }

    public class UserPlaylistResponse : CodedResponseBase
    {
        [JsonPropertyName("playlist")] public PlaylistDto[]? Playlists { get; set; }
    }

    public class UserPlaylistActualRequest : WeApiActualRequestBase
    {
        [JsonPropertyName("uid")] public string? Uid { get; set; }
        [JsonPropertyName("limit")] public int Limit { get; set; } = 30;
        [JsonPropertyName("offset")] public int Offset { get; set; }
        [JsonPropertyName("includeVideo")] public bool IncludeVideo => true;
    }
}
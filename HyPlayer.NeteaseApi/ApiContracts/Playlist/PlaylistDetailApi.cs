using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Playlist;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        /// <summary>
        /// 歌单详情
        /// </summary>
        public static PlaylistDetailApi PlaylistDetailApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Playlist
{

    public class PlaylistDetailApi : EApiContractBase<PlaylistDetailRequest, PlaylistDetailResponse, ErrorResultBase,
        PlaylistDetailActualRequest>
    {
        public override string IdentifyRoute => "/playlist/detail";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/playlist/list/get";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new PlaylistDetailActualRequest()
                {
                    IdList = Request.ConvertToIdStringList()
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/playlist/list/get";
    }

    public class PlaylistDetailActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("ids")] public required string IdList { get; set; }
    }

    public class PlaylistDetailRequest : IdOrIdListListRequest
    {
    }

    public class PlaylistDetailResponse : CodedResponseBase
    {
        [JsonPropertyName("playlists")] public PlayListData[]? Playlists { get; set; }

        public class PlayListData : PlaylistDto
        {
            [JsonPropertyName("tags")] public string[]? Tags { get; set; }

            [JsonPropertyName("titleImageUrl")] public string? TitleImageUrl { get; set; }

            [JsonPropertyName("createTime")] public long CreateTime { get; set; }
            [JsonPropertyName("commentCount")] public long CommentCount { get; set; }
            [JsonPropertyName("shareCount")] public long ShareCount { get; set; }
            [JsonPropertyName("newImported")] public bool IsNewImported { get; set; }

        }
    }
}
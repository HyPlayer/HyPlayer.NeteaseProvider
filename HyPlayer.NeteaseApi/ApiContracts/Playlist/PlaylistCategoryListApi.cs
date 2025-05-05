using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Playlist;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static PlaylistCategoryListApi PlaylistCategoryListApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Playlist
{
    public class PlaylistCategoryListApi : EApiContractBase<PlaylistCategoryListRequest, PlaylistCategoryListResponse,
        ErrorResultBase, PlaylistCategoryListActualRequest>
    {
        public override string IdentifyRoute => "/playlist/category/list";

        public override string Url { get; protected set; } =
            "https://interface.music.163.com/eapi/playlist/category/list";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new()
                {
                    Category = Request.Category,
                    Limit = Request.Limit
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/playlist/category/list";
    }

    public class PlaylistCategoryListRequest : RequestBase
    {
        public required string Category { get; set; }
        public int Limit { get; set; } = 6;
    }

    public class PlaylistCategoryListResponse : CodedResponseBase
    {
        [JsonPropertyName("playlists")] public PlaylistDto[]? Playlists { get; set; }
        [JsonPropertyName("playlistIds")] public string[]? PlaylistIds { get; set; }
    }

    public class PlaylistCategoryListActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("cat")] public required string Category { get; set; }
        [JsonPropertyName("limit")] public int Limit { get; set; } = 6;
    }
}
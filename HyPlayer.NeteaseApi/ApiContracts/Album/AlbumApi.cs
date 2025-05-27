using HyPlayer.NeteaseApi.ApiContracts.Album;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static AlbumApi AlbumApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Album
{


    public class AlbumApi : EApiContractBase<AlbumRequest, AlbumResponse, ErrorResultBase, AlbumActualRequest>
    {
        public override string IdentifyRoute => "/album";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/album/v3/detail";
        public override string ApiPath { get; protected set; } = "/api/album/v3/detail";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new AlbumActualRequest
                {
                    Id = Request.Id
                };
            return Task.CompletedTask;
        }
    }

    public class AlbumRequest : RequestBase
    {
        public required string Id { get; set; }
    }

    public class AlbumResponse : CodedResponseBase
    {
        [JsonPropertyName("info")] public AlbumResponseInfo? Info { get; set; }
        [JsonPropertyName("songs")] public EmittedSongDto[]? Songs { get; set; }
        [JsonPropertyName("album")] public AlbumDto? Album { get; set; }

        public class AlbumResponseInfo
        {
            [JsonPropertyName("resourceType")] public NeteaseResourceType ResourceType { get; set; }
            [JsonPropertyName("commentCount")] public long CommentCount { get; set; }
            [JsonPropertyName("likedCount")] public long LikedCount { get; set; }
            [JsonPropertyName("shareCount")] public long ShareCount { get; set; }
            [JsonPropertyName("threadId")] public string? ThreadId { get; set; }
        }
    }

    public class AlbumActualRequest : CacheKeyEApiActualRequest
    {
        [JsonPropertyName("id")] public required string Id { get; set; }
    }
}
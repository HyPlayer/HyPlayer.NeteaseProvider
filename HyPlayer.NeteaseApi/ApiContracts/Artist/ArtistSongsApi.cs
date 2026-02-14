using HyPlayer.NeteaseApi.ApiContracts.Artist;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        /// <summary>
        /// 歌手歌曲
        /// </summary>
        public static ArtistSongsApi ArtistSongsApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Artist
{

    public class ArtistSongsApi : EApiContractBase<ArtistSongsRequest, ArtistSongsResponse, ErrorResultBase,
        ArtistSongsActualRequest>
    {
        public override string IdentifyRoute => "/api/v2/artist/songs";
        public override string Url { get; protected set; } = "https://interfacepc.music.163.com/eapi/v2/artist/songs";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new ArtistSongsActualRequest
                {
                    Id = Request.ArtistId,
                    Offset = Request.Offset,
                    Limit = Request.Limit
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/v2/artist/songs";
    }

    public class ArtistSongsActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("id")] public required string Id { get; set; }
        [JsonPropertyName("offset")] public int Offset { get; set; } = 0;
        [JsonPropertyName("limit")] public int Limit { get; set; } = 100;
    }

    public class ArtistSongsRequest : RequestBase
    {
        /// <summary>
        /// 歌手 ID
        /// </summary>
        public required string ArtistId { get; set; }

        /// <summary>
        /// 起始位置
        /// </summary>
        public int Offset { get; set; } = 0;

        /// <summary>
        /// 获取数量
        /// </summary>
        public int Limit { get; set; } = 200;
    }

    public class ArtistSongsResponse : CodedResponseBase
    {
        [JsonPropertyName("total")] public int Total { get; set; }
        [JsonPropertyName("more")] public bool HasMore { get; set; }
        [JsonPropertyName("songs")] public ArtistSongDto[]? Songs { get; set; }
    }
}
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Artist;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static ArtistDetailApi ArtistDetailApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Artist
{

    public class ArtistDetailApi : EApiContractBase<ArtistDetailRequest, ArtistDetailResponse, ErrorResultBase,
        ArtistDetailActualRequest>
    {
        public override string IdentifyRoute => "/artist/detail";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/artist/v3/detail";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new ArtistDetailActualRequest
                {
                    ArtistId = Request.ArtistId
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/artist/v3/detail";
    }

    public class ArtistDetailRequest : RequestBase
    {
        public required string ArtistId { get; set; }
        public int TopSong { get; set; } = 50;
    }

    public class ArtistDetailResponse : CodedResponseBase
    {
        [JsonPropertyName("artist")] public ArtistDetailDto? Artist { get; set; }
        [JsonPropertyName("hotSongs")] public EmittedSongDto[]? HotSongs { get; set; }

        public class ArtistDetailDto
        {
            [JsonPropertyName("id")] public string? Id { get; set; }
            [JsonPropertyName("name")] public string? Name { get; set; }
            [JsonPropertyName("alias")] public string[]? Alias { get; set; }
            [JsonPropertyName("followed")] public bool Followed { get; set; }
            [JsonPropertyName("cover")] public string? PicUrl { get; set; }
            [JsonPropertyName("avatar")] public string? Img1v1Url { get; set; }
            [JsonPropertyName("briefDesc")] public string? BriefDesc { get; set; }
            [JsonPropertyName("trans")] public string? Translation { get; set; }
            [JsonPropertyName("musicSize")] public int MusicSize { get; set; }
            [JsonPropertyName("albumSize")] public int AlbumSize { get; set; }
            [JsonPropertyName("mvSize")] public int MvSize { get; set; }
            [JsonPropertyName("transNames")] public string[]? TransNames { get; set; }
        }
    }

    public class ArtistDetailActualRequest : CacheKeyEApiActualRequest
    {
        [JsonPropertyName("id")] public required string ArtistId { get; set; }
        [JsonPropertyName("top")] public int TopSong { get; set; } = 50;
    }
}
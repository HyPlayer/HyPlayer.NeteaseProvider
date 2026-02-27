using HyPlayer.NeteaseApi.ApiContracts.Recommend;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using HyPlayer.NeteaseApi.Models;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static SearchApi SearchApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Recommend
{

    public class SearchApi : EApiContractBase<SearchRequest, SearchResponse, ErrorResultBase, SearchActualRequest>
    {
        public override string IdentifyRoute => "/cloudsearch";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/cloudsearch/pc";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
            {
                var resourceType = Request.Type switch
                {
                    NeteaseResourceType.Song => 1,
                    NeteaseResourceType.Album => 10,
                    NeteaseResourceType.Artist => 100,
                    NeteaseResourceType.Playlist => 1000,
                    NeteaseResourceType.User => 1002,
                    NeteaseResourceType.MV => 1004,
                    NeteaseResourceType.Lyric => 1006,
                    NeteaseResourceType.RadioChannel => 1009,
                    NeteaseResourceType.Video => 1014,
                    NeteaseResourceType.MLog => 1014,
                    NeteaseResourceType.Complex => 1018,
                    _ => 1
                };
                ActualRequest = new()
                {
                    Keyword = Request.Keyword,
                    Type = resourceType,
                    Limit = Request.Limit,
                    Offset = Request.Offset
                };
            }

            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/cloudsearch/pc";
    }

    public class SearchRequest : RequestBase
    {
        public required string Keyword { get; set; }
        public NeteaseResourceType Type { get; set; } = NeteaseResourceType.Song;
        public int Limit { get; set; } = 30;
        public int Offset { get; set; } = 0;
    }

    public class SearchResponse : CodedResponseBase
    {
    }

    public class SearchPlaylistResponse : CodedResponseBase
    {
        [JsonPropertyName("result")] public SearchPlaylistResult? Result { get; set; }

        public class SearchPlaylistResult
        {
            [JsonPropertyName("playlists")] public PlaylistDto[]? Items { get; set; }
            [JsonPropertyName("playlistCount")] public int Count { get; set; }
        }
    }

    public class SearchAlbumResponse : CodedResponseBase
    {
        [JsonPropertyName("result")] public SearchAlbumResult? Result { get; set; }

        public class SearchAlbumResult
        {
            [JsonPropertyName("albums")] public AlbumDto[]? Items { get; set; }
            [JsonPropertyName("albumCount")] public int Count { get; set; }
        }
    }


    public class SearchUserResponse : CodedResponseBase
    {
        [JsonPropertyName("result")] public SearchUserResult? Result { get; set; }

        public class SearchUserResult
        {
            [JsonPropertyName("userprofiles")] public UserInfoDto[]? Items { get; set; }
            [JsonPropertyName("userprofileCount")] public int Count { get; set; }
        }
    }

    public class SearchLyricResponse : CodedResponseBase
    {
        [JsonPropertyName("result")] public SearchLyricResult? Result { get; set; }

        public class SearchLyricResult
        {
            [JsonPropertyName("songs")] public LyricSearchResultDto[]? Items { get; set; }
            [JsonPropertyName("songCount")] public int Count { get; set; }

            public class LyricSearchResultDto : EmittedSongDtoWithPrivilege
            {
                [JsonPropertyName("lyrics")] public string[]? Lyrics { get; set; }
            }
        }
    }

    public class SearchSongResponse : CodedResponseBase
    {
        [JsonPropertyName("result")] public SearchSongResult? Result { get; set; }

        public class SearchSongResult
        {
            [JsonPropertyName("songs")] public EmittedSongDtoWithPrivilege[]? Items { get; set; }
            [JsonPropertyName("songCount")] public int Count { get; set; }
        }
    }

    public class SearchMVResponse : CodedResponseBase
    {
        [JsonPropertyName("result")] public SearchMVResult? Result { get; set; }

        public class SearchMVResult
        {
            [JsonPropertyName("mvs")] public MVDto[]? Items { get; set; }
            [JsonPropertyName("mvCount")] public int Count { get; set; }
        }
    }

    public class SearchArtistResponse : CodedResponseBase
    {
        [JsonPropertyName("result")] public SearchArtistResult? Result { get; set; }

        public class SearchArtistResult
        {
            [JsonPropertyName("artists")] public ArtistDto[]? Items { get; set; }
            [JsonPropertyName("artistCount")] public int Count { get; set; }
        }
    }

    public class SearchVideoResponse : CodedResponseBase
    {
        [JsonPropertyName("result")] public SearchVideoResult? Result { get; set; }

        public class SearchVideoResult
        {
            [JsonPropertyName("videos")] public VideoDto[]? Items { get; set; }
            [JsonPropertyName("videoCount")] public int Count { get; set; }
        }
    }

    public class SearchRadioResponse : CodedResponseBase
    {
        [JsonPropertyName("result")] public SearchRadioResult? Result { get; set; }

        public class SearchRadioResult
        {
            [JsonPropertyName("djRadios")] public DjRadioChannelWithDjDto[]? Items { get; set; }
            [JsonPropertyName("djRadiosCount")] public int Count { get; set; }
        }
    }

    public class SearchActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("s")] public required string Keyword { get; set; }
        [JsonPropertyName("type")] public int Type { get; set; }
        [JsonPropertyName("limit")] public int Limit { get; set; } = 30;
        [JsonPropertyName("offset")] public int Offset { get; set; } = 0;
        [JsonPropertyName("total")] public bool Total => true;
    }
}
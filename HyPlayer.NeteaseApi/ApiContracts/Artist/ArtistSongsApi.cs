using HyPlayer.NeteaseApi.ApiContracts.Artist;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
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
        public override string IdentifyRoute => "/artist/songs";
        public override string Url { get; protected set; } = "https://interface3.music.163.com/eapi/v2/artist/songs";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new ArtistSongsActualRequest
                {
                    Id = Request.ArtistId,
                    OrderType = Request.OrderType switch
                    {
                        ArtistSongsOrderType.Time => "time",
                        _ => "hot"
                    },
                    WorkType = Request.WorkType switch
                    {
                        ArtistSongsWorkType.All => 1,
                        ArtistSongsWorkType.Sing => 5,
                        ArtistSongsWorkType.Lyric => 6,
                        ArtistSongsWorkType.Compose => 7,
                        _ => 1
                    },
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
        [JsonPropertyName("private_cloud")] public bool PrivateCloud => true;
        [JsonPropertyName("work_type")] public int WorkType = 1;
        [JsonPropertyName("order")] public string OrderType { get; set; } = "hot";
        [JsonPropertyName("offset")] public int Offset { get; set; } = 0;
        [JsonPropertyName("limit")] public int Limit { get; set; } = 100;
    }

    public enum ArtistSongsOrderType
    {
        Hot,
        Time
    }

    public enum ArtistSongsWorkType
    {
        All,
        Sing,
        Lyric,
        Compose
    }

    public class ArtistSongsRequest : RequestBase
    {
        /// <summary>
        /// 歌手 ID
        /// </summary>
        public required string ArtistId { get; set; }

        /// <summary>
        /// 排序类型 hot, time
        /// </summary>
        public ArtistSongsOrderType OrderType { get; set; } = ArtistSongsOrderType.Hot;

        /// <summary>
        ///作品类型
        /// </summary>
        public ArtistSongsWorkType WorkType { get; set; } = ArtistSongsWorkType.All;

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
        [JsonPropertyName("songs")] public EmittedSongDtoWithPrivilege[]? Songs { get; set; }
    }
}
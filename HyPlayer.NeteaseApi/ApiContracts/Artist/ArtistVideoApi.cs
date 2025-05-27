using HyPlayer.NeteaseApi.ApiContracts.Artist;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static ArtistVideoApi ArtistVideoApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Artist
{

    public class ArtistVideoApi : EApiContractBase<ArtistVideoRequest, ArtistVideoResponse, ErrorResultBase,
        ArtistVideoActualRequest>
    {
        public override string IdentifyRoute => "/artist/video";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/mlog/artist/video";
        public override string ApiPath { get; protected set; } = "/api/mlog/artist/video";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
            {
                var page = new Dictionary<string, int>()
                {
                    ["size"] = Request.Limit,
                    ["cursor"] = Request.Offset
                };
                ActualRequest = new()
                {
                    ArtistId = Request.ArtistId,
                    Tab = Request.Tab,
                    Order = Request.Order,
                    Page = JsonSerializer.Serialize(page)
                };
            }

            return Task.CompletedTask;
        }
    }

    public class ArtistVideoRequest : RequestBase
    {
        public required string ArtistId { get; set; }
        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = 20;
        public int Order { get; set; } = 0;
        public int Tab { get; set; } = 1;
    }

    public class ArtistVideoResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public ArtistVideoResponseData? Data { get; set; }

        public class ArtistVideoResponseData
        {
            [JsonPropertyName("records")] public ArtistVideoResponseDataRecord[]? Records { get; set; }
            [JsonPropertyName("page")] public ArtistVideoResponsePage? Page { get; set; }

            public class ArtistVideoResponseDataRecord
            {
                [JsonPropertyName("id")] public string? Id { get; set; }
                [JsonPropertyName("type")] public int Type { get; set; }
                [JsonPropertyName("resource")] public ArtistVideoResponseResource? Resource { get; set; }

                public class ArtistVideoResponseResource
                {
                    [JsonPropertyName("mlogBaseData")] public ArtistVideoResponseBaseData? BaseData { get; set; }
                    [JsonPropertyName("mlogExtVO")] public ArtistVideoResponseExtVO? ExtVO { get; set; }
                    [JsonPropertyName("userProfile")] public UserInfoDto? UserProfile { get; set; }


                    public class ArtistVideoResponseExtVO
                    {
                        [JsonPropertyName("likedCount")] public int LikedCount { get; set; }
                        [JsonPropertyName("commentCount")] public int CommentCount { get; set; }
                        [JsonPropertyName("shareCount")] public int ShareCount { get; set; }
                        [JsonPropertyName("playCount")] public int PlayCount { get; set; }
                        [JsonPropertyName("liked")] public bool IsLiked { get; set; }
                        [JsonPropertyName("song")] public SongDto? Song { get; set; }
                        [JsonPropertyName("artists")] public ArtistDto[]? Artists { get; set; }
                    }

                    public class ArtistVideoResponseBaseData
                    {
                        [JsonPropertyName("id")] public string? Id { get; set; }
                        [JsonPropertyName("userId")] public string? UserId { get; set; }
                        [JsonPropertyName("type")] public int Type { get; set; }
                        [JsonPropertyName("originalTitle")] public string? OriginalTitle { get; set; }
                        [JsonPropertyName("text")] public string? Title { get; set; }
                        [JsonPropertyName("desc")] public string? Description { get; set; }
                        [JsonPropertyName("pubTime")] public long PublishTime { get; set; }
                        [JsonPropertyName("coverUrl")] public string? CoverUrl { get; set; }
                        [JsonPropertyName("duration")] public long Duration { get; set; }
                        [JsonPropertyName("videos")] public VideoUrlResult[]? Videos { get; set; }
                        [JsonPropertyName("relatedPubUsers")] public UserInfoDto[]? RelatedPubUsers { get; set; }

                    }
                }
            }

            public class ArtistVideoResponsePage
            {
                [JsonPropertyName("size")] public int Size { get; set; }
                [JsonPropertyName("cursor")] public int Cursor { get; set; }
                [JsonPropertyName("more")] public bool HasMore { get; set; }
            }
        }
    }

    public class ArtistVideoActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("artistId")] public required string ArtistId { get; set; }
        [JsonPropertyName("tab")] public int Tab { get; set; } = 1;
        [JsonPropertyName("order")] public int Order { get; set; } = 0;
        [JsonPropertyName("page")] public required string Page { get; set; }
    }
}
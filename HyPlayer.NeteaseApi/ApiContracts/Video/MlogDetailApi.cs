using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Video;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static MlogDetailApi MlogDetailApi => new();
    }
}


namespace HyPlayer.NeteaseApi.ApiContracts.Video
{
    public class MlogDetailApi : EApiContractBase<MlogDetailRequest, MlogDetailResponse, ErrorResultBase,
        MlogDetailActualRequest>
    {
        public override string IdentifyRoute => "/mlog/detail";
        public override string Url { get; protected set; } = "https://interface3.music.163.com/eapi/mlog/box/detail";
        public override string ApiPath { get; protected set; } = "/api/mlog/box/detail";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
            {
                ActualRequest = new MlogDetailActualRequest
                {
                    Id = Request.MlogId,
                    Resolution = Request.Resolution
                };
                if (!string.IsNullOrEmpty(Request.SongId))
                {
                    ActualRequest.ExtInfo = JsonSerializer.Serialize(new Dictionary<string, string>()
                    {
                        ["songId"] = Request.SongId!,
                        ["keyword"] = ""
                    });
                }
            }

            return Task.CompletedTask;
        }

    }

    public class MlogDetailRequest : RequestBase
    {
        public required string MlogId { get; set; }
        public string Resolution { get; set; } = "480";
        public string? SongId { get; set; }
    }

    public class MlogDetailResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public MlogDetailResponseData? Data { get; set; }

        public class MlogDetailResponseData
        {
            [JsonPropertyName("id")] public string? Id { get; set; }
            [JsonPropertyName("type")] public int Type { get; set; }
            [JsonPropertyName("resource")] public MlogDetailResponseDataResource? Resource { get; set; }

            public class MlogDetailResponseDataResource
            {
                [JsonPropertyName("threadId")] public string? ThreadId { get; set; }
                [JsonPropertyName("commentCount")] public long CommentCount { get; set; }
                [JsonPropertyName("likedCount")] public long LikedCount { get; set; }
                [JsonPropertyName("shareCount")] public long ShareCount { get; set; }
                [JsonPropertyName("liked")] public bool Liked { get; set; }
                [JsonPropertyName("id")] public string? Id { get; set; }
                [JsonPropertyName("userId")] public string? UserId { get; set; }
                [JsonPropertyName("type")] public int Type { get; set; }
                [JsonPropertyName("content")] public MlogDetailResponseContent? Content { get; set; }
                [JsonPropertyName("profile")] public UserInfoDto? Profile { get; set; }
                [JsonPropertyName("pubTime")] public long PublishTime { get; set; }
                [JsonPropertyName("shareUrl")] public string? ShareUrl { get; set; }

                public class MlogDetailResponseContent
                {
                    [JsonPropertyName("title")] public string? Title { get; set; }
                    [JsonPropertyName("text")] public string? Text { get; set; }
                    [JsonPropertyName("video")] public MlogUrlResponse.MlogUrlItem? Video { get; set; }
                    [JsonPropertyName("song")] public FlattedSongWithPrivilegeDto? Song { get; set; }
                    [JsonPropertyName("relatedSongs")] public FlattedSongWithPrivilegeDto[]? RelatedSongs { get; set; }
                }

            }
        }
    }

    public class MlogDetailActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("id")] public required string Id { get; set; }
        [JsonPropertyName("resolution")] public string Resolution { get; set; } = "480";
        [JsonPropertyName("type")] public int Type => 1;
        [JsonPropertyName("rcmdType")] public int RcmdType => 20;
        [JsonPropertyName("songId")] public int SongId => 0; 
        [JsonPropertyName("firstVideo")] public int FirstVideo => 0;
        [JsonPropertyName("extInfo")] public string? ExtInfo { get; set; }
    }
}
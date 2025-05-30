﻿using HyPlayer.NeteaseApi.ApiContracts.Video;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static VideoDetailApi VideoDetailApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Video
{
    public class VideoDetailApi : EApiContractBase<VideoDetailRequest, VideoDetailResponse, ErrorResultBase,
        VideoDetailActualRequest>
    {
        public override string IdentifyRoute => "/video/detail";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/mlog/detail/v1";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new VideoDetailActualRequest
                {
                    Id = Request.Id,
                    Type = Request.Type
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/mlog/detail/v1";
    }

    public class VideoDetailRequest : RequestBase
    {
        public required string Id { get; set; }
        public int Type { get; set; } = 2;
    }

    public class VideoDetailResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public VideoDetailData? Data { get; set; }

        public class VideoDetailData
        {
            [JsonPropertyName("id")] public string? Id { get; set; }
            [JsonPropertyName("type")] public int Type { get; set; }
            [JsonPropertyName("resource")] public VideoDetailResource? Resource { get; set; }

            public class VideoDetailResource
            {
                [JsonPropertyName("commentCount")] public long CommentCount { get; set; }
                [JsonPropertyName("likedCount")] public long LikedCount { get; set; }
                [JsonPropertyName("shareCount")] public long ShareCount { get; set; }
                [JsonPropertyName("liked")] public bool Liked { get; set; }
                [JsonPropertyName("threadId")] public string? ThreadId { get; set; }
                [JsonPropertyName("data")] public VideoDetailResourceData? Data { get; set; }
                [JsonPropertyName("mp")] public VideoDetailResourceMp? Mp { get; set; }

                public class VideoDetailResourceMp
                {
                    [JsonPropertyName("id")] public string? Id { get; set; }
                    [JsonPropertyName("pl")] public int PlayResolution { get; set; }
                    [JsonPropertyName("dl")] public int DownloadResolution { get; set; }
                }

                public class VideoDetailResourceData
                {
                    [JsonPropertyName("id")] public string? Id { get; set; }
                    [JsonPropertyName("name")] public string? Name { get; set; }
                    [JsonPropertyName("playCount")] public long PlayCount { get; set; }
                    [JsonPropertyName("artistId")] public string? ArtistId { get; set; }
                    [JsonPropertyName("artistName")] public string? ArtistName { get; set; }
                    [JsonPropertyName("desc")] public string? Description { get; set; }
                    [JsonPropertyName("briefDesc")] public string? BriefDescription { get; set; }
                    [JsonPropertyName("subCount")] public long SubCount { get; set; }
                    [JsonPropertyName("duration")] public long Duration { get; set; }
                    [JsonPropertyName("publishTime")] public string? PublishTime { get; set; }
                    [JsonPropertyName("brs")] public VideoDetailResponseResourceBrItem[]? Brs { get; set; }
                    [JsonPropertyName("artists")] public ArtistDto[]? Artists { get; set; }
                    [JsonPropertyName("relatedSong")] public SongDto[]? RelatedSongs { get; set; }
                    [JsonPropertyName("videos")] public VideoUrlResult[]? Videos { get; set; }

                    public class VideoDetailResponseResourceBrItem
                    {
                        [JsonPropertyName("size")] public float Size { get; set; }
                        [JsonPropertyName("br")] public int Br { get; set; }
                        [JsonPropertyName("point")] public int Url { get; set; }
                    }
                }

            }
        }
    }

    public class VideoDetailActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("id")] public required string Id { get; set; }
        [JsonPropertyName("type")] public int Type { get; set; } = 2;
    }
}
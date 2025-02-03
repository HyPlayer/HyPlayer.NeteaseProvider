using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static VideoDetailApi VideoDetailApi => new();
}

public class VideoDetailApi : EApiContractBase<VideoDetailRequest, VideoDetailResponse, ErrorResultBase,
    VideoDetailActualRequest>
{
    public override string IdentifyRoute => "/video/detail";
    public override string Url => "https://interface.music.163.com/eapi/mlog/detail/v1";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest(VideoDetailRequest? request)
    {
        if (request is not null)
            ActualRequest = new VideoDetailActualRequest
            {
                Id = request.Id,
                Type = request.Type
            };
        return Task.CompletedTask;
    }

    public override string ApiPath => "/api/mlog/detail/v1";
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
            [JsonPropertyName("mp")] public VideoDetailResourceMp Mp { get; set; }

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
                [JsonPropertyName("subCount")] public long SubCount { get; set; }
                [JsonPropertyName("duration")] public long Duration { get; set; }
                [JsonPropertyName("publishTime")] public string? PublishTime { get; set; }
                [JsonPropertyName("brs")] public VideoDetailResponseResourceBrItem[]? Brs { get; set; }
                [JsonPropertyName("artists")] public ArtistDto[]? Artists { get; set; }
                [JsonPropertyName("relatedSong")] public SongDto[]? RelatedSongs { get; set; }
                [JsonPropertyName("videos")] public VideoUrlResult[]? Videos { get; set; }
                
                public class VideoDetailResponseResourceBrItem
                {
                    [JsonPropertyName("size")] public long Size { get; set; }
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
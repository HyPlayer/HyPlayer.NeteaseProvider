using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Video;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static MlogUrlApi MlogUrlApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Video
{
    public class MlogUrlApi : EApiContractBase<MlogUrlRequest, MlogUrlResponse, ErrorResultBase, MlogUrlActualRequest>
    {
        public override string IdentifyRoute => "/mlog/video/url";
        public override string Url { get; protected set; } = "https://interface3.music.163.com/eapi/mlog/video/ur";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest()
        {
            if (Request is not null)
            {
                ActualRequest = new MlogUrlActualRequest
                {
                    Ids = Request.ConvertToQuotedIdStringList(),
                    Resolution = Request.Resolution
                };
            }

            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/mlog/video/url";
    }

    public class MlogUrlRequest : IdOrIdListListRequest
    {
        public string Resolution { get; set; } = "480";
    }

    public class MlogUrlResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public Dictionary<string, MlogUrlItem?>? Data { get; set; }

        public class MlogUrlItem
        {
            [JsonPropertyName("videoKey")] public string? VideoKey { get; set; }
            [JsonPropertyName("nosKey")] public string? NosKey { get; set; }
            [JsonPropertyName("duration")] public long Duration { get; set; }
            [JsonPropertyName("size")] public long Size { get; set; }
            [JsonPropertyName("coverUrl")] public string? CoverUrl { get; set; }
            [JsonPropertyName("frameUrl")] public string? FrameUrl { get; set; }
            [JsonPropertyName("width")] public int Width { get; set; }
            [JsonPropertyName("height")] public int Height { get; set; }
            [JsonPropertyName("urlInfo")] public MlogUrlInfo? UrlInfo { get; set; }
            [JsonPropertyName("rcmdUrlInfo")] public MlogUrlInfo? RcmdInfo { get; set; }
            [JsonPropertyName("urlInfos")] public MlogUrlInfo[]? UrlInfos { get; set; }
            [JsonPropertyName("coverImageTime")] public long CoverImageTime { get; set; }

            public class MlogUrlInfo
            {
                [JsonPropertyName("id")] public string? Id { get; set; }
                [JsonPropertyName("url")] public string? Url { get; set; }
                [JsonPropertyName("size")] public long Size { get; set; }
            }
        }
    }

    public class MlogUrlActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("mlogIds")] public required string Ids { get; set; }
        [JsonPropertyName("resolution")] public string Resolution { get; set; } = "480";
        [JsonPropertyName("type")] public int Type => 1;
        [JsonPropertyName("scene")] public int Scene => 0;
        [JsonPropertyName("netstate")] public int NetState => 1;
    }
}
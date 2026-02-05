using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using System.Text.Json;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Video;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static VideoUnsubscribeApi VideoUnsubscribeApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Video
{
    public class VideoUnsubscribeApi : EApiContractBase<VideoUnsubscribeRequest, VideoUnsubscribeResponse, ErrorResultBase,
        VideoUnsubscribeActualRequest>
    {
        public override string ApiPath { get; protected set; } = "/api/video/unsub";

        public override string IdentifyRoute => "/video/unsub";

        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/video/";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
            {
                ActualRequest = new VideoUnsubscribeActualRequest
                {
                    // serialize C# array of ids to JSON array string expected by the API
                    VideoIds = JsonSerializer.Serialize(Request.VideoIds)
                };
                Url += "unsub";
            }
            return Task.CompletedTask;
        }
    }

    public class VideoUnsubscribeRequest : RequestBase
    {
        /// <summary>
        /// Array of artist ids. Will be converted to a JSON array string in the request, e.g. new long[] { 35374786 } -> "[35374786]"
        /// </summary>
        public required string[] VideoIds { get; set; }
    }

    public class VideoUnsubscribeResponse : CodedResponseBase
    {
    }

    public class VideoUnsubscribeActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("mvIds")] public required string VideoIds { get; set; }
    }
}
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
        public override string ApiPath { get; protected set; } = "/api/mv/unsub";

        public override string IdentifyRoute => "/mv/unsub";

        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/mv/unsub";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
            {
                ActualRequest = new VideoUnsubscribeActualRequest
                {
                    VideoIds = Request.ConvertToQuotedIdStringList()
                };
            }
            return Task.CompletedTask;
        }
    }

    public class VideoUnsubscribeRequest : IdOrIdListListRequest
    {
        
    }

    public class VideoUnsubscribeResponse : CodedResponseBase
    {
    }

    public class VideoUnsubscribeActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("mvIds")] public required string VideoIds { get; set; }
    }
}
using HyPlayer.NeteaseApi.ApiContracts.Video;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static VideoSubscribeApi VideoSubscribeApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Video
{

    public class VideoSubscribeApi : EApiContractBase<VideoSubscribeRequest, VideoSubscribeResponse, ErrorResultBase,
        VideoSubscribeActualRequest>
    {
        public override string IdentifyRoute => "/mv/subscribe";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/mv/sub";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
            {
                ActualRequest = new VideoSubscribeActualRequest
                {
                    MvId = Request.MvId
                };
            }

            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/mv/sub";
    }

    public class VideoSubscribeRequest : RequestBase
    {
        public required string MvId { get; set; }
    }

    public class VideoSubscribeResponse : CodedResponseBase
    {
    }

    public class VideoSubscribeActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("mvId")] public required string MvId { get; set; }
    }
}
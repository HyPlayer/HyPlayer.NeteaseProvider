using HyPlayer.NeteaseApi.ApiContracts.PersonalFM;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static AiDjSkipApi AiDjSkipApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.PersonalFM
{
    public class
        AiDjSkipApi : EApiContractBase<AiDjSkipRequest, AiDjSkipResponse, ErrorResultBase, AiDjSkipActualRequest>
    {
        public override string IdentifyRoute => "/aidj/skip";
        public override string Url { get; protected set; } = "https://interface3.music.163.com/eapi/v1/radio/skip";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
            {
                Url =
                    $"https://interface3.music.163.com/eapi/v1/radio/skip?songId={Request.SongId}&time={Request.Time}&mode={Request.Mode}&subMode={Request.SubMode}&source=userfm";
                ActualRequest = new AiDjSkipActualRequest
                {
                    SongId = Request.SongId,
                    Time = Request.Time
                };
            }

            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/v1/radio/skip";
    }

    public class AiDjSkipRequest : RequestBase
    {
        public required string SongId { get; set; }
        public long Time { get; set; }

        public string Mode { get; set; } =
            "DEFAULT"; // aidj, DEFAULT, FAMILIAR, EXPLORE, SCENE_RCMD ( EXERCISE, FOCUS, NIGHT_EMO  )

        public string? SubMode { get; set; } = null;
    }

    public class AiDjSkipResponse : CodedResponseBase
    {
    }

    public class AiDjSkipActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("songId")] public required string SongId { get; set; }
        [JsonPropertyName("time")] public long Time { get; set; }
        [JsonPropertyName("mode")] public string Mode = "DEFAULT";
        [JsonPropertyName("subMode")] public string? SubMode = null;
        [JsonPropertyName("source")] public string Source => "userfm";
    }
}
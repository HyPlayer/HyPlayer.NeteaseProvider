using HyPlayer.NeteaseApi.ApiContracts.PersonalFM;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static AiDjContentRcmdInfoApi AiDjContentRcmdInfoApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.PersonalFM
{

    public class AiDjContentRcmdInfoApi : EApiContractBase<AiDjContentRcmdInfoRequest, AiDjContentRcmdInfoResponse,
        ErrorResultBase, AiDjContentRcmdInfoActualRequest>
    {
        public override string IdentifyRoute => "/aidj/content/rcmd/info";

        public override string Url { get; protected set; } =
            "https://interface3.music.163.com/eapi/aidj/content/rcmd/info";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request == null) return Task.CompletedTask;
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var extInfo = new AiDjActualRequestExtInfo
            {
                LastRequestTimestamp = timestamp,
                ListenedTs = Request.IsNewToAidj,
                NoAidjToAidj = Request.IsNewToAidj
            };
            if (Request is { Latitude: not null, Longitude: not null })
            {
                extInfo.LbsInfoList =
                [
                    new AiDjActualRequestExtInfo.LbsInfo
                    {
                        Latitude = Request.Latitude.Value,
                        Longitude = Request.Longitude.Value,
                        Time = timestamp
                    }
                ];
            }

            ActualRequest = new AiDjContentRcmdInfoActualRequest
            {
                ExtInfo = JsonSerializer.Serialize(extInfo)
            };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/aidj/content/rcmd/info";
    }

    public class AiDjContentRcmdInfoRequest : RequestBase
    {
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public bool IsNewToAidj { get; set; } = false;
    }

    public class AiDjContentRcmdInfoResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public AiDjContentRcmdInfoData? Data { get; set; }

        public class AiDjContentRcmdInfoData
        {
            [JsonPropertyName("tagName")] public string? TagName { get; set; }
            [JsonPropertyName("aiDjResources")] public AiDjContentRcmdInfoResource[]? AiDjResources { get; set; }

            [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
            [JsonDerivedType(typeof(AiDjContentRcmdAudioResource), "audio")]
            [JsonDerivedType(typeof(AiDjContentRcmdAudioSong), "song")]
            public class AiDjContentRcmdInfoResource
            {

            }

            public class AiDjContentRcmdAudioSong : AiDjContentRcmdInfoResource
            {
                [JsonPropertyName("type")] public string? Type { get; set; }
                [JsonPropertyName("value")] public AiDjContentRcmdAudioSongValue? Value { get; set; }

                public class AiDjContentRcmdAudioSongValue
                {
                    [JsonPropertyName("alg")] public string? Algorithm { get; set; }
                    [JsonPropertyName("songId")] public string? SongId { get; set; }
                    [JsonPropertyName("songData")] public SongDto? SongName { get; set; }
                }
            }

            public class AiDjContentRcmdAudioResource : AiDjContentRcmdInfoResource
            {
                [JsonPropertyName("value")] public AiDjContentRcmdAudioResourceValue? Value { get; set; }

                [JsonPropertyName("type")] public string? Type { get; set; }

                public class AiDjContentRcmdAudioResourceValue
                {
                    [JsonPropertyName("audioList")] public AiDjContentRcmdAudioResourceValueAudio[]? Audio { get; set; }

                    public class AiDjContentRcmdAudioResourceValueAudio
                    {
                        [JsonPropertyName("audioUrl")] public string? Url { get; set; }
                        [JsonPropertyName("duration")] public float Duration { get; set; }
                        [JsonPropertyName("fadeInOut")] public bool FadeInOut { get; set; }
                        [JsonPropertyName("audioId")] public string? Id { get; set; }
                        [JsonPropertyName("size")] public long? Size { get; set; }

                        [JsonPropertyName("introductionRelatedSongIds")]
                        public string[]? IntroductionRelatedSongIds { get; set; }

                        [JsonPropertyName("gain")] public float Gain { get; set; }
                    }
                }
            }
        }
    }

    public class AiDjContentRcmdInfoActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("extInfo")] public string? ExtInfo { get; set; }
    }

    class AiDjActualRequestExtInfo
    {
        [JsonPropertyName("lastRequestTimestamp")]
        public long LastRequestTimestamp { get; set; }

        [JsonPropertyName("lbsInfoList")] public LbsInfo[]? LbsInfoList { get; set; }
        [JsonPropertyName("listenedTs")] public bool ListenedTs { get; set; }
        [JsonPropertyName("noAidjToAidj")] public bool NoAidjToAidj { get; set; }

        internal class LbsInfo
        {
            [JsonPropertyName("lat")] public float Latitude { get; set; }
            [JsonPropertyName("lon")] public float Longitude { get; set; }
            [JsonPropertyName("time")] public long Time { get; set; }
        }
    }
}
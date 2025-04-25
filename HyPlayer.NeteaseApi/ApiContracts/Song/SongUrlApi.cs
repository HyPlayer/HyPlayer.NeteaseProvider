using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Song;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        /// <summary>
        /// 歌曲播放链接
        /// </summary>
        public static SongUrlApi SongUrlApi => new();
    }
}


namespace HyPlayer.NeteaseApi.ApiContracts.Song
{
    public class SongUrlApi : EApiContractBase<SongUrlRequest, SongUrlResponse, ErrorResultBase, SongUrlActualRequest>
    {
        public override string IdentifyRoute => "/song/url";

        public override string Url { get; protected set; } =
            "https://interface.music.163.com/eapi/song/enhance/player/url/v1";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest()
        {
            if (Request is null) return Task.CompletedTask;
            var ids = Request.ConvertToIdStringList();
            ActualRequest = new SongUrlActualRequest
            {
                Ids = ids,
                Level = Request.Level
            };
            return Task.CompletedTask;
        }
        
        public override string ApiPath { get; protected set; } = "/api/song/enhance/player/url/v1";
    }

    public class SongUrlActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("ids")] public required string Ids { get; set; }
        [JsonPropertyName("level")] public required string Level { get; set; }
        [JsonPropertyName("encodeType")] public string EncodeType => "flac";
        [JsonPropertyName("immerseType")] public string ImmerseType => "c51";
    }

    public class SongUrlRequest : IdOrIdListListRequest
    {
        /// <summary>
        /// 音质
        /// </summary>
        public required string Level { get; set; }
    }

    public class SongUrlResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public SongUrlItem[]? SongUrls { get; set; }

        public class SongUrlItem
        {
            [JsonPropertyName("code")] public int Code { get; set; }
            [JsonPropertyName("id")] public required string Id { get; set; }
            [JsonPropertyName("url")] public string? Url { get; set; }
            [JsonPropertyName("br")] public string? BitRate { get; set; }
            [JsonPropertyName("size")] public long Size { get; set; }
            [JsonPropertyName("md5")] public string? Md5 { get; set; }
            [JsonPropertyName("type")] public string? Type { get; set; }
            [JsonPropertyName("level")] public string? Level { get; set; }
            [JsonPropertyName("encodeType")] public string? EncodeType { get; set; }
            [JsonPropertyName("time")] public long Time { get; set; }
            [JsonPropertyName("freeTrialInfo")] public FreeTrialInfoData? FreeTrialInfo { get; set; }
            [JsonPropertyName("gain")] public float? Gain { get; set; }



            public class FreeTrialInfoData
            {
                [JsonPropertyName("fragmentType")] public int FragmentType { get; set; }
                [JsonPropertyName("start")] public long Start { get; set; }
                [JsonPropertyName("end")] public long End { get; set; }
            }
        }
    }
}
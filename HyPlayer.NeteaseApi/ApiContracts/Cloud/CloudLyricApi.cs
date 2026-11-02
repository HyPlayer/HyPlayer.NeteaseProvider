using HyPlayer.NeteaseApi.ApiContracts.Cloud;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static CloudLyricApi CloudLyricApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Cloud
{
    public class CloudLyricApi : EApiContractBase<CloudLyricRequest, CloudLyricResponse, ErrorResultBase, CloudLyricActualRequest>
    {
        public override string IdentifyRoute => "/cloud/lyric/get";
        public override string Url { get; protected set; } = "https://interface3.music.163.com/eapi/cloud/lyric/get";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new CloudLyricActualRequest
                {
                    UserId = Request.UserId,
                    SongId = Request.SongId
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/cloud/lyric/get";
    }

    public class CloudLyricRequest : RequestBase
    {
        public required string UserId { get; set; }
        public required string SongId { get; set; }
    }

    public class CloudLyricResponse : CodedResponseBase
    {
        [JsonPropertyName("lrc")] public string? Lyric { get; set; }
    }

    public class CloudLyricActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("userId")] public required string UserId { get; set; }
        [JsonPropertyName("songId")] public required string SongId { get; set; }
        [JsonPropertyName("lv")] public int Lv => -1;
        [JsonPropertyName("kv")] public int Kv => -1;
    }
}

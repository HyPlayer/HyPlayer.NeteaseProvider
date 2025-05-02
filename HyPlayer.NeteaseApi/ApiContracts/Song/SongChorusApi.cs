using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Song;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static SongChorusApi SongChorusApi => new();
    }
}


namespace HyPlayer.NeteaseApi.ApiContracts.Song
{
    public class SongChorusApi : EApiContractBase<SongChorusRequest, SongChorusResponse, ErrorResultBase,
        SongChorusActualRequest>
    {
        public override string IdentifyRoute => "/song/chorus";
        public override string Url { get; protected set; } = "https://interface3.music.163.com/eapi/song/chorus";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest()
        {
            if (Request is not null)
                ActualRequest = new SongChorusActualRequest
                {
                    Ids = Request.ConvertToIdStringList()
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/song/chorus";
    }

    public class SongChorusRequest : IdOrIdListListRequest
    {
    }

    public class SongChorusResponse : CodedResponseBase
    {
        [JsonPropertyName("chorus")] public ChorusData[]? Chorus { get; set; }

        public class ChorusData
        {
            [JsonPropertyName("id")] public string? Id { get; set; }
            [JsonPropertyName("startTime")] public long StartTime { get; set; }
            [JsonPropertyName("endTime")] public long EndTime { get; set; }
        }
    }

    public class SongChorusActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("id")] public required string Ids { get; set; }
    }
}
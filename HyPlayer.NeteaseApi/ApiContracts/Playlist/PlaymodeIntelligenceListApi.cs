using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Playlist;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static PlaymodeIntelligenceListApi PlaymodeIntelligenceListApi => new();
    }
}


namespace HyPlayer.NeteaseApi.ApiContracts.Playlist
{

    public class PlaymodeIntelligenceListApi : EApiContractBase<PlaymodeIntelligenceListRequest,
        PlaymodeIntelligenceListResponse, ErrorResultBase, PlaymodeIntelligenceListActualRequest>
    {
        public override string IdentifyRoute => "/playmode/intelligence/list";

        public override string Url { get; protected set; } =
            "https://interface.music.163.com/eapi/playmode/intelligence/list";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new PlaymodeIntelligenceListActualRequest
                {
                    PlaylistId = Request.PlaylistId,
                    SongId = Request.SongId,
                    StartMusicId = Request.StartMusicId,
                    Count = Request.Count
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/playmode/intelligence/list";
    }

    public class PlaymodeIntelligenceListRequest : RequestBase
    {
        public required string PlaylistId { get; set; }
        public required string SongId { get; set; }
        public required string StartMusicId { get; set; }
        public required int Count { get; set; }
    }

    public class PlaymodeIntelligenceListResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public PlayModeIntelligenceListResponseItem[]? Data { get; set; }

        public class PlayModeIntelligenceListResponseItem
        {
            [JsonPropertyName("id")] public string? Id { get; set; }
            [JsonPropertyName("alg")] public string? Algorithm { get; set; }
            [JsonPropertyName("recommended")] public bool Recommended { get; set; }
            [JsonPropertyName("songInfo")] public EmittedSongDtoWithPrivilege? SongInfo { get; set; }
        }
    }

    public class PlaymodeIntelligenceListActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("playlistId")] public required string PlaylistId { get; set; }
        [JsonPropertyName("songId")] public required string SongId { get; set; }
        [JsonPropertyName("type")] public string Type => "fromPlayAll";
        [JsonPropertyName("startMusicId")] public required string StartMusicId { get; set; }
        [JsonPropertyName("count")] public int Count { get; set; }
    }
}
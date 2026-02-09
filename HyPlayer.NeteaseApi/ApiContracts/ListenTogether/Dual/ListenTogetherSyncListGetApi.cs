using HyPlayer.NeteaseApi.ApiContracts.ListenTogether;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static ListenTogetherSyncListGetApi ListenTogetherSyncListGetApi => new();
    }
}


namespace HyPlayer.NeteaseApi.ApiContracts.ListenTogether
{

    public class ListenTogetherSyncListGetApi : EApiContractBase<ListenTogetherSyncListGetRequest, ListenTogetherSyncListGetResponse, ErrorResultBase, ListenTogetherSyncListGetActualRequest>
    {
        public override string IdentifyRoute => "/listentogether/sync/list/get";
        public override string Url { get; protected set; } = "https://interface3.music.163.com/eapi/listen/together/sync/playlist/get";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new ListenTogetherSyncListGetActualRequest()
                {
                    RoomId = Request.RoomId
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/listen/together/sync/playlist/get";
    }

    public class ListenTogetherSyncListGetRequest : RequestBase
    {
        public required string RoomId { get; set; }
    }

    public class ListenTogetherSyncListGetResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public ListenTogetherSyncListGetResponseData? Data { get; set; }

        public class ListenTogetherSyncListGetResponseData
        {
            [JsonPropertyName("playlist")] public ListenTogetherSyncListGetResponsePlaylist? Playlist { get; set; }
            [JsonPropertyName("playCommand")] public ListenTogetherSyncListGetResponsePlayCommand? PlayCommand { get; set; }


            public class ListenTogetherSyncListGetResponsePlaylist
            {
                [JsonPropertyName("displayList")] public ListenTogetherSyncListGetResponseListInfo? DisplayList { get; set; }
                [JsonPropertyName("randomList")] public ListenTogetherSyncListGetResponseListInfo? RandomList { get; set; }
                [JsonPropertyName("playMode")] public string? PlayMode { get; set; }
                [JsonPropertyName("replace")] public bool Replace { get; set; }
                [JsonPropertyName("version")] public ListenTogetherSyncListGetResponseVersion[]? Version { get; set; }
            }

            public class ListenTogetherSyncListGetResponseListInfo
            {
                [JsonPropertyName("changed")] public required bool Changed { get; set; }
                [JsonPropertyName("result")] public required string[]? Result { get; set; }
                [JsonPropertyName("rcmdSongIds")] public required string[]? RcmdSongIds { get; set; }
            }

            public class ListenTogetherSyncListGetResponseVersion
            {
                [JsonPropertyName("userId")] public string? UserId { get; set; }
                [JsonPropertyName("version")] public int Version { get; set; }
            }

            public class ListenTogetherSyncListGetResponsePlayCommand
            {
                [JsonPropertyName("outerId")] public string? OuterId { get; set; }
                [JsonPropertyName("userId")] public string? UserId { get; set; }
                [JsonPropertyName("commandType")] public string? CommandType { get; set; }
                [JsonPropertyName("formerSongId")] public string? FormerSongId { get; set; }
                [JsonPropertyName("targetSongId")] public string? TargetSongId { get; set; }
                [JsonPropertyName("triggerType")] public string? TriggerType { get; set; }
                [JsonPropertyName("progress")] public int Progress { get; set; }
                [JsonPropertyName("playStatus")] public string? PlayStatus { get; set; }
                [JsonPropertyName("clientSeq")] public int ClientSeq { get; set; }
                [JsonPropertyName("serverSeq")] public long ServerSeq { get; set; }
                [JsonPropertyName("anotherUid")] public long AnotherUid { get; set; }
                [JsonPropertyName("anotherOuterId")] public string? AnotherOuterId { get; set; }
            }
        }


    }

    public class ListenTogetherSyncListGetActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("roomId")] public required string RoomId { get; set; }
    }
}
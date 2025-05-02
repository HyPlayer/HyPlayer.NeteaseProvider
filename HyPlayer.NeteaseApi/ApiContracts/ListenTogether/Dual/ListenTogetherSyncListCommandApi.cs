using System.Text.Json;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Category;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static ListenTogetherSyncListReportApi ListenTogetherSyncListReportApi => new();
    }
}


namespace HyPlayer.NeteaseApi.ApiContracts.Category
{
    public class ListenTogetherSyncListReportApi : EApiContractBase<ListenTogetherSyncListReportRequest,
        ListenTogetherSyncListReportResponse, ErrorResultBase, ListenTogetherSyncListReportActualRequest>
    {
        public override string IdentifyRoute => "/listentogether/sync/list/command";

        public override string Url { get; protected set; } =
            "https://interface3.music.163.com/eapi/listen/together/sync/list/command/report";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest()
        {
            if (Request is not null)
            {
                var playlistParam =
                    new ListenTogetherSyncListReportActualRequest.
                        ListenTogetherSyncListReportActualRequestPlaylistParam()
                        {
                            AnchorPosition = Request.AnchorPosition,
                            AnchorSongId = Request.AnchorSongId,
                            ClientSeq = Request.ClientSeq,
                            CommandType = Request.CommandType switch
                            {
                                ListenTogetherSyncListReportRequest.ListenTogetherSyncListReportCommandType.Replace =>
                                    "REPLACE",
                                ListenTogetherSyncListReportRequest.ListenTogetherSyncListReportCommandType.PlayModeChange =>
                                    "PLAYMODE_CHANGE",
                                _ => throw new ArgumentOutOfRangeException()
                            },
                            DisplayList = Request.DisplaySongList,
                            RandomList = Request.RandomSongList,
                            PlayMode = Request.PlayMode switch
                            {
                                ListenTogetherSyncListReportRequest.ListenTogetherSyncListReportPlayMode.OrderLoop =>
                                    "ORDER_LOOP",
                                ListenTogetherSyncListReportRequest.ListenTogetherSyncListReportPlayMode.Random =>
                                    "RANDOM",
                                ListenTogetherSyncListReportRequest.ListenTogetherSyncListReportPlayMode.SingleLoop =>
                                    "SINGLE_LOOP",
                                _ => throw new ArgumentOutOfRangeException()
                            },
                            Version = [
                                new ListenTogetherSyncListReportActualRequest.ListenTogetherSyncListReportActualRequestPlaylistParam.ListenTogetherSyncListReportActualRequestVersion()
                                {
                                    UserId = long.Parse(Request.UserId),
                                    Version = Request.ClientSeq
                                }
                            ]
                        };
                var playlistParamJson = JsonSerializer.Serialize(playlistParam, new JsonSerializerOptions(JsonSerializerDefaults.Web)
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });
                ActualRequest = new ListenTogetherSyncListReportActualRequest()
                {
                    ClientSeq = Request.ClientSeq.ToString(),
                    PlaylistParam = playlistParamJson,
                    RoomId = Request.RoomId
                };
            }

            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/listen/together/sync/list/command/report";
    }

    public class ListenTogetherSyncListReportRequest : RequestBase
    {
        public required string RoomId { get; set; }
        public required ListenTogetherSyncListReportCommandType CommandType { get; set; }
        public required ListenTogetherSyncListReportPlayMode PlayMode { get; set; }
        public required string UserId { get; set; }
        public required int ClientSeq { get; set; }
        public int AnchorPosition { get; set; } = -1;
        public string AnchorSongId { get; set; } = "";
        public required string[] DisplaySongList { get; set; }
        public string[]? RandomSongList { get; set; } = null;

        public enum ListenTogetherSyncListReportCommandType
        {
            Replace,
            PlayModeChange
        }

        public enum ListenTogetherSyncListReportPlayMode
        {
            OrderLoop,
            Random,
            SingleLoop
        }
    }

    public class ListenTogetherSyncListReportResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public ListenTogetherSyncListReportResponseData? Data { get; set; }

        public class ListenTogetherSyncListReportResponseData
        {
            [JsonPropertyName("result")] public required bool Result { get; set; }
        }
    }

    public class ListenTogetherSyncListReportActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("clientSeq")] public required string ClientSeq { get; set; }
        [JsonPropertyName("playlistParam")] public required string PlaylistParam { get; set; }
        [JsonPropertyName("roomId")] public required string RoomId { get; set; }

        public class ListenTogetherSyncListReportActualRequestPlaylistParam
        {
            [JsonPropertyName("anchorPosition")] public int AnchorPosition { get; set; } = -1;
            [JsonPropertyName("anchorSongId")] public string AnchorSongId { get; set; } = "";
            [JsonPropertyName("clientSeq")] public required int ClientSeq { get; set; }
            [JsonPropertyName("commandType")] public required string CommandType { get; set; }
            [JsonPropertyName("displayList")] public required string[] DisplayList { get; set; }
            [JsonPropertyName("randomList")] public string[]? RandomList { get; set; } = null;
            [JsonPropertyName("playMode")] public required string PlayMode { get; set; }

            [JsonPropertyName("version")]
            public required ListenTogetherSyncListReportActualRequestVersion[] Version { get; set; }


            public class ListenTogetherSyncListReportActualRequestVersion
            {
                [JsonPropertyName("userId")] public long UserId { get; set; }
                [JsonPropertyName("version")] public int Version { get; set; }
            }
        }
    }
}
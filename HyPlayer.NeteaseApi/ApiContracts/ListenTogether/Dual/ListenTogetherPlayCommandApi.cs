using System.Text.Json;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Category;
using HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Dual;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static ListenTogetherPlayCommandApi ListenTogetherPlayCommandApi => new();
    }
}


namespace HyPlayer.NeteaseApi.ApiContracts.Category
{
    public class ListenTogetherPlayCommandApi : EApiContractBase<ListenTogetherPlayCommandRequest,
        ListenTogetherPlayCommandResponse, ErrorResultBase, ListenTogetherPlayCommandActualRequest>
    {
        public override string IdentifyRoute => "/listentogether/play/command";

        public override string Url { get; protected set; } =
            "https://interface.music.163.com/eapi/listen/together/play/command/report";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new ListenTogetherPlayCommandActualRequest
                {
                    RoomId = Request.RoomId,
                    CommandInfo = JsonSerializer.Serialize(
                        new ListenTogetherPlayCommandActualRequest.ListenTogetherPlayCommandActualRequestCommandInfo
                        {
                            CommandType = Request.CommandType switch
                            {
                                ListenTogetherPlayCommandRequest.ListenTogetherPlayCommandRequestCommandType
                                    .Pause => "PAUSE",
                                ListenTogetherPlayCommandRequest.ListenTogetherPlayCommandRequestCommandType
                                    .Play => "PLAY",
                                ListenTogetherPlayCommandRequest.ListenTogetherPlayCommandRequestCommandType
                                    .Previous => "PREV",
                                ListenTogetherPlayCommandRequest.ListenTogetherPlayCommandRequestCommandType
                                    .Next => "NEXT",
                                ListenTogetherPlayCommandRequest.ListenTogetherPlayCommandRequestCommandType
                                    .Goto => "GOTO",
                                ListenTogetherPlayCommandRequest.ListenTogetherPlayCommandRequestCommandType
                                    .Progress => "PROGRESS",
                                _ => throw new ArgumentOutOfRangeException()
                            },
                            Progress = Request.Progress,
                            PlayStatus = Request.PlayStatus,
                            FormerSongId = Request.FormerSongId,
                            TargetSongId = Request.TargetSongId,
                            ClientSeq = Request.ClientSeq
                        })
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/listen/together/play/command/report";
    }

    public class ListenTogetherPlayCommandRequest : RequestBase
    {
        public required string RoomId { get; set; }
        public required ListenTogetherPlayCommandRequestCommandType CommandType { get; set; }
        public required long Progress { get; set; } = 0;
        public required ListenTogetherHeartBeatRequest.ListenTogetherPlayStatus PlayStatus { get; set; }
        public required string FormerSongId { get; set; }
        public required string TargetSongId { get; set; }
        public required int ClientSeq { get; set; }

        public enum ListenTogetherPlayCommandRequestCommandType
        {
            Pause,
            Play,
            Previous,
            Next,
            Goto,
            Progress,
        }
    }

    public class ListenTogetherPlayCommandResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public ListenTogetherPlayCommandResponseData? Data { get; set; }

        public class ListenTogetherPlayCommandResponseData
        {
            [JsonPropertyName("result")] public required bool Result { get; set; }
        }
    }

    public class ListenTogetherPlayCommandActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("roomId")] public required string RoomId { get; set; }
        [JsonPropertyName("commandInfo")] public required string CommandInfo { get; set; } = string.Empty;

        public class ListenTogetherPlayCommandActualRequestCommandInfo
        {
            [JsonPropertyName("commandType")] public required string CommandType { get; set; }
            [JsonPropertyName("progress")] public required long Progress { get; set; } = 0;

            [JsonPropertyName("playStatus")]
            public required ListenTogetherHeartBeatRequest.ListenTogetherPlayStatus PlayStatus { get; set; }

            [JsonPropertyName("formerSongId")] public required string FormerSongId { get; set; }
            [JsonPropertyName("targetSongId")] public required string TargetSongId { get; set; }
            [JsonPropertyName("clientSeq")] public required int ClientSeq { get; set; }
        }
    }
}
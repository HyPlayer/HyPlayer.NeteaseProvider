using HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Multi;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static MultiListenTogetherSongOperateApi MultiListenTogetherSongOperateApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Multi
{
    public class MultiListenTogetherSongOperateApi : DesktopEApiContractBase<MultiListenTogetherSongOperateRequest,
        MultiListenTogetherSongOperateResponse, ErrorResultBase, MultiListenTogetherSongOperateActualRequest>
    {
        public override string IdentifyRoute => "/listentogether/multi/match/song/operate";

        public override string Url { get; protected set; } =
            "https://interface3.music.163.com/eapi/listen/together/multi/match/song/operate";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new MultiListenTogetherSongOperateActualRequest
                {
                    RoomId = Request.RoomId,
                    SongId = Request.SongId,
                    BizId = Request.BizId,
                    Operate = Request.Operate switch
                    {
                        MultiListenTogetherSongOperation.Switch => "4",
                        _ => "1"
                    },
                    CheckToken = Request.CheckToken
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/listen/together/multi/match/song/operate";
    }

    public enum MultiListenTogetherSongOperation
    {
        Add,
        Switch
    }

    public class MultiListenTogetherSongOperateRequest : RequestBase
    {
        public required string RoomId { get; set; }
        public required string SongId { get; set; }
        public required string BizId { get; set; }
        public required MultiListenTogetherSongOperation Operate { get; set; }
        public required string CheckToken { get; set; }
    }

    public class MultiListenTogetherSongOperateResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public MultiListenTogetherSongOperateResponseData? Data { get; set; }

        public class MultiListenTogetherSongOperateResponseData
        {
            [JsonPropertyName("result")] public bool? Result { get; set; }
            [JsonPropertyName("failedMsg")] public string? Message { get; set; }
            [JsonPropertyName("failedCode")] public int? FailedCode { get; set; }
            [JsonPropertyName("roomSongInfo")] public MultiListenTogetherPlayInfo? PlayInfo { get; set; }
        }
    }

    public class MultiListenTogetherSongOperateActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("roomId")] public required string RoomId { get; set; }
        [JsonPropertyName("songId")] public required string SongId { get; set; }
        [JsonPropertyName("bizId")] public required string BizId { get; set; }
        [JsonPropertyName("operate")] public required string Operate { get; set; }
        [JsonPropertyName("checkToken")] public required string CheckToken { get; set; }
    }
}

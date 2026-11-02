using HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Multi;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static MultiListenTogetherRoomCreateApi MultiListenTogetherRoomCreateApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Multi
{
    public class MultiListenTogetherRoomCreateApi : DesktopEApiContractBase<MultiListenTogetherRoomCreateRequest,
        MultiListenTogetherRoomCreateResponse, ErrorResultBase, MultiListenTogetherRoomCreateActualRequest>
    {
        public override string IdentifyRoute => "/listentogether/multi/room/create";

        public override string Url { get; protected set; } =
            "https://interface3.music.163.com/eapi/listen/together/multi/room/create";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new MultiListenTogetherRoomCreateActualRequest
                {
                    SongId = Request.SongId,
                    CheckToken = Request.CheckToken
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/listen/together/multi/room/create";
    }

    public class MultiListenTogetherRoomCreateRequest : RequestBase
    {
        public required string SongId { get; set; }
        public required string CheckToken { get; set; }
    }

    public class MultiListenTogetherRoomCreateResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public MultiListenTogetherRoomCreateResponseData? Data { get; set; }

        public class MultiListenTogetherRoomCreateResponseData
        {
            [JsonPropertyName("success")] public bool? Success { get; set; }
            [JsonPropertyName("failedMessage")] public string? FailedMessage { get; set; }
            [JsonPropertyName("existRoomId")] public string? ExistRoomId { get; set; }
            [JsonPropertyName("multiLtRoomSnapshot")] public MultiListenTogetherSnapshot? Snapshot { get; set; }
            [JsonPropertyName("multiRoomPermission")] public MultiListenTogetherPermission? Permission { get; set; }
        }
    }

    public class MultiListenTogetherRoomCreateActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("type")] public string Type => "1";
        [JsonPropertyName("songId")] public required string SongId { get; set; }
        [JsonPropertyName("groupIds")] public string GroupIds => "[]";
        [JsonPropertyName("inviteUids")] public string InviteUids => "[]";
        [JsonPropertyName("checkToken")] public required string CheckToken { get; set; }
    }
}

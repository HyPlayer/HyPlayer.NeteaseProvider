using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.ListenTogether;
using HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Dual;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static ListenTogetherInvitationAcceptApi ListenTogetherInvitationAcceptApi => new();
    }
}


namespace HyPlayer.NeteaseApi.ApiContracts.ListenTogether
{
    public class ListenTogetherInvitationAcceptApi : EApiContractBase<ListenTogetherInvitationAcceptRequest,
        ListenTogetherInvitationAcceptResponse, ErrorResultBase, ListenTogetherInvitationAcceptActualRequest>
    {
        public override string IdentifyRoute => "/listentogether/invitation/accept";

        public override string Url { get; protected set; } =
            "https://interface3.music.163.com/eapi/listen/together/play/invitation/accept";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/listen/together/play/invitation/accept";
    }

    public class ListenTogetherInvitationAcceptRequest : RequestBase
    {
        public required string RoomId { get; set; }
        public required string InviterId { get; set; }
        public string Refer { get; set; } = "third_party_invite";
    }

    public class ListenTogetherInvitationAcceptResponse : CodedResponseBase
    {
        [JsonPropertyName("data")]
        public ListenTogetherRoomCreateResponse.ListenTogetherRoomCreateResponseData? Data { get; set; }
    }

    public class ListenTogetherInvitationAcceptActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("roomId")] public required string RoomId { get; set; }
        [JsonPropertyName("inviterId")] public required string InviterId { get; set; }
        [JsonPropertyName("refer")] public string Refer { get; set; } = "third_party_invite";
    }
}
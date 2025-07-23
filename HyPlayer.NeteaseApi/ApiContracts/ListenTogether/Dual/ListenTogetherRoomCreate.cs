using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Dual;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static ListenTogetherRoomCreateApi ListenTogetherRoomCreateApi => new();
    }
}


namespace HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Dual
{

    public class ListenTogetherRoomCreateApi : EApiContractBase<ListenTogetherRoomCreateRequest, ListenTogetherRoomCreateResponse, ErrorResultBase, ListenTogetherRoomCreateActualRequest>
    {
        public override string IdentifyRoute => "/listentogether/room/create";
        public override string Url { get; protected set; } = "https://interface3.music.163.com/eapi/listen/together/room/create";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/listen/together/room/create";
    }

    public class ListenTogetherRoomCreateRequest : RequestBase
    {
        
    }

    
    public class ListenTogetherRoomCreateResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public ListenTogetherRoomCreateResponseData? Data { get; set; }

        public class ListenTogetherRoomCreateResponseData
        {
            [JsonPropertyName("type")] public string? Type { get; set; }

            [JsonPropertyName("roomInfo")] public ListenTogetherRoomInfo? RoomInfo { get; set; }
        }
        
        public class ListenTogetherRoomInfo
        {
            [JsonPropertyName("creatorId")] public string? CreatorId { get; set; }
            [JsonPropertyName("roomId")] public string? RoomId { get; set; }
            [JsonPropertyName("effectiveDurationMs")] public long EffectiveDurationMs { get; set; }
            [JsonPropertyName("waitMs")] public long WaitMs { get; set; }
            [JsonPropertyName("roomCreateTime")] public long RoomCreateTime { get; set; }
            [JsonPropertyName("chatRoomId")] public string? ChatRoomId { get; set; }
            [JsonPropertyName("agoraChannelId")] public string? AgoraChannelId { get; set; }
            
            [JsonPropertyName("roomUsers")] public List<RoomUser>? RoomUsers { get; set; }
            [JsonPropertyName("roomType")] public string? RoomType { get; set; }
            [JsonPropertyName("matchedReason")] public string? MatchedReason { get; set; }
            [JsonPropertyName("alg")] public string? Alg { get; set; }
            

            public class RoomUser
            {
                [JsonPropertyName("userId")] public string? UserId { get; set; }
                [JsonPropertyName("nickname")] public string? Nickname { get; set; }
                [JsonPropertyName("avatarUrl")] public string? AvatarUrl { get; set; }
            }
        }
    }

    public class ListenTogetherRoomCreateActualRequest : EApiActualRequestBase
    {

    }
}
using HyPlayer.NeteaseApi.ApiContracts.User;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static UserRecordApi UserRecordApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.User
{

    public class UserRecordApi : EApiContractBase<UserRecordRequest, UserRecordResponse, ErrorResultBase,
        UserRecordActualRequest>
    {
        public override string IdentifyRoute => "/user/record";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/v1/play/record";
        public override HttpMethod Method => HttpMethod.Post;
        public override string ApiPath { get; protected set; } = "/api/v1/play/record";

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new UserRecordActualRequest
                {
                    UserId = Request.UserId,
                    RecordType = Request.RecordType,
                    Offset = Request.Offset,
                    Count = Request.Count
                };
            return Task.CompletedTask;
        }
    }

    public class UserRecordRequest : RequestBase
    {
        public required string UserId { get; set; }
        public UserRecordType RecordType { get; set; } = UserRecordType.All;
        public int Offset { get; set; } = 0;
        public int Count { get; set; } = 120;
    }

    public enum UserRecordType
    {
        All = 0,
        WeekData = 1
    }

    [JsonContextSerializable(typeof(UserRecordWeekResponse))]
    public class UserRecordWeekResponse : UserRecordResponse
    {
        [JsonPropertyName("weekData")] public UserRecordResponseItem[]? WeekData { get; set; }
    }

    [JsonContextSerializable(typeof(UserRecordAllResponse))]
    public class UserRecordAllResponse : UserRecordResponse
    {
        [JsonPropertyName("allData")] public UserRecordResponseItem[]? AllData { get; set; }
    }

    public class UserRecordResponse : CodedResponseBase
    {

    }

    public class UserRecordResponseItem
    {
        [JsonPropertyName("song")] public EmittedSongDtoWithPrivilege? Song { get; set; }
        [JsonPropertyName("playCount")] public long PlayCount { get; set; }
        [JsonPropertyName("score")] public int Score { get; set; }
    }

    public class UserRecordActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("uid")] public required string UserId { get; set; }
        [JsonPropertyName("type")] public UserRecordType RecordType { get; set; } = UserRecordType.All;
        [JsonPropertyName("offset")] public int Offset { get; set; } = 0;
        [JsonPropertyName("limit")] public int Count { get; set; } = 120;
        [JsonPropertyName("total")] public bool Total => true;
    }
}
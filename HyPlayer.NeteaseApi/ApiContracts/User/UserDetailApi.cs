using HyPlayer.NeteaseApi.ApiContracts.User;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static UserDetailApi UserDetailApi => new();
    }
}


namespace HyPlayer.NeteaseApi.ApiContracts.User
{
    public class UserDetailApi : EApiContractBase<UserDetailRequest, UserDetailResponse, ErrorResultBase,
        UserDetailActualRequest>
    {
        public override string IdentifyRoute => "/user/detail";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/w/v1/user/detail/";
        public override string ApiPath { get; protected set; } = "/api/w/v1/user/detail/";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
            {
                ActualRequest = new UserDetailActualRequest
                {
                    UserId = Request.UserId
                };
                Url = $"https://interface.music.163.com/eapi/w/v1/user/detail/{Request.UserId}";
                ApiPath = $"/api/w/v1/user/detail/{Request.UserId}";
            }

            return Task.CompletedTask;
        }


    }

    public class UserDetailRequest : RequestBase
    {
        public required string UserId { get; set; }
    }

    public class UserDetailResponse : CodedResponseBase
    {
        [JsonPropertyName("level")] public int Level { get; set; }
        [JsonPropertyName("listenSongs")] public long ListenSongs { get; set; }
        [JsonPropertyName("profile")] public UserInfoDto? Profile { get; set; }
        [JsonPropertyName("createTime")] public long CreateTime { get; set; }
        [JsonPropertyName("createDays")] public int CreateDays { get; set; }
    }

    public class UserDetailActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("userId")] public required string UserId { get; set; }
        [JsonPropertyName("all")] public bool All => true;
    }
}
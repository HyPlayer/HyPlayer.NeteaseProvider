using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using HyPlayer.NeteaseApi.ApiContracts.User;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static UserUnfollowApi UserUnfollowApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.User
{
    public class UserUnfollowApi : EApiContractBase<UserUnfollowRequest, UserUnfollowResponse, ErrorResultBase, UserUnfollowActualRequestBase>, IFakeCheckTokenApi
    {
        public override string ApiPath { get; protected set; } = "/api/user/delfollow/";

        public override string IdentifyRoute => "/user/delfollow";

        public override string Url { get; protected set; } = "https://interfacepc.music.163.com/eapi/user/delfollow/";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            ActualRequest = new UserUnfollowActualRequestBase();
            if (Request is not null)
            {
                Url += Request.Id;
                ApiPath += Request.Id;
            }

            return Task.CompletedTask;
        }
    }

    public class UserUnfollowActualRequestBase : EApiActualRequestBase
    {
    }

    public class UserUnfollowResponse : CodedResponseBase
    {
    }

    public class UserUnfollowRequest : RequestBase
    {
        public required string Id { get; set; }
    }
}

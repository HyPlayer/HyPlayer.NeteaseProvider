using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using HyPlayer.NeteaseApi.ApiContracts.User;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static UserFollowApi UserFollowApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.User
{
    public class UserFollowApi : EApiContractBase<UserFollowRequest, UserFollowResponse, ErrorResultBase, UserFollowActualRequestBase>
    {
        public override string ApiPath { get; protected set; } = "/user/follow";

        public override string IdentifyRoute => "/user/follow";

        public override string Url { get; protected set; } = "https://interfacepc.music.163.com/eapi/user/";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            ActualRequest = new UserFollowActualRequestBase();

            Url += Request.IsFollow ? "follow/" : "delfollow/";
            Url += Request.Id;

            return Task.CompletedTask;
        }
    }

    public class UserFollowActualRequestBase : EApiActualRequestBase
    {
    }

    public class UserFollowResponse : CodedResponseBase
    {
    }

    public class UserFollowRequest : RequestBase
    {
        public required string Id { get; set; }

        public bool IsFollow { get; set; }
    }
}

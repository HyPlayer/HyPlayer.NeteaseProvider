using HyPlayer.NeteaseApi.ApiContracts.Artist;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.WeApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        /// <summary>
        /// 艺术家收藏
        /// </summary>
        public static ArtistSubscribeApi ArtistSubscribeApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Artist
{
    public class ArtistSubscribeApi : WeApiContractBase<ArtistSubscribeRequest, ArtistSubscribeResponse, ErrorResultBase,
        ArtistSubscribeActualRequest>
    {
        public override string IdentifyRoute => "/subscribeartist";

        public override string Url { get; protected set; } = "https://music.163.com/weapi/artist/sub";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            throw new NotImplementedException();
        }
    }

    public class ArtistSubscribeRequest : RequestBase
    {
    }

    public class ArtistSubscribeResponse : CodedResponseBase
    {
    }

    public class ArtistSubscribeActualRequest : WeApiActualRequestBase
    {
    }
}

using HyPlayer.NeteaseApi.ApiContracts.Artist;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using System.Text.Json.Serialization;

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
    public class ArtistSubscribeApi : EApiContractBase<ArtistSubscribeRequest, ArtistSubscribeResponse, ErrorResultBase,
        ArtistSubscribeActualRequest>, IFakeCheckTokenApi
    {
        public override string ApiPath { get; protected set; } = "/api/artist/sub";

        public override string IdentifyRoute => "/artist/subscribe";

        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/artist/sub";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
            {
                ActualRequest = new ArtistSubscribeActualRequest
                {
                    ArtistId = Request.ArtistId
                };
            }
            return Task.CompletedTask;
        }
    }

    public class ArtistSubscribeRequest : RequestBase
    {
        public required string ArtistId { get; set; }
    }

    public class ArtistSubscribeResponse : CodedResponseBase
    {
    }

    public class ArtistSubscribeActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("artistId")] public required string ArtistId { get; set; }
    }
}

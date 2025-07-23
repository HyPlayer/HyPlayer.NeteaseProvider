using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Album;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static AlbumSubscribeApi AlbumSubscribeApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Album
{

    public class AlbumSubscribeApi : EApiContractBase<AlbumSubscribeRequest, AlbumSubscribeResponse, ErrorResultBase,
        AlbumSubscribeActualRequest>
    {
        public override string IdentifyRoute => "/album/subscribe";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/album/";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
            {
                ActualRequest = new AlbumSubscribeActualRequest
                {
                    Id = Request.Id
                };
                Url += Request.IsSubscribe ? "sub" : "unsub";
            }

            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/album/sub";
    }

    public class AlbumSubscribeRequest : RequestBase
    {
        public required string Id { get; set; }
        public bool IsSubscribe { get; set; }
    }

    public class AlbumSubscribeResponse : CodedResponseBase
    {
    }

    public class AlbumSubscribeActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("id")] public required string Id { get; set; }
    }
}
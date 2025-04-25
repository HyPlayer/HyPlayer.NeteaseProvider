using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Album;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static AlbumSubscribeApi AlbumSubscribeApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Album
{

    public class AlbumSubscribeApi : WeApiContractBase<AlbumSubscribeRequest, AlbumSubscribeResponse, ErrorResultBase,
        AlbumSubscribeActualRequest>
    {
        public override string IdentifyRoute => "/album/subscribe";
        public override string Url { get; protected set; } = "https://music.163.com/api/album/";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest()
        {
            if (Request is not null)
            {
                ActualRequest = new AlbumSubscribeActualRequest
                {
                    Id = Request.Id
                };
                Url += Request.IsSubscribe is true ? "sub" : "unsub";
            }

            return Task.CompletedTask;
        }
    }

    public class AlbumSubscribeRequest : RequestBase
    {
        public required string Id { get; set; }
        public bool IsSubscribe { get; set; }
    }

    public class AlbumSubscribeResponse : CodedResponseBase
    {
    }

    public class AlbumSubscribeActualRequest : WeApiActualRequestBase
    {
        [JsonPropertyName("id")] public required string Id { get; set; }
    }
}
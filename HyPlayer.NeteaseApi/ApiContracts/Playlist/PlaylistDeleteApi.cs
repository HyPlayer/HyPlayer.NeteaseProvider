using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Playlist;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static PlaylistDeleteApi PlaylistDeleteApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Playlist
{

    public class PlaylistDeleteApi : WeApiContractBase<PlaylistDeleteRequest, PlaylistDeleteResponse, ErrorResultBase,
        PlaylistDeleteActualRequest>
    {
        public override string IdentifyRoute => "/playlist/delete";
        public override string Url { get; protected set; } = "https://music.163.com/weapi/playlist/remove";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest()
        {
            if (Request is not null)
                ActualRequest = new PlaylistDeleteActualRequest
                {
                    Ids = Request.ConvertToIdStringList()
                };
            return Task.CompletedTask;
        }
    }

    public class PlaylistDeleteRequest : IdOrIdListListRequest
    {

    }

    public class PlaylistDeleteResponse : CodedResponseBase
    {

    }

    public class PlaylistDeleteActualRequest : WeApiActualRequestBase
    {
        [JsonPropertyName("ids")] public required string Ids { get; set; }
    }
}
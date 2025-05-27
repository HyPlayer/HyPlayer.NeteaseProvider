using HyPlayer.NeteaseApi.ApiContracts.Playlist;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static PlaylistPrivacyApi PlaylistPrivacyApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Playlist
{

    public class PlaylistPrivacyApi : EApiContractBase<PlaylistPrivacyRequest, PlaylistPrivacyResponse, ErrorResultBase,
        PlaylistPrivacyActualRequest>
    {

        public override string IdentifyRoute => "/playlist/privacy";

        public override string Url { get; protected set; } =
            "https://interface.music.163.com/eapi/playlist/update/privacy";

        public override string ApiPath { get; protected set; } = "/api/playlist/update/privacy";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new PlaylistPrivacyActualRequest
                {
                    Id = Request.Id
                };
            return Task.CompletedTask;
        }
    }

    public class PlaylistPrivacyRequest : RequestBase
    {
        public required string Id { get; set; }
    }

    public class PlaylistPrivacyResponse : CodedResponseBase
    {

    }

    public class PlaylistPrivacyActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("id")] public required string Id { get; set; }
        [JsonPropertyName("privacy")] public int Privacy => 0;
    }
}
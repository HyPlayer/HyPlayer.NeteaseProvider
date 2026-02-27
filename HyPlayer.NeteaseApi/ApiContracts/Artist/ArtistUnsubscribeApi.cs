using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using System.Text.Json;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Artist;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static ArtistUnsubscribeApi ArtistUnsubscribeApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Artist
{
    public class ArtistUnsubscribeApi : EApiContractBase<ArtistUnsubscribeRequest, ArtistUnsubscribeResponse, ErrorResultBase,
        ArtistUnsubscribeActualRequest>
    {
        public override string ApiPath { get; protected set; } = "/api/artist/unsub";

        public override string IdentifyRoute => "/artist/unsub";

        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/artist/";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
            {
                ActualRequest = new ArtistUnsubscribeActualRequest
                {
                    // serialize C# array of ids to JSON array string expected by the API
                    ArtistIds = JsonSerializer.Serialize(Request.ArtistIds)
                };
                Url += "unsub";
            }
            return Task.CompletedTask;
        }
    }

    public class ArtistUnsubscribeRequest : RequestBase
    {
        /// <summary>
        /// Array of artist ids. Will be converted to a JSON array string in the request, e.g. new long[] { 35374786 } -> "[35374786]"
        /// </summary>
        public required long[] ArtistIds { get; set; }
    }

    public class ArtistUnsubscribeResponse : CodedResponseBase
    {
    }

    public class ArtistUnsubscribeActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("artistIds")] public required string ArtistIds { get; set; }
    }
}

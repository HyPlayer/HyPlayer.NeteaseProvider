using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static ArtistSublistApi ArtistSublistApi => new();
}

public class ArtistSublistApi : WeApiContractBase<ArtistSublistRequest, ArtistSublistResponse, ErrorResultBase, ArtistSublistActualRequest>
{
    public override string IdentifyRoute => "/artist/sublist";
    public override string Url { get; protected set; } = "https://music.163.com/weapi/artist/sublist";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest()
    {
        if (Request is not null)
            ActualRequest = new ArtistSublistActualRequest
            {
                Limit = Request.Limit,
                Offset = Request.Offset
            };
        return Task.CompletedTask;
    }
}

public class ArtistSublistRequest : RequestBase
{
    public int Limit { get; set; } = 25;
    public int Offset { get; set; } = 0;
}

public class ArtistSublistResponse : CodedResponseBase
{
    [JsonPropertyName("count")] public int Count { get; set; }
    [JsonPropertyName("hasMore")] public bool HasMore { get; set; }
    [JsonPropertyName("data")] public ArtistDto[]? Artists { get; set; }
}

public class ArtistSublistActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("limit")] public int Limit { get; set; }
    [JsonPropertyName("offset")] public int Offset { get; set; }
    [JsonPropertyName("total")] public bool Total => true;
}
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static ArtistDetailApi ArtistDetailApi => new();
}

public class ArtistDetailApi : WeApiContractBase<ArtistDetailRequest, ArtistDetailResponse, ErrorResultBase, ArtistDetailActualRequest>
{
    public override string IdentifyRoute => "/artist/detail";
    public override string Url { get; protected set; } = "https://music.163.com/api/artist/head/info/get";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest()
    {
        if (Request is not null)
            ActualRequest = new ArtistDetailActualRequest
            {
                ArtistId = Request.ArtistId
            };
        return Task.CompletedTask;
    }
}

public class ArtistDetailRequest : RequestBase
{
    public required string ArtistId { get; set; }
}

public class ArtistDetailResponse : CodedResponseBase
{
    [JsonPropertyName("data")] public ArtistDetailResponseData? Data { get; set; }

    public class ArtistDetailResponseData
    {
        [JsonPropertyName("videoCount")] public long VideoCount { get; set; }
        [JsonPropertyName("artist")] public ArtistDto? Artist { get; set; }
        [JsonPropertyName("blacklist")] public bool Blacklist { get; set; }
        [JsonPropertyName("preferShow")] public int PreferShow { get; set; }
    }
}

public class ArtistDetailActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("id")] public required string ArtistId { get; set; }
}
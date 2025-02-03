using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static DjChannelProgramsApi DjChannelProgramsApi => new();
}

public class DjChannelProgramsApi : WeApiContractBase<DjChannelProgramsRequest, DjChannelProgramsResponse,
    ErrorResultBase, DjChannelProgramsActualRequest>
{
    public override string IdentifyRoute => "/dj/program";
    public override string Url => "https://music.163.com/weapi/dj/program/byradio";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest(DjChannelProgramsRequest? request)
    {
        if (request is not null)
            ActualRequest = new DjChannelProgramsActualRequest
            {
                RadioId = request.RadioId,
                Limit = request.Limit,
                Offset = request.Offset,
                Asc = request.Asc
            };
        return Task.CompletedTask;
    }
}

public class DjChannelProgramsRequest : RequestBase
{
    public required string RadioId { get; set; }
    public int Limit { get; set; } = 30;
    public int Offset { get; set; } = 0;
    public bool Asc { get; set; } = false;
}

public class DjChannelProgramsResponse : CodedResponseBase
{
    [JsonPropertyName("count")] public int Count { get; set; }
    [JsonPropertyName("programs")] public DjRadioProgramDto[]? Programs { get; set; }
}

public class DjChannelProgramsActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("radioId")] public required string RadioId { get; set; }
    [JsonPropertyName("limit")] public int Limit { get; set; } = 30;
    [JsonPropertyName("offset")] public int Offset { get; set; } = 0;
    [JsonPropertyName("asc")] public bool Asc { get; set; } = false;
}
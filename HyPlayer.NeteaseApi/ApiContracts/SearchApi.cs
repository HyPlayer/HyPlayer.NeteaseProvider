using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static SearchApi SearchApi = new();
}

public class SearchApi : EApiContractBase<SearchRequest, SearchResponse, ErrorResultBase, SearchActualRequest>
{
    public override string Url => "https://interface.music.163.com/eapi/cloudsearch/pc";
    public override HttpMethod Method => HttpMethod.Post;

    public override async Task MapRequest(SearchRequest? request)
    {
        throw new NotImplementedException();
    }

    public override string ApiPath => "/api/cloudsearch/pc";
}

public class SearchRequest : RequestBase
{
    public required string Keyword { get; set; }
    public int Type { get; set; } = 1;
    public int Limit { get; set; } = 30;
    public int Offset { get; set; } = 0;
}

public class SearchResponse : CodedResponseBase
{
    
}

public class SearchActualRequest : EApiActualRequestBase
{
    [JsonPropertyName("s")] public required string Keyword { get; set; }
    [JsonPropertyName("type")] public int Type { get; set; }
    [JsonPropertyName("limit")] public int Limit { get; set; } = 30;
    [JsonPropertyName("offset")] public int Offset { get; set; } = 0;
    [JsonPropertyName("total")] public bool Total  => true;
}
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static SearchSuggestionApi SearchSuggestionApi => new();
}

public class SearchSuggestionApi : WeApiContractBase<SearchSuggestionRequest, SearchSuggestionResponse, ErrorResultBase, SearchSuggestionActualRequest>
{
    public override string IdentifyRoute => "/search/suggest";
    public override string Url => "https://music.163.com/weapi/search/suggest/keyword";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest(SearchSuggestionRequest? request)
    {
        if (request is not null)
            ActualRequest = new SearchSuggestionActualRequest
            {
                Keyword = request.Keyword
            };
        return Task.CompletedTask;
    }
}

public class SearchSuggestionRequest : RequestBase
{
    public required string Keyword { get; set; }
}

public class SearchSuggestionResponse : CodedResponseBase
{
    [JsonPropertyName("result")] public SearchSuggestionResponseResult? Result { get; set; }
    
    public class SearchSuggestionResponseResult
    {
        [JsonPropertyName("allMatch")] public SearchSuggestionResponseResultItem[]? AllMatch { get; set; }
        
        public class SearchSuggestionResponseResultItem
        {
            [JsonPropertyName("keyword")] public string? Keyword { get; set; }
            [JsonPropertyName("type")] public int Type { get; set; }
            [JsonPropertyName("alg")] public string? Algorithm { get; set; }
            [JsonPropertyName("lastKeyword")] public string? LastKeyword { get; set; }
            [JsonPropertyName("feature")] public string? Feature { get; set; }
            
        }
    }
}

public class SearchSuggestionActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("s")] public required string Keyword { get; set; }
}
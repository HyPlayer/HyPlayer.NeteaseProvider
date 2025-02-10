using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static SongWikiSummaryApi SongWikiSummaryApi => new();
}

public class SongWikiSummaryApi : EApiContractBase<SongWikiSummaryRequest, SongWikiSummaryResponse, ErrorResultBase, SongWikiSummaryActualRequest>
{
    public override string IdentifyRoute => "/song/wiki/summary";
    public override string Url { get; protected set; } = "https://interface3.music.163.com/eapi/music/wiki/home/song/get";
    public override string ApiPath { get; protected set; } = "/api/song/play/about/block/page";

    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest()
    {
        if (Request is not null)
            ActualRequest = new SongWikiSummaryActualRequest
            {
                SongId = Request.SongId
            };
        return Task.CompletedTask;
    }

}

public class SongWikiSummaryRequest : RequestBase
{
    public required string SongId { get; set; }
}

public class SongWikiSummaryResponse : CodedResponseBase
{
    [JsonPropertyName("data")] public SongWikiSummaryResponseData? Data { get; set; }

    public class SongWikiSummaryResponseData
    {
        [JsonPropertyName("cursor")] public string? Cursor { get; set; }
        [JsonPropertyName("hasMore")] public bool HasMore { get; set; }
        [JsonPropertyName("blocks")] public SongWikiSummaryResponseBlock[]? Blocks { get; set; }

        public class SongWikiSummaryResponseBlock
        {
            [JsonPropertyName("showType")] public string? ShowType { get; set; }
            [JsonPropertyName("id")] public string? Id { get; set; }
            [JsonPropertyName("alg")] public string? Algorithm { get; set; }
            [JsonPropertyName("channel")] public string? Channel { get; set; }
            [JsonPropertyName("code")] public string? Code { get; set; }
            [JsonPropertyName("uiElement")] public SongWikiSummaryResponseUiElement? UiElement { get; set; }

            public class SongWikiSummaryResponseUiElement
            {
                // TODO: Finish this complex object
            }
        }
    }
}

public class SongWikiSummaryActualRequest : EApiActualRequestBase
{
    [JsonPropertyName("songId")] public required string SongId { get; set; }
}
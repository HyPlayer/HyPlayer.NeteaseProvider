using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts;

public class ArtistAlbumsApi : WeApiContractBase<ArtistAlbumsRequest, ArtistAlbumsResponse, ErrorResultBase,
    ArtistAlbumsActualRequest>
{
    public override string Url => "https://music.163.com/weapi/artist/albums/";
    public override HttpMethod Method => HttpMethod.Post;

    public override async Task<HttpRequestMessage> GenerateRequestMessageAsync(ApiHandlerOption option)
    {
        var req = await base.GenerateRequestMessageAsync(option);
        req.RequestUri = new Uri(Url + Request!.ArtistId);
        return req;
    }

    public override Task MapRequest(ArtistAlbumsRequest? request)
    {
        if (request is not null)
            ActualRequest = new ArtistAlbumsActualRequest
                            {
                                Limit = request.Limit,
                                Offset = request.Start
                            };
        return Task.CompletedTask;
    }
}

public class ArtistAlbumsRequest : RequestBase
{
    public int Start { get; set; } = 0;
    public int Limit { get; set; } = 30;
    public required string ArtistId { get; set; }
}

public class ArtistAlbumsActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("limit")] public int Limit { get; set; }
    [JsonPropertyName("offset")] public int Offset { get; set; }
    [JsonPropertyName("total")] public bool Total => true;
}

public class ArtistAlbumsResponse : CodedResponseBase
{
    [JsonPropertyName("more")] public bool HasMore { get; set; }
    [JsonPropertyName("hotAlbums")] public AlbumItem[]? Albums { get; set; }


    public class AlbumItem : SongDetailResponse.SongItem.AlbumData
    {
        [JsonPropertyName("alias")] public string[]? Alias { get; set; }
        [JsonPropertyName("transNames")] public string[]? Translations { get; set; }
        [JsonPropertyName("company")] public string? Company { get; set; }
        [JsonPropertyName("briefDesc")] public string? BriefDescription { get; set; }
        [JsonPropertyName("description")] public string? Description { get; set; }
        [JsonPropertyName("subType")] public string? Subtype { get; set; }
        [JsonPropertyName("type")] public string? AlbumType { get; set; }
        [JsonPropertyName("isSub")] public bool IsSubscribed { get; set; }
        [JsonPropertyName("artists")] public SongDetailResponse.SongItem.ArtistItem[]? Artists { get; set; }
    }
}
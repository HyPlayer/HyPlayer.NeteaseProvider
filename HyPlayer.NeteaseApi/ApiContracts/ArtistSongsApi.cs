using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts;

public class ArtistSongsApi : WeApiContractBase<ArtistSongsRequest, ArtistSongsResponse, ErrorResultBase, ArtistSongsActualRequest>
{
    public override string Url => "https://music.163.com/api/v1/artist/songs";
    public override HttpMethod Method => HttpMethod.Post;
    public override async Task MapRequest(ArtistSongsRequest request)
    {
        ActualRequest = new ArtistSongsActualRequest
                        {
                            Id = request.ArtistId,
                            OrderType = request.OrderType,
                            Offset = request.Offset,
                            Limit = request.Limit
                        };
    }
}

public class ArtistSongsActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("id")] public required string Id { get; set; }
    [JsonPropertyName("private_cloud")] public bool PrivateCloud => true;
    [JsonPropertyName("work_type")] public int WorkType => 1;
    [JsonPropertyName("order")] public string OrderType { get; set; } = "hot";
    [JsonPropertyName("offset")] public int Offset { get; set; } = 0;
    [JsonPropertyName("limit")] public int Limit { get; set; } = 100;

}

public class ArtistSongsRequest : RequestBase
{
    public required string ArtistId { get; set; }
    public string OrderType { get; set; } = "hot";
    public int Offset { get; set; } = 0;
    public int Limit { get; set; } = 200;
}

public class ArtistSongsResponse : CodedResponseBase
{
    [JsonPropertyName("total")] public bool Total { get; set; }
    [JsonPropertyName("more")] public bool HasMore { get; set; }
    [JsonPropertyName("songs")] public SongItemWithPrivilege[]? Songs { get; set; }
}

public class SongItemWithPrivilege : SongDetailResponse.SongItem
{
    [JsonPropertyName("privilege")] public SongDetailResponse.PrivilegeItem? Privilege { get; set; }
}
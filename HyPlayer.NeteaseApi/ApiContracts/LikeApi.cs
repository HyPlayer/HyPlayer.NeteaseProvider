using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts;

public class LikeApi : WeApiContractBase<LikeRequest, LikeResponse, ErrorResultBase, LikeActualRequest>
{
    public override string Url => "https://music.163.com/api/radio/like";
    public override HttpMethod Method => HttpMethod.Post;
    public override Task MapRequest(LikeRequest request)
    {
        ActualRequest = new LikeActualRequest
                        {
                            TrackId = request.TrackId,
                            Like = request.Like
                        };
        return Task.CompletedTask;
    }
}

public class LikeActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("alg")] public string Alg => "itembased";
    [JsonPropertyName("trackId")] public required string TrackId { get; set; }
    [JsonPropertyName("like")] public bool Like { get; set; } = true;
    [JsonPropertyName("time")] public int Time => 3;
}

public class LikeRequest : RequestBase
{
    public required string TrackId { get; set; }
    public bool Like { get; set; } = true;
}

public class LikeResponse : CodedResponseBase
{
    
}
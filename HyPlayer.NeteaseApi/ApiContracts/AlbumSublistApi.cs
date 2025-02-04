using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static AlbumSublistApi AlbumSublistApi => new();
}

public class AlbumSublistApi : WeApiContractBase<AlbumSublistRequest, AlbumSublistResponse, ErrorResultBase, AlbumSublistActualRequest>
{
    public override string IdentifyRoute => "/album/sublist";
    public override string Url { get; protected set; } = "https://music.163.com/weapi/album/sublist";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest()
    {
        if (Request is not null)
            ActualRequest = new AlbumSublistActualRequest
            {
                Limit = Request.Limit,
                Offset = Request.Offset
            };

        return Task.CompletedTask;
    }
}

public class AlbumSublistRequest : RequestBase
{

    public int Limit { get; set; } = 25;
    public int Offset { get; set; } = 0;
}

public class AlbumSublistResponse : CodedResponseBase
{
    [JsonPropertyName("data")] public AlbumSublistResponseData[]? Data { get; set; }
    
    public class AlbumSublistResponseData : AlbumDto
    {
        [JsonPropertyName("subTime")] public long SubTime { get; set; }
    }
}



public class AlbumSublistActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("limit")] public int Limit { get; set; } = 25;
    [JsonPropertyName("offset")] public int Offset { get; set; }
    [JsonPropertyName("total")] public bool Total => true;
}
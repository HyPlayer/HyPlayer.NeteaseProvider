using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    /// <summary>
    /// 歌手歌曲
    /// </summary>
    public static ArtistSongsApi ArtistSongsApi => new();
}

public class ArtistSongsApi : WeApiContractBase<ArtistSongsRequest, ArtistSongsResponse, ErrorResultBase,
    ArtistSongsActualRequest>
{
    public override string IdentifyRoute => "/artist/songs";
    public override string Url { get; protected set; } = "https://music.163.com/api/v1/artist/songs";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest()
    {
        if (Request is not null)
            ActualRequest = new ArtistSongsActualRequest
            {
                Id = Request.ArtistId,
                OrderType = Request.OrderType switch
                {
                    ArtistSongsOrderType.Time => "time",
                    _ => "hot"
                },
                Offset = Request.Offset,
                Limit = Request.Limit
            };
        return Task.CompletedTask;
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

public enum ArtistSongsOrderType
{
    Hot,
    Time
}

public class ArtistSongsRequest : RequestBase
{
    /// <summary>
    /// 歌手 ID
    /// </summary>
    public required string ArtistId { get; set; }

    /// <summary>
    /// 排序类型 hot, time
    /// </summary>
    public ArtistSongsOrderType OrderType { get; set; } = ArtistSongsOrderType.Hot;

    /// <summary>
    /// 起始位置
    /// </summary>
    public int Offset { get; set; } = 0;

    /// <summary>
    /// 获取数量
    /// </summary>
    public int Limit { get; set; } = 200;
}

public class ArtistSongsResponse : CodedResponseBase
{
    [JsonPropertyName("total")] public bool Total { get; set; }
    [JsonPropertyName("more")] public bool HasMore { get; set; }
    [JsonPropertyName("songs")] public EmittedSongDtoWithPrivilege[]? Songs { get; set; }
}
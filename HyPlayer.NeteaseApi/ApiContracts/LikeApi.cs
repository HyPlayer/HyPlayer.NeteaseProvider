﻿using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    /// <summary>
    /// 喜欢歌曲
    /// </summary>
    public static LikeApi LikeApi => new();
}

public class LikeApi : WeApiContractBase<LikeRequest, LikeResponse, ErrorResultBase, LikeActualRequest>
{
    public override string IdentifyRoute => "/like";
    public override string Url { get; protected set; } = "https://music.163.com/api/radio/like";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest()
    {
        if (Request is not null)
            ActualRequest = new LikeActualRequest
            {
                TrackId = Request.TrackId,
                Like = Request.Like
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
    /// <summary>
    /// 歌曲 ID
    /// </summary>
    public required string TrackId { get; set; }

    /// <summary>
    /// 是否喜欢
    /// </summary>
    public bool Like { get; set; } = true;
}

public class LikeResponse : CodedResponseBase
{
}
﻿using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    /// <summary>
    /// 歌手专辑
    /// </summary>
    public static ArtistAlbumsApi ArtistAlbumsApi => new();
}

public class ArtistAlbumsApi : WeApiContractBase<ArtistAlbumsRequest, ArtistAlbumsResponse, ErrorResultBase,
    ArtistAlbumsActualRequest>
{
    public override string IdentifyRoute => "/artist/albums";
    public override string Url { get; protected set; } = "https://music.163.com/weapi/artist/albums/";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest()
    {
        if (Request is not null)
        {
            Url += Request.ArtistId;
            ActualRequest = new ArtistAlbumsActualRequest
            {
                Limit = Request.Limit,
                Offset = Request.Start
            };
        }

        return Task.CompletedTask;
    }
}

public class ArtistAlbumsRequest : RequestBase
{
    /// <summary>
    /// 起始位置
    /// </summary>
    public int Start { get; set; } = 0;

    /// <summary>
    /// 获取数量
    /// </summary>
    public int Limit { get; set; } = 30;

    /// <summary>
    /// 歌手 ID
    /// </summary>
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
    [JsonPropertyName("hotAlbums")] public ArtistAlbumDto[]? Albums { get; set; }


    public class ArtistAlbumDto : AlbumDto
    {
        [JsonPropertyName("transNames")] public string[]? Translations { get; set; }
        [JsonPropertyName("type")] public string? AlbumType { get; set; }
        [JsonPropertyName("isSub")] public bool IsSubscribed { get; set; }
        [JsonPropertyName("paid")] public bool Paid { get; set; }
    }
}
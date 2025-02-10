﻿using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    /// <summary>
    /// 创建歌单
    /// </summary>
    public static PlaylistCreateApi PlaylistCreateApi => new();
}

public class PlaylistCreateApi : WeApiContractBase<PlaylistCreateRequest, PlaylistCreateResponse, ErrorResultBase,
    PlaylistCreateActualRequest>
{
    public override string IdentifyRoute => "/playlist/create";
    public override string Url { get; protected set; } = "https://music.163.com/api/playlist/create";
    public override HttpMethod Method => HttpMethod.Post;

    public override string? UserAgent => "pc";

    public override Task MapRequest()
    {
        if (Request is not null)
            ActualRequest = new PlaylistCreateActualRequest
            {
                Name = Request.Name,
                Privacy = Request.Privacy,
                Type = Request.Type,
                CheckToken = Request.CheckToken,
            };
        return Task.CompletedTask;
    }
}

public class PlaylistCreateRequest : RequestBase
{
    /// <summary>
    /// 歌单名
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 隐私性, 10 为隐私
    /// </summary>
    public required int Privacy { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public string Type { get; set; } = "NORMAL";
    /// <summary>
    /// CheckToken
    /// </summary>
    public string CheckToken { get; set; } = "";
}

public class PlaylistCreateResponse : CodedResponseBase
{

}

public class PlaylistCreateActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("name")] public required string Name { get; set; }
    [JsonPropertyName("privacy")] public int Privacy { get; set; }
    [JsonPropertyName("type")] public string Type { get; set; } = "NORMAL";
    [JsonPropertyName("checkToken")] public string CheckToken { get; set; } = "";
}
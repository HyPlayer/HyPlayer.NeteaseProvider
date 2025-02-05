using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    /// <summary>
    /// 热门榜单
    /// </summary>
    public static ToplistApi ToplistApi => new();
}


public class ToplistApi : RawApiContractBase<ToplistRequest, ToplistResponse, ErrorResultBase, ToplistActualRequest>
{
    public override string IdentifyRoute => "/toplist";
    // backup: https://interface.music.163.com/eapi/toplist/detail/v2
    public override string Url { get; protected set; } = "https://music.163.com/api/toplist";
    public override HttpMethod Method => HttpMethod.Get;
    public override Task MapRequest()
    {
        return Task.CompletedTask;
    }
}

public class ToplistRequest : RequestBase
{

}

public class ToplistResponse : CodedResponseBase
{

    [JsonPropertyName("list")] public PlaylistDto[]? List { get; set; }
}

public class ToplistActualRequest : RawApiActualRequestBase
{

}
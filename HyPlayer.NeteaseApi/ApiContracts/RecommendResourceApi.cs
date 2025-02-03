using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static RecommendResourceApi RecommendResourceApi => new();
}

public class RecommendResourceApi : WeApiContractBase<RecommendResourceRequest, RecommendResourceResponse, ErrorResultBase, RecommendResourceActualRequest>
{
    public override string IdentifyRoute => "/recommend/resource";
    public override string Url => "https://music.163.com/weapi/v1/discovery/recommend/resource";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest(RecommendResourceRequest? request)
    {
        return Task.CompletedTask;
    }
}

public class RecommendResourceRequest : RequestBase
{

}

public class RecommendResourceResponse : CodedResponseBase
{
    [JsonPropertyName("featureFirst")] public bool FeatureFirst { get; set; }
    [JsonPropertyName("haveRcmdSongs")] public bool HaveRecommendSongs { get; set; }
    [JsonPropertyName("recommend")] public PlaylistDto[]? Recommends { get; set; }

    public class RecommendResourceItem
    {
        [JsonPropertyName("id")] public string? Id { get; set; }
        [JsonPropertyName("type")] public int Type { get; set; }
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("copywriter")] public string? Copywriter { get; set; }
        [JsonPropertyName("picUrl")] public string? PicUrl { get; set; }
        [JsonPropertyName("playcount")] public long PlayCount { get; set; }
        [JsonPropertyName("createTime")] public long CreateTime { get; set; }
        [JsonPropertyName("creator")] public UserInfoDto? Creator { get; set; }
        [JsonPropertyName("trackCount")] public int TrackCount { get; set; }
        [JsonPropertyName("userId")] public long UserId { get; set; }
        [JsonPropertyName("alg")] public string? Alg { get; set; }
    }
}

public class RecommendResourceActualRequest : WeApiActualRequestBase
{
    
}
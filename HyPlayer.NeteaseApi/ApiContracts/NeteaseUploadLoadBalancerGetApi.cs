using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static NeteaseUploadLoadBalancerGetApi NeteaseUploadLoadBalancerGetApi => new();
}

public class NeteaseUploadLoadBalancerGetApi : RawApiContractBase<NeteaseUploadLoadBalancerGetRequest, NeteaseUploadLoadBalancerGetResponse, ErrorResultBase, NeteaseUploadLoadBalancerGetActualRequest>
{
    public override string IdentifyRoute => "/upload/lbs";
    public override string Url { get; protected set; } = "http://wanproxy.127.net/lbs?version=1.0&bucketname=";
    public override HttpMethod Method => HttpMethod.Get;
    public override Task MapRequest()
    {
        Url += Request?.Bucket ?? "jd-musicrep-privatecloud-audio-public";
        return Task.CompletedTask;
    }
}

public class NeteaseUploadLoadBalancerGetActualRequest : RawApiActualRequestBase
{
}

public class NeteaseUploadLoadBalancerGetResponse : ResponseBase
{
    [JsonPropertyName("lbs")] public string? LoadBalancer { get; set; }
    [JsonPropertyName("upload")] public string[]? Upload { get; set; }
}

public class NeteaseUploadLoadBalancerGetRequest : RequestBase
{
    public string Bucket { get; set; } = "jd-musicrep-privatecloud-audio-public";
}

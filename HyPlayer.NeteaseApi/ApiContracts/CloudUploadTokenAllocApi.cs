using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static CloudUploadTokenAllocApi CloudUploadTokenAllocApi => new();
}

public class CloudUploadTokenAllocApi : RawApiContractBase<CloudUploadTokenAllocRequest, CloudUploadTokenAllocResponse,
    ErrorResultBase, CloudUploadTokenAllocActualRequest>
{
    public override string IdentifyRoute => "/cloud/upload/token/alloc";
    public override string Url { get; protected set; } = "http://musicupload.netease.com/api/whale/token/alloc";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest()
    {
        var rand = new Random();
        if (Request is not null)
            ActualRequest = new CloudUploadTokenAllocActualRequest
            {
                ["channel"] = Request.Channel.ToString(),
                ["filename"] = Request.FileName,
                ["md5"] = Request.Md5,
                ["type"] = Request.Type,
                ["bucket"] = "jd-musicrep-privatecloud-audio-public",
                ["bizKey"] = $"{rand.Next(4096, 65535):x}{rand.Next(65535):x}"
            };
        return Task.CompletedTask;
    }
}

public class CloudUploadTokenAllocRequest : RequestBase
{
    public int Channel { get; set; } = 3;
    public required string FileName { get; set; }
    public required string Md5 { get; set; }
    public string Type { get; set; } = "audio";
}

public class CloudUploadTokenAllocResponse : CodedResponseBase
{
    [JsonPropertyName("data")] public CloudUploadTokenAllocResponseData? Data { get; set; }

    public class CloudUploadTokenAllocResponseData
    {
        [JsonPropertyName("bucket")] public string? Bucket { get; set; }
        [JsonPropertyName("key")] public string? Key { get; set; }
        [JsonPropertyName("objectKey")] public string? ObjectKey { get; set; }
        [JsonPropertyName("token")] public string? Token { get; set; }
        [JsonPropertyName("accessKeyId")] public string? AccessKeyId { get; set; }
        [JsonPropertyName("secretAccessKey")] public string? SecretAccessKey { get; set; }
        [JsonPropertyName("resourceId")] public string? ResourceId { get; set; }
        [JsonPropertyName("channel")] public string? Channel { get; set; }
        [JsonPropertyName("region")] public string? Region { get; set; }
        [JsonPropertyName("endpoint")] public string? Endpoint { get; set; }
        [JsonPropertyName("outerUrl")] public string? OuterUrl { get; set; }
    }
}

public class CloudUploadTokenAllocActualRequest : RawApiActualRequestBase
{
}
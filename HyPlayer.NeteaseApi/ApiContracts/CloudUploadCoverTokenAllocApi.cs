using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static CloudUploadCoverTokenAllocApi CloudUploadCoverTokenAllocApi => new();
}

public class CloudUploadCoverTokenAllocApi : EApiContractBase<CloudUploadCoverTokenAllocRequest, CloudUploadCoverTokenAllocResponse, ErrorResultBase, CloudUploadCoverTokenAllocActualRequest>
{
    public override string IdentifyRoute => "/cloud/upload/cover/token/alloc";
    public override string ApiPath => "/api/nos/token/alloc";

    public override string Url => "http://musicupload.netease.com/eapi/nos/token/alloc";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest(CloudUploadCoverTokenAllocRequest? request)
    {
        if (request is not null)
            ActualRequest = new CloudUploadCoverTokenAllocActualRequest
            {
                Extension = request.Ext,
                FileName = request.Filename,
                Type = request.Type
            };
        return Task.CompletedTask;
    }

}

public class CloudUploadCoverTokenAllocRequest : RequestBase
{
    public required string Ext { get; set; }
    public required string Filename { get; set; }
    public string Type { get; set; } = "other";
}

public class CloudUploadCoverTokenAllocResponse : CodedResponseBase
{
    
    [JsonPropertyName("result")] public CloudUploadCoverTokenAllocResponseResult? Result { get; set; }

    public class CloudUploadCoverTokenAllocResponseResult
    {
        [JsonPropertyName("bucket")] public string? Bucket { get; set; }
        [JsonPropertyName("objectKey")] public string? ObjectKey { get; set; }
        [JsonPropertyName("token")] public string? Token { get; set; }
        [JsonPropertyName("docId")] public string? DocId { get; set; }
    }
}

public class CloudUploadCoverTokenAllocActualRequest : EApiActualRequestBase
{
    [JsonPropertyName("ext")] public required string Extension { get; set; }
    [JsonPropertyName("filename")] public required string FileName { get; set; }
    [JsonPropertyName("type")] public string Type { get; set; } = "other";
}
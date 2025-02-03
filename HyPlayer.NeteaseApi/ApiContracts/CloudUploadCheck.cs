using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static CloudUploadCheckApi CloudUploadCheckApi => new();
}

public class CloudUploadCheckApi : EApiContractBase<CloudUploadCheckRequest, CloudUploadCheckResponse, ErrorResultBase, CloudUploadCheckActualRequest>
{
    public override string IdentifyRoute => "/cloud/upload/check";
    public override string Url => "http://music.163.com/eapi/cloud/upload/check";
    public override string ApiPath => "/api/cloud/upload/check";

    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest(CloudUploadCheckRequest? request)
    {
        if (request is not null)
            ActualRequest = new CloudUploadCheckActualRequest
            {
                BitRate = request.Bitrate,
                Ext = request.Ext,
                Length = request.Length,
                Md5 = request.Md5,
                SongId = request.SongId
            };
        return Task.CompletedTask;
    }

}

public class CloudUploadCheckRequest : RequestBase
{
    public int Bitrate { get; set; }
    public required string Ext { get; set; }
    public long Length { get; set; }
    public required string Md5 { get; set; }
    public string SongId { get; set; } = "0";
}

public class CloudUploadCheckResponse : CodedResponseBase
{
    [JsonPropertyName("needUpload")] public bool NeedUpload { get; set; }
    [JsonPropertyName("songId")] public string? SongId { get; set; }
}

// "bitrate":1788,"ext":".flac","length":56297070,"md5":"1df7b42b3f9362568a0f893579fb6290","songId":"0","version":1
public class CloudUploadCheckActualRequest : EApiActualRequestBase
{
    [JsonPropertyName("bitrate")] public int BitRate { get; set; }
    [JsonPropertyName("ext")] public required string Ext { get; set; }
    [JsonPropertyName("length")] public long Length { get; set; }
    [JsonPropertyName("md5")] public required string Md5 { get; set; }
    [JsonPropertyName("songId")] public string SongId { get; set; } = "0";
    [JsonPropertyName("version")] public int Version => 1;
}
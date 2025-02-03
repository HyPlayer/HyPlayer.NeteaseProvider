using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static CloudUploadInfoApi CloudUploadInfoApi => new();
}

public class CloudUploadInfoApi : EApiContractBase<CloudUploadInfoRequest, CloudUploadInfoResponse, ErrorResultBase, CloudUploadInfoActualRequest>
{
    public override string IdentifyRoute => "";
    public override string Url => "http://musicupload.netease.com/eapi/upload/cloud/info/v2";
    public override string ApiPath => "/api/upload/cloud/info/v2";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest(CloudUploadInfoRequest? request)
    {
        if (request is not null)
            ActualRequest = new CloudUploadInfoActualRequest
            {
                Album = request.Album,
                Artist = request.Artist,
                Bitrate = request.Bitrate,
                CoverId = request.CoverId,
                FileName = request.FileName,
                Md5 = request.Md5,
                ObjectKey = request.ObjectKey,
                ResourceId = request.ResourceId,
                Song = request.Song,
                SongId = request.SongId
            };
        return Task.CompletedTask;
    }


}

public class CloudUploadInfoRequest : RequestBase
{
    public string Album { get; set; } = "未知专辑";
    public string Artist { get; set; } = "未知歌手";
    public int Bitrate { get; set; } = 0;
    public required string CoverId { get; set; }
    public required string FileName { get; set; }
    public required string Md5 { get; set; }
    public required string ObjectKey { get; set; }
    public required string ResourceId { get; set; }
    public string Song { get; set; } = "未知歌曲";
    public required string SongId { get; set; }
}

public class CloudUploadInfoResponse : CodedResponseBase
{
    [JsonPropertyName("privateCloud")] public CloudMusicDto? PrivateCloud { get; set; }
    [JsonPropertyName("songId")] public string? SongId { get; set; }
    [JsonPropertyName("waitTime")] public int WaitTime { get; set; }
    [JsonPropertyName("exists")] public bool Exists { get; set; }
    [JsonPropertyName("nextUploadTime")] public int NextUploadTime { get; set; }
}

public class CloudUploadInfoActualRequest : EApiActualRequestBase
{
    [JsonPropertyName("album")] public string? Album { get; set; }
    [JsonPropertyName("artist")] public string? Artist { get; set; }
    [JsonPropertyName("bitrate")] public int Bitrate { get; set; }
    [JsonPropertyName("coverid")] public required string CoverId { get; set; }
    [JsonPropertyName("filename")] public required string FileName { get; set; }
    [JsonPropertyName("md5")] public required string Md5 { get; set; }
    [JsonPropertyName("objectKey")] public required string ObjectKey { get; set; }
    [JsonPropertyName("resourceId")] public required string ResourceId { get; set; }
    [JsonPropertyName("song")] public string? Song { get; set; }
    [JsonPropertyName("songid")] public required string SongId { get; set; }
}
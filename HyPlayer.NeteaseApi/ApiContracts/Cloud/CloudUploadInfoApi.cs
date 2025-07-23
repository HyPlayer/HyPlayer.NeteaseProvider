using HyPlayer.NeteaseApi.ApiContracts.Cloud;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Bases.EApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static CloudUploadInfoApi CloudUploadInfoApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Cloud
{

    public class CloudUploadInfoApi : EApiContractBase<CloudUploadInfoRequest, CloudUploadInfoResponse, ErrorResultBase,
        CloudUploadInfoActualRequest>
    {
        public override string IdentifyRoute => "";
        public override string Url { get; protected set; } = "http://musicupload.netease.com/eapi/upload/cloud/info/v2";
        public override string ApiPath { get; protected set; } = "/api/upload/cloud/info/v2";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new CloudUploadInfoActualRequest
                {
                    Album = Request.Album,
                    Artist = Request.Artist,
                    Bitrate = Request.Bitrate,
                    CoverId = Request.CoverId,
                    FileName = Request.FileName,
                    Md5 = Request.Md5,
                    ObjectKey = Request.ObjectKey,
                    ResourceId = Request.ResourceId,
                    Song = Request.Song,
                    SongId = Request.SongId
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
}
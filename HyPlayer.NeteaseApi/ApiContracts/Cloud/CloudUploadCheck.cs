using HyPlayer.NeteaseApi.ApiContracts.Cloud;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static CloudUploadCheckApi CloudUploadCheckApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Cloud
{

    public class CloudUploadCheckApi : EApiContractBase<CloudUploadCheckRequest, CloudUploadCheckResponse,
        ErrorResultBase, CloudUploadCheckActualRequest>
    {
        public override string IdentifyRoute => "/cloud/upload/check";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/cloud/upload/check";
        public override string ApiPath { get; protected set; } = "/api/cloud/upload/check";

        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new CloudUploadCheckActualRequest
                {
                    BitRate = Request.Bitrate,
                    Ext = Request.Ext,
                    Length = Request.Length,
                    Md5 = Request.Md5,
                    SongId = Request.SongId
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


    public class CloudUploadCheckActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("bitrate")] public int BitRate { get; set; }
        [JsonPropertyName("ext")] public required string Ext { get; set; }
        [JsonPropertyName("length")] public long Length { get; set; }
        [JsonPropertyName("md5")] public required string Md5 { get; set; }
        [JsonPropertyName("songId")] public string SongId { get; set; } = "0";
        [JsonPropertyName("version")] public int Version => 1;
    }
}
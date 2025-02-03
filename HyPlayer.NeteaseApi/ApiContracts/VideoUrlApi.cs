using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static VideoUrlApi VideoUrlApi => new();
}

public class VideoUrlApi : EApiContractBase<VideoUrlRequest, VideoUrlResponse, ErrorResultBase, VideoUrlActualRequest>
{
    public override string IdentifyRoute => "/video/url";
    public override string Url => "https://interface.music.163.com/eapi/song/enhance/play/mv/url";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest(VideoUrlRequest? request)
    {
        if (request is not null)
            ActualRequest = new VideoUrlActualRequest
            {
                Id = request.Id,
                Resolution = request.Resolution
            };
        return Task.CompletedTask;
    }

    public override string ApiPath => "/api/song/enhance/play/mv/url";
}

public class VideoUrlRequest : RequestBase
{
    public required string Id { get; set; }
    public string Resolution { get; set; } = "480";
}

public class VideoUrlResponse : CodedResponseBase
{

}

public class VideoUrlActualRequest : EApiActualRequestBase
{
    [JsonPropertyName("id")] public required string Id { get; set; }
    [JsonPropertyName("r")] public string Resolution { get; set; } = "480";
}
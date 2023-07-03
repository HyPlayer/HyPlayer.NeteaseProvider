using System.Text.Json.Serialization;
using HyPlayer.NeteaseProvider.Bases;
using HyPlayer.NeteaseProvider.Bases.ApiContractBases;

namespace HyPlayer.NeteaseProvider.ApiContracts;

public class SongUrlApi : EApiContractBase<SongUrlRequest, SongUrlResponse, ErrorResultBase, SongUrlActualRequest>
{
    public override string Url => "https://interface.music.163.com/eapi/song/enhance/player/url/v1";
    public override HttpMethod Method => HttpMethod.Post;

    public override async Task MapRequest(SongUrlRequest request)
    {
        var ids = string.IsNullOrWhiteSpace(request.Id) ? $"[{string.Join(",", request.IdList!)}]" : $"[{request.Id}]";
        ActualRequest = new SongUrlActualRequest
                        {
                            Ids = ids,
                            Level = request.Level
                        };
    }

    public override Dictionary<string, string> Cookies =>
        new()
        {
            { "os", "android" },
            { "appver", "8.10.05" }
        };

    public override string ApiPath => "/api/song/enhance/player/url/v1";
}

public class SongUrlActualRequest : EApiActualRequestBase
{
    [JsonPropertyName("ids")] public required string Ids { get; set; }
    [JsonPropertyName("level")] public required string Level { get; set; }
    [JsonPropertyName("encodeType")] public string EncodeType => "flac";
    [JsonPropertyName("immerseType")] public string ImmerseType => "c51";
}

public class SongUrlRequest : RequestBase
{
    public string? Id { get; set; }
    public string[]? IdList { get; set; }
    public required string Level { get; set; }
}

public class SongUrlResponse : CodedResponseBase
{
    [JsonPropertyName("data")] public SongUrlItem[]? SongUrls { get; set; }

    public class SongUrlItem
    {
        [JsonPropertyName("code")] public int Code { get; set; }
        [JsonPropertyName("id")] public required string Id { get; set; }
        [JsonPropertyName("url")] public string? Url { get; set; }
        [JsonPropertyName("br")] public string? BitRate { get; set; }
        [JsonPropertyName("size")] public long Size { get; set; }
        [JsonPropertyName("md5")] public string? Md5 { get; set; }
        [JsonPropertyName("type")] public string? Type { get; set; }
        [JsonPropertyName("level")] public string? Level { get; set; }
        [JsonPropertyName("encodeType")] public string? EncodeType { get; set; }
        [JsonPropertyName("time")] public long Time { get; set; }
    }
}
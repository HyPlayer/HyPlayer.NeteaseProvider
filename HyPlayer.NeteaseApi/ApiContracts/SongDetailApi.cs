using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts;

public class SongDetailApi : WeApiContractBase<SongDetailRequest, SongDetailResponse, ErrorResultBase, SongDetailActualRequest>
{
    public override string Url => "https://music.163.com/weapi/v3/song/detail";
    public override HttpMethod Method => HttpMethod.Post;
    public override Task MapRequest(SongDetailRequest request)
    {
        var requestIds = string.IsNullOrWhiteSpace(request.Id)
            ? $"[{string.Join(",", request.IdList.Select(id => $$"""{"id":'{{id}}'}"""))}]"
            : $$"""[{"id": '{{request.Id}}'}]""";
        ActualRequest = new SongDetailActualRequest { Ids = requestIds };
        return Task.CompletedTask;
    }
}

public class SongDetailActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("c")] public required string Ids { get; set; }
}

public class SongDetailRequest : RequestBase
{
    public List<string>? IdList { get; set; }
    public string? Id { get; set; }
}

public class SongDetailResponse : CodedResponseBase
{
    [JsonPropertyName("songs")] public SongItem[]? Songs { get; set; }
    [JsonPropertyName("privileges")] public PrivilegeItem[]? Privileges { get; set; }

    public class SongItem
    {
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("id")] public string? Id { get; set; }
        [JsonPropertyName("dt")] public long Duration { get; set; }
        [JsonPropertyName("alia")] public string[]? Alias { get; set; }
        [JsonPropertyName("tns")] public string[]? Translations { get; set; }
        [JsonPropertyName("mv")] public string? MvId { get; set; }
        [JsonPropertyName("cd")] public string? CdName { get; set; }
        [JsonPropertyName("no")] public int TrackNumber { get; set; }
        [JsonPropertyName("al")] public AlbumData? Album { get; set; }
        [JsonPropertyName("ar")] public ArtistItem[]? Artists { get; set; }


        public class ArtistItem
        {
            [JsonPropertyName("id")] public string? Id { get; set; }
            [JsonPropertyName("name")] public string? Name { get; set; }
        }

        public class AlbumData
        {
            [JsonPropertyName("id")] public string? Id { get; set; }
            [JsonPropertyName("name")] public string? Name { get; set; }
            [JsonPropertyName("picUrl")] public string? PictureUrl { get; set; }
        }
    }

    public class PrivilegeItem
    {
        [JsonPropertyName("id")] public string? Id { get; set; }
        [JsonPropertyName("plLevel")] public string? PlayLevel { get; set; }
    }
}
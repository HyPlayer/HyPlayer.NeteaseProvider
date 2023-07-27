using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    /// <summary>
    /// 歌曲详情
    /// </summary>
    public static SongDetailApi SongDetailApi = new();
}


public class SongDetailApi : WeApiContractBase<SongDetailRequest, SongDetailResponse, ErrorResultBase, SongDetailActualRequest>
{
    public override string Url => "https://music.163.com/weapi/v3/song/detail";
    public override HttpMethod Method => HttpMethod.Post;
    public override Task MapRequest(SongDetailRequest? request)
    {
        if (request is null) return Task.CompletedTask;
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
    /// <summary>
    /// 歌曲 ID 列表
    /// </summary>
    public List<string>? IdList { get; set; }
    
    /// <summary>
    /// 歌曲 ID
    /// </summary>
    public string? Id { get; set; }
}

public class SongDetailResponse : CodedResponseBase
{
    [JsonPropertyName("songs")] public EmittedSongDto[]? Songs { get; set; }
    [JsonPropertyName("privileges")] public PrivilegeDto[]? Privileges { get; set; }
}
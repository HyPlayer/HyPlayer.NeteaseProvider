using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static ArtistTopSongApi ArtistTopSongApi => new();
}

public class ArtistTopSongApi : WeApiContractBase<ArtistTopSongRequest, ArtistTopSongResponse, ErrorResultBase, ArtistTopSongActualRequest>
{
    public override string IdentifyRoute => "/artist/top/song";
    public override string Url { get; protected set; } = "https://music.163.com/api/artist/top/song";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest()
    {
        if (Request is not null)
            ActualRequest = new()
            {
                ArtistId = Request.ArtistId
            };
        return Task.CompletedTask;
    }
}

public class ArtistTopSongRequest : RequestBase
{
    public required string ArtistId { get; set; }
}

public class ArtistTopSongResponse : CodedResponseBase
{
    [JsonPropertyName("more")] public bool More { get; set; }
    [JsonPropertyName("songs")] public EmittedSongDtoWithPrivilege[]? Songs { get; set; }
}

public class ArtistTopSongActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("id")] public required string ArtistId { get; set; }
}
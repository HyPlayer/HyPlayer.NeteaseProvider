using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static PlaylistPrivacyApi PlaylistPrivacyApi => new();
}

public class PlaylistPrivacyApi : EApiContractBase<PlaylistPrivacyRequest, PlaylistPrivacyResponse, ErrorResultBase, PlaylistPrivacyActualRequest>
{

    public override string IdentifyRoute => "/playlist/privacy";
    public override string Url => "https://interface.music.163.com/eapi/playlist/update/privacy";
    public override string ApiPath => "/api/playlist/update/privacy";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest(PlaylistPrivacyRequest? request)
    {
        if (request is not null)
            ActualRequest = new PlaylistPrivacyActualRequest
            {
                Id = request.Id
            };
        return Task.CompletedTask;
    }
}

public class PlaylistPrivacyRequest : RequestBase
{
    public required string Id { get; set; }
}

public class PlaylistPrivacyResponse : CodedResponseBase
{

}

public class PlaylistPrivacyActualRequest : EApiActualRequestBase
{
    [JsonPropertyName("id")] public required string Id { get; set; }
    [JsonPropertyName("privacy")] public int Privacy => 0; 
}
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static CloudPubApi CloudPubApi => new();
}

public class CloudPubApi : EApiContractBase<CloudPubRequest, CloudPubResponse, ErrorResultBase, CloudPubActualRequest>
{
    public override string IdentifyRoute => "/cloud/pub";
    public override string Url => "https://interface.music.163.com/eapi/cloud/pub/v2";
    public override string ApiPath => "/api/cloud/pub/v2";

    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest(CloudPubRequest? request)
    {
        if (request is not null)
            ActualRequest = new CloudPubActualRequest
            {
                SongId = request.SongId
            };
        return Task.CompletedTask;
    }

}

public class CloudPubRequest : RequestBase
{
    public required string SongId { get; set; }
}

public class CloudPubResponse : CodedResponseBase
{
    [JsonPropertyName("privateCloud")] public CloudMusicDto? PrivateCloud { get; set; }
}

public class CloudPubActualRequest : EApiActualRequestBase
{
    [JsonPropertyName("songid")] public required string SongId { get; set; }
}
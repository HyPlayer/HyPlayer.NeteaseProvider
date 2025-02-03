using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static CloudDeleteApi CloudDeleteApi => new();
}

public class CloudDeleteApi : EApiContractBase<CloudDeleteRequest, CloudDeleteResponse, ErrorResultBase, CloudDeleteActualRequest>
{
    public override string IdentifyRoute => "/cloud/del";
    public override string Url => "https://interface.music.163.com/eapi/cloud/del";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest(CloudDeleteRequest? request)
    {
        if (request is not null)
            ActualRequest = new CloudDeleteActualRequest
            {
                SongIds = request.ConvertToIdStringList()
            };
        return Task.CompletedTask;
    }

    public override string ApiPath => "/api/cloud/del";
}


public class CloudDeleteRequest : IdOrIdListListRequest
{

}

public class CloudDeleteResponse : CodedResponseBase
{

}

public class CloudDeleteActualRequest : EApiActualRequestBase
{
    [JsonPropertyName("songIds")] public string? SongIds { get; set; }
}
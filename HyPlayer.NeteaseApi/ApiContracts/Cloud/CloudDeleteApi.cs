using HyPlayer.NeteaseApi.ApiContracts.Cloud;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static CloudDeleteApi CloudDeleteApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Cloud
{

    public class CloudDeleteApi : EApiContractBase<CloudDeleteRequest, CloudDeleteResponse, ErrorResultBase,
        CloudDeleteActualRequest>
    {
        public override string IdentifyRoute => "/cloud/del";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/cloud/del";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new CloudDeleteActualRequest
                {
                    SongIds = Request.ConvertToIdStringList()
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/cloud/del";
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
}
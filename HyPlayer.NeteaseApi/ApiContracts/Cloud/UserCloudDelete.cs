using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Cloud;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        /// <summary>
        /// 删除云盘歌曲
        /// </summary>
        public static UserCloudDeleteApi UserCloudDeleteApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Cloud
{
    public class UserCloudDeleteApi : EApiContractBase<IdOrIdListListRequest, UserCloudDeleteResponse, ErrorResultBase
        , UserCloudDeleteActualRequest>
    {
        public override string IdentifyRoute => "/user/cloud/del";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/cloud/del";
        public override HttpMethod Method => HttpMethod.Post;
        public override Task MapRequest()
        {
            if (Request is null) return Task.CompletedTask;
            var ids = Request.ConvertToIdStringList();
            ActualRequest = new UserCloudDeleteActualRequest()
            {
                SongIds = ids
            };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/cloud/del";

    }

    public class UserCloudDeleteResponse : CodedResponseBase
    {
    }

    public class UserCloudDeleteActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("songIds")] public required string SongIds { get; set; }
    }
}
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Album;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static AlbumDetailDynamicApi AlbumDetailDynamicApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Album
{
    public class AlbumDetailDynamicApi : WeApiContractBase<AlbumDetailDynamicRequest, AlbumDetailDynamicResponse,
        ErrorResultBase, AlbumDetailDynamicActualRequest>
    {
        public override string IdentifyRoute => "/album/detail/dynamic";
        public override string Url { get; protected set; } = "https://music.163.com/api/album/detail/dynamic";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest()
        {
            if (Request?.Id is not null)
                ActualRequest = new AlbumDetailDynamicActualRequest
                {
                    Id = Request.Id
                };
            return Task.CompletedTask;
        }
    }

    public class AlbumDetailDynamicRequest : RequestBase
    {
        public required string Id { get; set; }
    }

    public class AlbumDetailDynamicResponse : CodedResponseBase
    {
        [JsonPropertyName("onSale")] public bool OnSale { get; set; }
        [JsonPropertyName("commentCount")] public int CommentCount { get; set; }
        [JsonPropertyName("likedCount")] public int LikedCount { get; set; }
        [JsonPropertyName("shareCount")] public int ShareCount { get; set; }
        [JsonPropertyName("isSub")] public bool IsSub { get; set; }
        [JsonPropertyName("subTime")] public long SubTime { get; set; }
        [JsonPropertyName("subCount")] public int SubCount { get; set; }
    }

    public class AlbumDetailDynamicActualRequest : WeApiActualRequestBase
    {
        [JsonPropertyName("id")] public required string Id { get; set; }
    }
}
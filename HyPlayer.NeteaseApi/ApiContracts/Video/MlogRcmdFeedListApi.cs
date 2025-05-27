using HyPlayer.NeteaseApi.ApiContracts.Artist;
using HyPlayer.NeteaseApi.ApiContracts.Video;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static MlogRcmdFeedListApi MlogRcmdFeedListApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Video
{

    public class MlogRcmdFeedListApi : EApiContractBase<MlogRcmdFeedListRequest, MlogRcmdFeedListResponse,
        ErrorResultBase, MlogRcmdFeedListActualRequest>
    {
        public override string IdentifyRoute => "/mlog/rcmd/feed/list";
        public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/mlog/rcmd/feed/list";
        public override string ApiPath { get; protected set; } = "/api/mlog/rcmd/feed/list";

        public override HttpMethod Method => HttpMethod.Post;

        public override async Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
            {

                ActualRequest = new MlogRcmdFeedListActualRequest
                {
                    Id = Request.Id,
                    Limit = Request.Limit
                };

                if (!string.IsNullOrEmpty(Request.SongId))
                {
                    ActualRequest.ExtInfo = JsonSerializer.Serialize(new Dictionary<string, string>()
                    {
                        ["songId"] = Request.SongId
                    });
                }
            }

            await Task.CompletedTask;
        }

    }

    public class MlogRcmdFeedListRequest : RequestBase
    {
        public required string Id { get; set; }
        public string? SongId { get; set; }
        public int Limit { get; set; } = 10;
    }

    public class MlogRcmdFeedListResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public MlogRcmdFeedListResponseData? Data { get; set; }

        public class MlogRcmdFeedListResponseData
        {
            [JsonPropertyName("feeds")]
            public ArtistVideoResponse.ArtistVideoResponseData.ArtistVideoResponseDataRecord[]? Feeds { get; set; }
        }
    }

    public class MlogRcmdFeedListActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("id")] public required string Id { get; set; }
        [JsonPropertyName("type")] public int Type => 2;
        [JsonPropertyName("rcmdType")] public int RcmdType => 20;
        [JsonPropertyName("limit")] public int Limit { get; set; } = 10;
        [JsonPropertyName("extInfo")] public string? ExtInfo { get; set; }
    }
}
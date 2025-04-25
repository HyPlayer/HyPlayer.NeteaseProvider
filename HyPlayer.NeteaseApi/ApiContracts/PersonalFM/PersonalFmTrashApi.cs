using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.PersonalFM;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        /// <summary>
        /// 私人 FM 垃圾桶
        /// </summary>
        public static PersonalFmTrashApi PersonalFmTrashApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.PersonalFM
{

    public class
        PersonalFmTrashApi : WeApiContractBase<FmTrashRequest, FmTrashResponse, ErrorResultBase, FmTrashActualRequest>
    {
        public override string IdentifyRoute => "/fm/trash";
        public override string Url { get; protected set; } = "";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest()
        {
            if (Request is not null)
                ActualRequest = new FmTrashActualRequest()
                {
                    Id = Request.Id
                };
            return Task.CompletedTask;
        }

        public override async Task<HttpRequestMessage> GenerateRequestMessageAsync(
            ApiHandlerOption option, CancellationToken cancellationToken = default)
        {
            var req = await base.GenerateRequestMessageAsync(option, cancellationToken).ConfigureAwait(false);
            req.RequestUri =
                new Uri($"https://music.163.com/weapi/radio/trash/add?alg=RT&songId={Request?.Id}&time=25");
            return req;
        }
    }

    public class FmTrashRequest : RequestBase
    {
        /// <summary>
        /// 歌曲 ID
        /// </summary>
        public required string Id { get; set; }
    }

    public class FmTrashResponse : CodedResponseBase
    {
    }

    public class FmTrashActualRequest : WeApiActualRequestBase
    {
        [JsonPropertyName("songId")] public required string Id { get; set; }
    }
}
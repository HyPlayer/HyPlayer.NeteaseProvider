using HyPlayer.NeteaseApi.ApiContracts.Playlist;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        /// <summary>
        /// 歌单收藏
        /// </summary>
        public static PlaylistSubscribeApi PlaylistSubscribeApi => new();
    }
}


namespace HyPlayer.NeteaseApi.ApiContracts.Playlist
{
    public class PlaylistSubscribeApi : WeApiContractBase<PlaylistSubscribeRequest, PlaylistSubscribeResponse,
        ErrorResultBase, PlaylistSubscribeActualRequest>
    {
        public override string IdentifyRoute => "/playlist/subscribe";
        public override string Url { get; protected set; } = "https://music.163.com/weapi/playlist/";
        public override HttpMethod Method => HttpMethod.Post;

        private string _action = "subscribe";

        public override async Task<HttpRequestMessage> GenerateRequestMessageAsync(
            ApiHandlerOption option, CancellationToken cancellationToken = default)
        {
            var req = await base.GenerateRequestMessageAsync(option, cancellationToken).ConfigureAwait(false);
            req.RequestUri = new Uri(Url + _action);
            return req;
        }

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is null) return Task.CompletedTask;
            ActualRequest = new()
            {
                PlaylistId = Request.PlaylistId
            };
            _action = Request.IsSubscribe ? "subscribe" : "unsubscribe";
            return Task.CompletedTask;
        }
    }

    public class PlaylistSubscribeActualRequest : WeApiActualRequestBase
    {
        [JsonPropertyName("id")] public required string PlaylistId { get; set; }
    }

    public class PlaylistSubscribeRequest : RequestBase
    {
        /// <summary>
        /// 是否收藏
        /// </summary>
        public bool IsSubscribe { get; set; } = true;

        /// <summary>
        /// 歌单 ID
        /// </summary>
        public required string PlaylistId { get; set; }
    }

    public class PlaylistSubscribeResponse : CodedResponseBase
    {
    }
}
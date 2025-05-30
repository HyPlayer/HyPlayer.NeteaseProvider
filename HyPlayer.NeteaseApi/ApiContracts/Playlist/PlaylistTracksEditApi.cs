﻿using HyPlayer.NeteaseApi.ApiContracts.Playlist;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        /// <summary>
        /// 歌单歌曲编辑
        /// </summary>
        public static PlaylistTracksEditApi PlaylistTracksEditApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Playlist
{
    public class PlaylistTracksEditApi : WeApiContractBase<PlaylistTracksEditRequest, PlaylistTracksEditResponse,
        ErrorResultBase, PlaylistTracksEditActualRequest>
    {
        public override string IdentifyRoute => "/playlist/tracks/edit";
        public override string Url { get; protected set; } = "https://music.163.com/weapi/playlist/manipulate/tracks";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is null) return Task.CompletedTask;
            var trackIds = Request.ConvertToIdStringList();

            ActualRequest = new PlaylistTracksEditActualRequest
            {
                Operation = Request.IsAdd ? "add" : "del",
                PlaylistId = Request.PlaylistId,
                TrackIds = trackIds
            };
            return Task.CompletedTask;
        }
    }

    public class PlaylistTracksEditActualRequest : WeApiActualRequestBase
    {
        [JsonPropertyName("op")] public required string Operation { get; set; }
        [JsonPropertyName("pid")] public required string PlaylistId { get; set; }
        [JsonPropertyName("trackIds")] public required string TrackIds { get; set; }
        [JsonPropertyName("imme")] public bool Imme => true;

    }

    public class PlaylistTracksEditRequest : IdOrIdListListRequest
    {
        /// <summary>
        /// 是否添加
        /// </summary>
        public bool IsAdd { get; set; } = true;

        /// <summary>
        /// 歌单 ID
        /// </summary>
        public required string PlaylistId { get; set; }
    }

    public class PlaylistTracksEditResponse : CodedResponseBase
    {

    }
}
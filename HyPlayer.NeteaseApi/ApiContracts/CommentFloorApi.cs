using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Extensions;
using HyPlayer.NeteaseApi.Models;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static CommentFloorApi CommentFloorApi => new();
}

public class CommentFloorApi : EApiContractBase<CommentFloorRequest, CommentFloorResponse, ErrorResultBase,
    CommentFloorActualRequest>
{
    public override string IdentifyRoute => "/comment/floor";
    public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/v2/resource/comment/floor/get";
    public override HttpMethod Method => HttpMethod.Post;

    public override string ApiPath { get; protected set; } = "/api/v2/resource/comment/floor/get";

    public override Task MapRequest()
    {
        if (Request is not null)
        {
            var threadId = $"{NeteaseUtils.CommentTypeToThreadPrefix(Request.ResourceType)}{Request.ResourceId}";
            ActualRequest = new CommentFloorActualRequest
            {
                ThreadId = threadId,
                ParentCommentId = Request.ParentCommentId,
                Time = Request.Time,
                Limit = Request.Limit
            };
        }

        return Task.CompletedTask;
    }
}

public class CommentFloorRequest : RequestBase
{
    public NeteaseResourceType ResourceType { get; set; } = NeteaseResourceType.Song;
    public required string ResourceId { get; set; }

    /// <summary>
    /// 父评论 ID
    /// </summary>
    public required string ParentCommentId { get; set; }

    /// <summary>
    /// 分页参数 - 最后一项的 time
    /// </summary>
    public long Time { get; set; } = 0;

    /// <summary>
    /// 评论数量
    /// </summary>
    public int Limit { get; set; } = 20;
}

public class CommentFloorResponse : CodedResponseBase
{
    [JsonPropertyName("data")] public CommentFloorData? Data { get; set; }

    public class CommentFloorData
    {
        [JsonPropertyName("time")] public long Time { get; set; }
        [JsonPropertyName("hasMore")] public bool HasMore { get; set; }
        [JsonPropertyName("comments")] public CommentDto[]? Comments { get; set; }
        [JsonPropertyName("ownerComment")] public CommentDto? OwnerComments { get; set; }
        [JsonPropertyName("bestComments")] public CommentDto[]? BestComments { get; set; }
        [JsonPropertyName("totalCount")] public int TotalCount { get; set; }
    }
}

public class CommentFloorActualRequest : EApiActualRequestBase
{
    [JsonPropertyName("threadId")] public required string ThreadId { get; set; }
    [JsonPropertyName("parentCommentId")] public required string ParentCommentId { get; set; }
    [JsonPropertyName("time")] public long Time { get; set; } = 0;
    [JsonPropertyName("limit")] public int Limit { get; set; } = 20;
    [JsonPropertyName("order")] public long Order { get; set; } = 0;
    [JsonPropertyName("scene")] public string Scene { get; set; } = "SONG_COMMENT";
    [JsonPropertyName("cursor")] public string Cursor { get; set; } = "";

}
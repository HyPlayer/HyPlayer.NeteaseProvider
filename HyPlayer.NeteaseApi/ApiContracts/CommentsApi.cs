using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Extensions;
using HyPlayer.NeteaseApi.Models;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static CommentsApi CommentsApi => new();
}

public class CommentsApi : EApiContractBase<CommentsRequest, CommentsResponse, ErrorResultBase, CommentsActualRequest>
{
    public override string IdentifyRoute => "/comments";
    public override string Url { get; protected set; } = "https://interface3.music.163.com/eapi/v2/resource/comments";
    public override string ApiPath { get; protected set; } = "/api/v2/resource/comments";
    public override HttpMethod Method => HttpMethod.Post;


    public override Task MapRequest()
    {
        if (Request is null) return Task.CompletedTask;
        var threadId = $"{NeteaseUtils.CommentTypeToThreadPrefix(Request.ResourceType)}{Request.ResourceId}";
        var cursor = Request.Cursor ?? Request.CommentSortType switch
        {
            CommentSortType.Recommend => ((Request.PageNo - 1) * Request.PageSize).ToString(),
            CommentSortType.Hot => $"normalHot#{(Request.PageNo - 1) * Request.PageSize}",
            _ => "0"
        };
        ActualRequest = new CommentsActualRequest
        {
            ThreadId = threadId,
            Cursor = cursor,
            SortType = (int)Request.CommentSortType,
            PageSize = Request.PageSize,
            PageNo = Request.PageNo,
            ShowInnerComment = false,
            ParentCommentId = "0"
        };
        return Task.CompletedTask;
    }
}

public class CommentsRequest : RequestBase
{
    public NeteaseResourceType ResourceType { get; set; } = NeteaseResourceType.Song;
    public required string ResourceId { get; set; }
    public CommentSortType CommentSortType { get; set; }
    public int PageSize { get; set; } = 20;
    public int PageNo { get; set; } = 1;
    public string? Cursor { get; set; }
}

public enum CommentSortType
{
    Recommend = 1,
    Hot = 2,
    Time = 3
}

public class CommentsResponse : CodedResponseBase
{
    [JsonPropertyName("data")] public CommentsResponseData? Data { get; set; }
}

public class CommentsResponseData
{
    [JsonPropertyName("commentsTitle")] public string? CommentsTitle { get; set; }
    [JsonPropertyName("comments")] public CommentDto[]? Comments { get; set; }
    [JsonPropertyName("currentCommentTitle")] public string? CurrentCommentTitle { get; set; }
    [JsonPropertyName("totalCount")] public long TotalCount { get; set; }
    [JsonPropertyName("hasMore")] public bool HasMore { get; set; }
    [JsonPropertyName("cursor")] public string? Cursor { get; set; }
}

public class CommentsActualRequest : EApiActualRequestBase
{
    [JsonPropertyName("threadId")] public required string ThreadId { get; set; }
    [JsonPropertyName("cursor")] public string Cursor { get; set; } = "0";
    [JsonPropertyName("sortType")] public int SortType { get; set; } = 1;
    [JsonPropertyName("pageSize")] public int PageSize { get; set; } = 20;
    [JsonPropertyName("pageNo")] public int PageNo { get; set; } = 1;
    [JsonPropertyName("showInner")] public bool ShowInnerComment { get; set; } = true;
    [JsonPropertyName("parentCommentId")] public required string ParentCommentId { get; set; } = "0";
}
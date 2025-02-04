using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Extensions;
using HyPlayer.NeteaseApi.Models;
using HyPlayer.NeteaseApi.Models.ResponseModels;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static CommentsApi CommentsApi => new();
}

public class CommentsApi : EApiContractBase<CommentsRequest, CommentsResponse, ErrorResultBase, CommentsActualRequest>
{
    public override string IdentifyRoute => "/comments";
    public override string Url { get; protected set; } = "https://interface.music.163.com/eapi/v1/resource/comments/R_SO_4_2033878955";
    public override string ApiPath { get; protected set; } = "/api/v1/resource/comments/";
    public override HttpMethod Method => HttpMethod.Post;

   
    public override Task MapRequest()
    {
        if (Request is null) return Task.CompletedTask;
        var resourceId = $"{NeteaseUtils.CommentTypeTransformer(Request.ResourceType)}{Request.Id}";
        ApiPath = $"/api/v1/resource/comments/{resourceId}";
        ActualRequest = new CommentsActualRequest()
        {
            Limit = Request.Limit,
            Offset = Request.Offset,
            BeforeTime = Request.BeforeTime,
            CompareUserLocation = Request.CompareUserLocation,
            CommentId = Request.CommentId,
            ComposeConcert = Request.ComposeConcert,
            MarkReplied = Request.MarkReplied,
            ForceFlatComment = Request.ForceFlatComment,
            ShowInner = Request.ShowInner
        };
        return Task.CompletedTask;
    }
}

public class CommentsRequest : RequestBase
{
    public NeteaseResourceType ResourceType { get; set; } = NeteaseResourceType.Song;
    public required string Id { get; set; }
    public int Limit { get; set; } = 60;
    public int Offset { get; set; } = 0;
    public int BeforeTime { get; set; } = 0;
    public bool CompareUserLocation { get; set; } = false;
    public string CommentId { get; set; } = "0";
    public bool ComposeConcert { get; set; } = false;
    public bool MarkReplied { get; set; } = false;
    public bool ForceFlatComment { get; set; } = false;
    public bool ShowInner { get; set; } = false;
}

public class CommentsResponse : CodedResponseBase
{
    [JsonPropertyName("isMusician")] public bool IsMusician { get; set; }
    [JsonPropertyName("userId")] public string? UserId { get; set; }
    [JsonPropertyName("moreHot")] public bool MoreHot { get; set; }
    [JsonPropertyName("total")] public long Total { get; set; }
    [JsonPropertyName("more")] public bool More { get; set; }
    [JsonPropertyName("hotComments")] public CommentDto[]? HotComments { get; set; }
    [JsonPropertyName("topComments")] public CommentDto[]? TopComments { get; set; }
    [JsonPropertyName("comments")] public CommentDto[]? Comments { get; set; }
}

public class CommentsActualRequest : EApiActualRequestBase
{
    [JsonPropertyName("limit")] public int Limit { get; set; } = 60;
    [JsonPropertyName("offset")] public int Offset { get; set; } = 0;
    [JsonPropertyName("beforeTime")] public int BeforeTime { get; set; } = 0;
    [JsonPropertyName("compareUserLocation")] public bool CompareUserLocation { get; set; } = false;
    [JsonPropertyName("commentId")] public string CommentId { get; set; } = "0";
    [JsonPropertyName("composeConcert")] public bool ComposeConcert { get; set; } = false;
    [JsonPropertyName("markReplied")] public bool MarkReplied { get; set; } = false;
    [JsonPropertyName("forceFlatComment")] public bool ForceFlatComment { get; set; } = false;
    [JsonPropertyName("showInner")] public bool ShowInner { get; set; } = false;
}
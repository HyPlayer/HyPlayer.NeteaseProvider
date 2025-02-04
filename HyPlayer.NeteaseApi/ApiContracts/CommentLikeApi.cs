using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Extensions;
using HyPlayer.NeteaseApi.Models;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static CommentLikeApi CommentLikeApi => new();
}

public class CommentLikeApi : WeApiContractBase<CommentLikeRequest, CommentLikeResponse, ErrorResultBase,
    CommentLikeActualRequest>
{
    public override string IdentifyRoute => "/comment/like";
    public override string Url { get; protected set; } = "https://music.163.com/weapi/v1/comment/";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest()
    {
        if (Request is not null)
            ActualRequest = new CommentLikeActualRequest
            {
                CommentId = Request.CommentId,
                ThreadId = Request.ThreadId ?? NeteaseUtils.CommentTypeTransformer(Request.ResourceType) + Request.CommentId
            };
        return Task.CompletedTask;
    }

    public override async Task<HttpRequestMessage> GenerateRequestMessageAsync<TActualRequestModel>(TActualRequestModel actualRequest, ApiHandlerOption option,
        CancellationToken cancellationToken = default)
    {
        var req = await base.GenerateRequestMessageAsync(actualRequest, option, cancellationToken).ConfigureAwait(false);
        req.RequestUri = new Uri(Url + (Request?.IsLike == true ? "like" : "unlike"));
        return req;
    }
}

public class CommentLikeRequest : RequestBase
{
    public bool IsLike { get; set; } = true;
    public required string CommentId { get; set; }
    public required NeteaseResourceType ResourceType { get; set; }
    public string? ThreadId { get; set; }
}

public class CommentLikeResponse : CodedResponseBase
{
}

public class CommentLikeActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("commentId")] public required string CommentId { get; set; }
    [JsonPropertyName("threadId")] public required string ThreadId { get; set; }
}
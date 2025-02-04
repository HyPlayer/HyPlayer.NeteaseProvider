using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
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
    public override string Url => "https://music.163.com/weapi/v1/comment/";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest(CommentLikeRequest? request)
    {
        if (request is not null)
            ActualRequest = new CommentLikeActualRequest
            {
                CommentId = request.CommentId,
                ThreadId = request.ThreadId ?? CommentTypeTransformer(request.ResourceType) + request.CommentId
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

    private static string CommentTypeTransformer(NeteaseResourceType type)
    {
        switch (type)
        {
            case NeteaseResourceType.Song: return "R_SO_4_";
            case NeteaseResourceType.MV: return "R_MV_5_";
            case NeteaseResourceType.Playlist: return "A_PL_0_";
            case NeteaseResourceType.Album: return "R_AL_3_";
            case NeteaseResourceType.RadioChannel: return "A_DJ_1_";
            case NeteaseResourceType.Video: return "R_VI_62_";
            case NeteaseResourceType.Dynamic: return "A_EV_2_";
            case NeteaseResourceType.MLog: return "R_MLOG_1001_";
            default: throw new ArgumentOutOfRangeException(nameof(type));
        }
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
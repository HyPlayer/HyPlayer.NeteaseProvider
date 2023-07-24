using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts;

public class PlaylistSubscribeApi : WeApiContractBase<PlaylistSubscribeRequest, PlaylistSubscribeResponse,
    ErrorResultBase, PlaylistSubscribeActualRequest>
{
    public override string Url => "https://music.163.com/weapi/playlist/";
    public override HttpMethod Method => HttpMethod.Post;

    private string _action = "subscribe";

    public override async Task<HttpRequestMessage> GenerateRequestMessageAsync(ApiHandlerOption option)
    {
        var req = await base.GenerateRequestMessageAsync(option);
        req.RequestUri = new Uri(Url + _action);
        return req;
    }

    public override Task MapRequest(PlaylistSubscribeRequest? request)
    {
        if (request is null) return Task.CompletedTask;
            ActualRequest = new()
                            {
                                PlaylistId = request.PlaylistId
                            };
        _action = request.IsSubscribe ? "subscribe" : "unsubscribe";
        return Task.CompletedTask;
    }
}

public class PlaylistSubscribeActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("id")] public required string PlaylistId { get; set; }
}

public class PlaylistSubscribeRequest : RequestBase
{
    public bool IsSubscribe { get; set; } = true;
    public required string PlaylistId { get; set; }
}

public class PlaylistSubscribeResponse : CodedResponseBase
{
}
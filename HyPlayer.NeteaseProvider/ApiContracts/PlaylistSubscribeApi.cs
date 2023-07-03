using System.Text.Json.Serialization;
using HyPlayer.NeteaseProvider.Bases;
using HyPlayer.NeteaseProvider.Bases.ApiContractBases;

namespace HyPlayer.NeteaseProvider.ApiContracts;

public class PlaylistSubscribeApi : WeApiContractBase<PlaylistSubscribeRequest, PlaylistSubscribeResponse, ErrorResultBase, PlaylistSubscribeActualRequest>
{
    public override string Url => "https://music.163.com/weapi/playlist/";
    public override HttpMethod Method => HttpMethod.Post;

    private string _action = "subscribe";

    public override Task<HttpRequestMessage> GenerateRequestMessageAsync(ProviderOption option)
    {
        var req = base.GenerateRequestMessageAsync(option).Result;
        req.RequestUri = new Uri(Url + _action);
        return Task.FromResult(req);
    }

    public override async Task MapRequest(PlaylistSubscribeRequest request)
    {
        ActualRequest = new()
                        {
                            PlaylistId = request.PlaylistId
                        };
        _action = request.IsSubscribe ? "subscribe" : "unsubscribe";
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
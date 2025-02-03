using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static AlbumSubscribeApi AlbumSubscribeApi => new();
}

public class AlbumSubscribeApi : WeApiContractBase<AlbumSubscribeRequest, AlbumSubscribeResponse, ErrorResultBase,
    AlbumSubscribeActualRequest>
{
    public override string IdentifyRoute => "/album/subscribe";
    public override string Url => "https://music.163.com/api/album/";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest(AlbumSubscribeRequest? request)
    {
        if (request is not null)
            ActualRequest = new AlbumSubscribeActualRequest
            {
                Id = request.Id
            };
        return Task.CompletedTask;
    }

    public override async Task<HttpRequestMessage> GenerateRequestMessageAsync<TActualRequestModel>(
        TActualRequestModel actualRequest, ApiHandlerOption option,
        CancellationToken cancellationToken = default)
    {
        var req = await base.GenerateRequestMessageAsync(actualRequest, option, cancellationToken);
        req.RequestUri = new Uri(Url + (Request?.IsSubscribe is true ? "sub" : "unsub"));
        return req;
    }
}

public class AlbumSubscribeRequest : RequestBase
{
    public required string Id { get; set; }
    public bool IsSubscribe { get; set; }
}

public class AlbumSubscribeResponse : CodedResponseBase
{
}

public class AlbumSubscribeActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("id")] public required string Id { get; set; }
}
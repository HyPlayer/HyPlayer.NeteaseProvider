using HyPlayer.NeteaseApi.Extensions;

namespace HyPlayer.NeteaseApi.Bases;

public abstract class ApiContractBase<TRequest, TResponse, TError, TActualRequest>
    : ApiContractBase, IApiMapRequest
    where TRequest : RequestBase
    where TActualRequest : ActualRequestBase
    where TError : ErrorResultBase
    where TResponse : ResponseBase, new()
{
    public abstract string IdentifyRoute { get; }
    public abstract string Url { get; protected set; }
    public abstract HttpMethod Method { get; }
    public virtual Dictionary<string, string> Cookies { get; } = new();
    public TRequest? Request { get; set; }

    public TActualRequest? ActualRequest { get; set; }
    public virtual string? UserAgent { get; } = null;

    public abstract Task<HttpRequestMessage> GenerateRequestMessageAsync<TActualRequestModel>(
        TActualRequestModel actualRequest, ApiHandlerOption option, CancellationToken cancellationToken = default);

    public abstract Task<Results<TResponse, ErrorResultBase>> ProcessResponseAsync(
        HttpResponseMessage response, ApiHandlerOption option, CancellationToken cancellationToken = default);

    public abstract Task<Results<TResponseModel, ErrorResultBase>> ProcessResponseAsync<TResponseModel>(
        HttpResponseMessage response, ApiHandlerOption option, CancellationToken cancellationToken = default)
        where TResponseModel : ResponseBase, new();

    public abstract Task MapRequest(ApiHandlerOption option);
}

public interface IApiMapRequest
{
    Task MapRequest(ApiHandlerOption option);
}

public abstract class ApiContractBase
{
    public abstract Task<HttpRequestMessage> GenerateRequestMessageAsync(
        ApiHandlerOption option, CancellationToken cancellationToken = default);

}
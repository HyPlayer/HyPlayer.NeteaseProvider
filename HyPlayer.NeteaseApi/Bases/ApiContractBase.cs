using Kengwang.Toolkit;

namespace HyPlayer.NeteaseApi.Bases;

public abstract class ApiContractBase<TRequest, TResponse, TError, TActualRequest>
    where TRequest : RequestBase
    where TActualRequest : ActualRequestBase
    where TError : ErrorResultBase
{
    public abstract string Url { get; }
    public abstract HttpMethod Method { get; }
    public virtual Dictionary<string, string> Cookies { get; } = new();
    public TRequest? Request { get; set; }
    public TActualRequest? ActualRequest { get; set; }
    public virtual string? UserAgent { get; } = null;
    public abstract Task MapRequest(TRequest request);
    public abstract Task<HttpRequestMessage> GenerateRequestMessageAsync(ApiHandlerOption option);

    public abstract Task<Results<TResponse, ErrorResultBase>> ProcessResponseAsync(
        HttpResponseMessage response, ApiHandlerOption option);
}
using HyPlayer.NeteaseApi.Extensions;

namespace HyPlayer.NeteaseApi.Bases;

public abstract class ApiContractBase<TRequest, TResponse, TError, TActualRequest>
    : ApiContractBase
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
    public abstract Task MapRequest();

    public abstract Task<HttpRequestMessage> GenerateRequestMessageAsync<TActualRequestModel>(
        TActualRequestModel actualRequest, ApiHandlerOption option, CancellationToken cancellationToken = default);

    public abstract Task<Results<TResponse, ErrorResultBase>> ProcessResponseAsync(
        HttpResponseMessage response, ApiHandlerOption option, CancellationToken cancellationToken = default);

    public abstract Task<Results<TResponseModel, ErrorResultBase>> ProcessResponseAsync<TResponseModel>(
        HttpResponseMessage response, ApiHandlerOption option, CancellationToken cancellationToken = default)
        where TResponseModel : ResponseBase, new();
    protected void CheckApiPrivileges(ApiHandlerOption option, RequestBase api)
    {
        if (option.AdditionalParameters.HasValue()) return;
        if (api is not ICheckTokenApi || option.EnableCheckTokenApi is true) return;
        throw new InvalidOperationException("此操作需要启用 CheckToken 接口");
    }
}

public abstract class ApiContractBase
{
    public abstract Task<HttpRequestMessage> GenerateRequestMessageAsync(
        ApiHandlerOption option, CancellationToken cancellationToken = default);

}
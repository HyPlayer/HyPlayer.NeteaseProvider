using HyPlayer.NeteaseApi.Bases;
using Kengwang.Toolkit;
using System.Net;

namespace HyPlayer.NeteaseApi;

public class NeteaseCloudMusicApiHandler
{
    private readonly HttpClientHandler _httpClientHandler =
        new()
        {
            UseCookies = false,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        };

    public void UseProxyConfiguration(bool useProxy, IWebProxy proxy)
    {
        _httpClientHandler.UseProxy = useProxy;
        _httpClientHandler.Proxy = proxy;
    }

    public async Task<Results<TResponse, ErrorResultBase>> RequestAsync<TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, ApiHandlerOption option,
        CancellationToken cancellationToken = default)
        where TError : ErrorResultBase where TActualRequest : ActualRequestBase where TRequest : RequestBase where TResponse : ResponseBase, new()
    {
        var client = new HttpClient(_httpClientHandler);
        var response = await client.SendAsync(await contract.GenerateRequestMessageAsync(option, cancellationToken),
                                              cancellationToken).ConfigureAwait(false);
        return await contract.ProcessResponseAsync(response, option, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Results<TResponse, ErrorResultBase>> RequestAsync<TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, TRequest? request,
        ApiHandlerOption option, CancellationToken cancellationToken = default)
        where TError : ErrorResultBase where TActualRequest : ActualRequestBase where TRequest : RequestBase where TResponse : ResponseBase, new()
    {
        var client = new HttpClient(_httpClientHandler);
        contract.Request = request;
        await contract.MapRequest(request).ConfigureAwait(false);
        var response = await client.SendAsync(await contract.GenerateRequestMessageAsync(option, cancellationToken).ConfigureAwait(false), cancellationToken).ConfigureAwait(false);
        return await contract.ProcessResponseAsync(response, option, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Results<TCustomResponse, ErrorResultBase>> RequestAsync<TCustomResponse, TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, TRequest? request,
        ApiHandlerOption option, CancellationToken cancellationToken = default)
        where TError : ErrorResultBase where TActualRequest : ActualRequestBase where TRequest : RequestBase where TCustomResponse : ResponseBase, new() where TResponse : ResponseBase, new()
    {
        var client = new HttpClient(_httpClientHandler);
        contract.Request = request;
        await contract.MapRequest(request).ConfigureAwait(false);
        var response = await client.SendAsync(await contract.GenerateRequestMessageAsync(option, cancellationToken).ConfigureAwait(false), cancellationToken).ConfigureAwait(false);
        return await contract.ProcessResponseAsync<TCustomResponse>(response, option, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Results<TCustomResponse, ErrorResultBase>> RequestAsync<TCustomRequest, TCustomResponse, TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, bool differ, TCustomRequest? request,
        ApiHandlerOption option, CancellationToken cancellationToken = default)
        where TError : ErrorResultBase where TActualRequest : ActualRequestBase where TRequest : RequestBase where TResponse : ResponseBase, new() where TCustomResponse : ResponseBase, new()
    {
        var client = new HttpClient(_httpClientHandler);
        var response = await client.SendAsync(await contract.GenerateRequestMessageAsync<TCustomRequest>(request!, option, cancellationToken).ConfigureAwait(false), cancellationToken).ConfigureAwait(false);
        return await contract.ProcessResponseAsync<TCustomResponse>(response, option, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Results<TResponse, ErrorResultBase>> RequestAsync<TCustomRequest, TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, bool differ, TCustomRequest? request,
        ApiHandlerOption option, CancellationToken cancellationToken = default)
        where TError : ErrorResultBase where TActualRequest : ActualRequestBase where TRequest : RequestBase where TResponse : ResponseBase, new()
    {
        var client = new HttpClient(_httpClientHandler);
        var response = await client.SendAsync(await contract.GenerateRequestMessageAsync<TCustomRequest>(request!, option, cancellationToken), cancellationToken).ConfigureAwait(false);
        return await contract.ProcessResponseAsync(response, option, cancellationToken).ConfigureAwait(false);
    }
}
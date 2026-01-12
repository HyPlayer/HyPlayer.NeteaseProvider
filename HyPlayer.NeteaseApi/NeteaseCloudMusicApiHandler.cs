using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Extensions;
using System.Net;

namespace HyPlayer.NeteaseApi;

public class NeteaseCloudMusicApiHandler
{
    public NeteaseCloudMusicApiHandler()
    {
        HttpClient = new HttpClient();
    }
    public NeteaseCloudMusicApiHandler(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }
    public ApiHandlerOption Option { get; set; } = new();
    public HttpClient HttpClient { get; set; }
    public static readonly HttpClientHandler HttpClientHandler =
        new()
        {
            UseCookies = false,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        };

    public void UseProxyConfiguration(bool useProxy, IWebProxy proxy)
    {
        HttpClientHandler.UseProxy = useProxy;
        HttpClientHandler.Proxy = proxy;
    }

    public async Task<Results<TResponse, ErrorResultBase>> RequestAsync<TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, CancellationToken cancellationToken = default)
        where TError : ErrorResultBase where TActualRequest : ActualRequestBase where TRequest : RequestBase where TResponse : ResponseBase, new()
    {
        try
        {
            await contract.MapRequest(Option).ConfigureAwait(false);
            using var requestMessage = await contract.GenerateRequestMessageAsync(Option, cancellationToken);
            using var response = await HttpClient.SendAsync(requestMessage,
                                                  cancellationToken).ConfigureAwait(false);
            return await contract.ProcessResponseAsync(response, Option, cancellationToken).ConfigureAwait(false);
        }
        catch (InvalidOperationException ex)
        {
            return new ExceptionedErrorBase(500, ex.Message, exception: ex);
        }
    }

    public async Task<Results<TResponse, ErrorResultBase>> RequestAsync<TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, TRequest? request, CancellationToken cancellationToken = default)
        where TError : ErrorResultBase where TActualRequest : ActualRequestBase where TRequest : RequestBase where TResponse : ResponseBase, new()
    {
        try
        {
            contract.Request = request;
            await contract.MapRequest(Option).ConfigureAwait(false);
            using var requestMessage = await contract.GenerateRequestMessageAsync(Option, cancellationToken).ConfigureAwait(false);
            using var response = await HttpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
            return await contract.ProcessResponseAsync(response, Option, cancellationToken).ConfigureAwait(false);
        }

        catch (InvalidOperationException ex)
        {
            return new ExceptionedErrorBase(500, ex.Message, exception: ex);
        }
    }

    public async Task<Results<TCustomResponse, ErrorResultBase>> RequestAsync<TCustomResponse, TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, TRequest? request, CancellationToken cancellationToken = default)
        where TError : ErrorResultBase where TActualRequest : ActualRequestBase where TRequest : RequestBase where TCustomResponse : ResponseBase, new() where TResponse : ResponseBase, new()
    {
        try
        {
            contract.Request = request;
            await contract.MapRequest(Option).ConfigureAwait(false);
            using var requestMessage = await contract.GenerateRequestMessageAsync(Option, cancellationToken).ConfigureAwait(false);
            using var response = await HttpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
            return await contract.ProcessResponseAsync<TCustomResponse>(response, Option, cancellationToken).ConfigureAwait(false);
        }

        catch (InvalidOperationException ex)
        {
            return new ExceptionedErrorBase(500, ex.Message, exception: ex);
        }
    }

    public async Task<Results<TCustomResponse, ErrorResultBase>> RequestAsync<TCustomRequest, TCustomResponse, TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, TCustomRequest? request, CancellationToken cancellationToken = default)
        where TError : ErrorResultBase where TActualRequest : ActualRequestBase where TRequest : RequestBase where TResponse : ResponseBase, new() where TCustomResponse : ResponseBase, new()
    {
        try
        {
            using var requestMessage = await contract.GenerateRequestMessageAsync<TCustomRequest>(request!, Option, cancellationToken).ConfigureAwait(false);
            using var response = await HttpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
            return await contract.ProcessResponseAsync<TCustomResponse>(response, Option, cancellationToken).ConfigureAwait(false);
        }
        catch (InvalidOperationException ex)
        {
            return new ExceptionedErrorBase(500, ex.Message, exception: ex);
        }
    }

    public async Task<Results<TResponse, ErrorResultBase>> RequestAsync<TCustomRequest, TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, bool differ, TCustomRequest? request,
        ApiHandlerOption option, CancellationToken cancellationToken = default)
        where TError : ErrorResultBase where TActualRequest : ActualRequestBase where TRequest : RequestBase where TResponse : ResponseBase, new()
    {
        try
        {
            using var requestMessage = await contract.GenerateRequestMessageAsync<TCustomRequest>(request!, option, cancellationToken);
            using var response = await HttpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
            return await contract.ProcessResponseAsync(response, option, cancellationToken).ConfigureAwait(false);
        }
        catch (InvalidOperationException ex)
        {
            return new ExceptionedErrorBase(500, ex.Message, exception: ex);
        }
    }
}
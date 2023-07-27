using System.Net;
using HyPlayer.NeteaseApi.Bases;
using Kengwang.Toolkit;

namespace HyPlayer.NeteaseApi;

public class NeteaseCloudMusicApiHandler
{
    private readonly HttpClientHandler _httpClientHandler =
        new()
        {
            UseCookies = false,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        };

    public async void UseProxyConfiguration(bool useProxy, IWebProxy proxy)
    {
        _httpClientHandler.UseProxy = useProxy;
        _httpClientHandler.Proxy = proxy;
    }

    public async Task<Results<TResponse, ErrorResultBase>> RequestAsync<TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, ApiHandlerOption option,
        CancellationToken cancellationToken = default)
        where TError : ErrorResultBase where TActualRequest : ActualRequestBase where TRequest : RequestBase
    {
        var client = new HttpClient(_httpClientHandler);
        var response = await client.SendAsync(await contract.GenerateRequestMessageAsync(option, cancellationToken),
                                              cancellationToken);
        return await contract.ProcessResponseAsync(response, option, cancellationToken);
    }

    public async Task<Results<TResponse, ErrorResultBase>> RequestAsync<TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, TRequest? request,
        ApiHandlerOption option, CancellationToken cancellationToken = default)
        where TError : ErrorResultBase where TActualRequest : ActualRequestBase where TRequest : RequestBase
    {
        var client = new HttpClient(_httpClientHandler);
        contract.Request = request;
        await contract.MapRequest(request);
        var response = await client.SendAsync(await contract.GenerateRequestMessageAsync(option, cancellationToken), cancellationToken);
        return await contract.ProcessResponseAsync(response, option, cancellationToken);
    }
}
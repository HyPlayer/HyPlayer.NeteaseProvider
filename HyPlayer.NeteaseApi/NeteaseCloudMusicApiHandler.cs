﻿using System.Net;
using HyPlayer.NeteaseApi.Bases;
using Kengwang.Toolkit;

namespace HyPlayer.NeteaseApi;

public class NeteaseCloudMusicApiHandler
{
    private readonly HttpClientHandler _httpClientHandler =
        new()
        {
            UseCookies = false,
            AutomaticDecompression =  DecompressionMethods.GZip | DecompressionMethods.Deflate,
        };

    public async void UseProxyConfiguration(bool useProxy, IWebProxy proxy)
    {
        _httpClientHandler.UseProxy = useProxy;
        _httpClientHandler.Proxy = proxy;
    }
    
    public async Task<Results<TResponse, ErrorResultBase>> RequestAsync<TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, ApiHandlerOption option)
         where TError : ErrorResultBase where TActualRequest : ActualRequestBase where TRequest : RequestBase
    {
        var client = new HttpClient(_httpClientHandler);
        var response = await client.SendAsync(await contract.GenerateRequestMessageAsync(option));
        return await contract.ProcessResponseAsync(response, option);
    }
    
    public async Task<Results<TResponse, ErrorResultBase>> RequestAsync<TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, TRequest? request, ApiHandlerOption option)
        where TError : ErrorResultBase where TActualRequest : ActualRequestBase where TRequest : RequestBase
    {
        var client = new HttpClient(_httpClientHandler);
        await contract.MapRequest(request);
        var response = await client.SendAsync(await contract.GenerateRequestMessageAsync(option));
        return await contract.ProcessResponseAsync(response, option);
    }
}
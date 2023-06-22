using HyPlayer.NeteaseProvider.Bases;
using HyPlayer.NeteaseProvider.Extensions;

namespace HyPlayer.NeteaseProvider;

public class NeteaseCloudMusicApiHandler
{
    private readonly HttpClientHandler _httpClientHandler =
        new()
        {
            UseCookies = false
        };

    public async Task<Results<TResponse, ErrorResultBase>> RequestAsync<TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, ProviderOption option)
         where TError : ErrorResultBase where TActualRequest : ActualRequestBase where TRequest : RequestBase
    {
        _httpClientHandler.UseProxy = option.UseProxy;
        _httpClientHandler.Proxy = option.Proxy;
        var client = new HttpClient(_httpClientHandler);
        var response = await client.SendAsync(await contract.GenerateRequestMessageAsync(option));
        return await contract.ProcessResponseAsync(response, option);
    }
}
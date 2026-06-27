using HyPlayer.NeteaseApi.Extensions;

namespace HyPlayer.NeteaseApi.Bases.RawApiContractBases;

public abstract class RawApiContractBase<TRequest, TResponse, TError, TActualRequest> :
    ApiContractBase<TRequest, TResponse, TError, TActualRequest>
    where TActualRequest : RawApiActualRequestBase
    where TError : ErrorResultBase
    where TRequest : RequestBase
    where TResponse : ResponseBase, new()
{
    public override Task<HttpRequestMessage> GenerateRequestMessageAsync<TActualRequestModel>(
        TActualRequestModel actualRequest, ApiHandlerOption option,
        CancellationToken cancellationToken = default)
    {
        var url = ApplyHttpDegrade(Url, option);
        var requestMessage = new HttpRequestMessage(Method, url);
        ApplyXRealIp(requestMessage, option);
        ApplyUserAgent(requestMessage, option);
        ApplyMusic163Referrer(requestMessage);
        ApplyCookieHeader(requestMessage, BuildRequestCookies(option));

        if (actualRequest is RawApiActualRequestBase rr)
        {
            foreach (var dataToken in option.AdditionalParameters.DataTokens)
            {
                if (dataToken.Value is null)
                    rr.Remove(dataToken.Key);
                else
                    rr[dataToken.Key] = dataToken.Value;
            }

            requestMessage.Content = new FormUrlEncodedContent(rr);
        }

        ApplyAdditionalHeaders(requestMessage, option);
        return Task.FromResult(requestMessage);
    }

    public override async Task<Results<TResponseModel, ErrorResultBase>> ProcessResponseAsync<TResponseModel>(
        HttpResponseMessage response, ApiHandlerOption option,
        CancellationToken cancellationToken = default)
    {
        return await ProcessJsonResponseAsync<TResponseModel>(response, option).ConfigureAwait(false);
    }
}

using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Extensions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

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

    public override async Task<HttpRequestMessage> GenerateRequestMessageAsync(
        ApiHandlerOption option, CancellationToken cancellationToken = default)
    {
        return await GenerateRequestMessageAsync(ActualRequest!, option, cancellationToken).ConfigureAwait(false);
    }

    public abstract Task<HttpRequestMessage> GenerateRequestMessageAsync<TActualRequestModel>(
        TActualRequestModel actualRequest, ApiHandlerOption option, CancellationToken cancellationToken = default);

    public virtual async Task<Results<TResponse, ErrorResultBase>> ProcessResponseAsync(
        HttpResponseMessage response, ApiHandlerOption option, CancellationToken cancellationToken = default)
    {
        return await ProcessResponseAsync<TResponse>(response, option, cancellationToken).ConfigureAwait(false);
    }

    public abstract Task<Results<TResponseModel, ErrorResultBase>> ProcessResponseAsync<TResponseModel>(
        HttpResponseMessage response, ApiHandlerOption option, CancellationToken cancellationToken = default)
        where TResponseModel : ResponseBase, new();

    public abstract Task MapRequest(ApiHandlerOption option);

    protected static string ApplyHttpDegrade(string url, ApiHandlerOption option)
    {
        return option.DegradeHttp ? url.Replace("https://", "http://") : url;
    }

    protected Dictionary<string, string?> BuildRequestCookies(ApiHandlerOption option)
    {
        var cookies = option.Cookies.ToDictionary(t => t.Key, t => (string?)t.Value);
        foreach (var keyValuePair in Cookies)
        {
            cookies[keyValuePair.Key] = keyValuePair.Value;
        }

        cookies.MergeDictionary(option.AdditionalParameters.Cookies);
        return cookies;
    }

    protected static void ApplyCookieHeader(HttpRequestMessage requestMessage, Dictionary<string, string?> cookies)
    {
        if (cookies.Count > 0)
            requestMessage.Headers.Add("Cookie", string.Join("; ", cookies.Select(c => $"{c.Key}={c.Value}")));
    }

    protected void ApplyUserAgent(HttpRequestMessage requestMessage, ApiHandlerOption option, string? userAgent = null)
    {
        requestMessage.Headers.UserAgent.Clear();
        requestMessage.Headers.TryAddWithoutValidation("User-Agent",
            UserAgentHelper.GetRandomUserAgent(userAgent ?? UserAgent ?? option.UserAgent));
    }

    protected static void ApplyXRealIp(HttpRequestMessage requestMessage, ApiHandlerOption option)
    {
        if (!string.IsNullOrWhiteSpace(option.XRealIP))
            requestMessage.Headers.Add("X-Real-IP", option.XRealIP);
    }

    protected void ApplyMusic163Referrer(HttpRequestMessage requestMessage)
    {
        if (Url.Contains("music.163.com"))
            requestMessage.Headers.Referrer = new Uri("https://music.163.com");
    }

    protected static void ApplyAdditionalHeaders(HttpRequestMessage requestMessage, ApiHandlerOption option)
    {
        foreach (var additionalParametersHeader in option.AdditionalParameters.Headers)
        {
            if (requestMessage.Headers.Contains(additionalParametersHeader.Key))
                requestMessage.Headers.Remove(additionalParametersHeader.Key);
            if (additionalParametersHeader.Value is not null)
                requestMessage.Headers.TryAddWithoutValidation(additionalParametersHeader.Key,
                    additionalParametersHeader.Value);
        }
    }

    protected static string ApplyAdditionalDataTokens(string json, ApiHandlerOption option)
    {
        if (option.AdditionalParameters.DataTokens.Count == 0)
            return json;

        return ApplyDataTokens(json, option, option.AdditionalParameters.DataTokens);
    }

    protected static string ApplyDataToken(string json, ApiHandlerOption option, string key, string? value)
    {
        return ApplyDataTokens(json, option, new Dictionary<string, string?> { [key] = value });
    }

    private static string ApplyDataTokens(string json, ApiHandlerOption option, IReadOnlyDictionary<string, string?> dataTokens)
    {
        var node = JsonNode.Parse(string.IsNullOrWhiteSpace(json) ? "{}" : json) as JsonObject ?? new JsonObject();
        foreach (var dataToken in dataTokens)
        {
            if (dataToken.Value is null)
                node.Remove(dataToken.Key);
            else
                node[dataToken.Key] = dataToken.Value;
        }

        return node.ToJsonString(option.JsonSerializerOptions);
    }

    protected static ErrorResultBase? TryCreateHttpStatusError(HttpResponseMessage response)
    {
        return response.IsSuccessStatusCode
            ? null
            : new ErrorResultBase((int)response.StatusCode, $"请求返回 HTTP 代码: {response.StatusCode}");
    }

    protected static void MergeSetCookies(HttpResponseMessage response, ApiHandlerOption option)
    {
        if (!response.Headers.TryGetValues("Set-Cookie", out var rawSetCookies))
            return;

        foreach (var rawSetCookie in rawSetCookies)
        {
            var arr1 = rawSetCookie.Split(';');
            var arr2 = arr1[0].TrimStart().Split('=');
            option.Cookies[arr2[0]] = arr2[1];
        }
    }

    protected static async Task<Results<TResponseModel, ErrorResultBase>> ProcessJsonResponseAsync<TResponseModel>(
        HttpResponseMessage response, ApiHandlerOption option)
        where TResponseModel : ResponseBase, new()
    {
        var statusError = TryCreateHttpStatusError(response);
        if (statusError is not null)
            return statusError;

        MergeSetCookies(response, option);
        var buffer = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        if (buffer is null || buffer.Length == 0) return new ErrorResultBase(500, "返回体预读取错误");
        var result = Encoding.UTF8.GetString(buffer);
        var ret = JsonSerializer.Deserialize<TResponseModel>(result, option.JsonSerializerOptions);
#if DEBUG
        if (ret is null)
            ret = new();
        ret.OriginalResponse = result;
#endif
        if (ret is null) return new ErrorResultBase(500, "返回 JSON 解析为空");
        if (ret is CodedResponseBase codedResponseBase && codedResponseBase.Code != 200)
            return Results<TResponseModel, ErrorResultBase>
                .CreateError(new ErrorResultBase(codedResponseBase.Code,
                    $"返回不成功({codedResponseBase.Code}): {codedResponseBase.Message}")).WithValue(ret);
        return ret;
    }
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

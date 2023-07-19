using System.Text;
using System.Text.Json;
using HyPlayer.NeteaseApi.Extensions;
using Kengwang.Toolkit;

namespace HyPlayer.NeteaseApi.Bases.ApiContractBases;

public abstract class RawApiContractBase<TRequest, TResponse, TError, TActualRequest> :
    ApiContractBase<TRequest, TResponse, TError, TActualRequest>
    where TActualRequest : RawApiActualRequestBase
    where TError : ErrorResultBase
    where TRequest : RequestBase
{
    public override Task<HttpRequestMessage> GenerateRequestMessageAsync(ApiHandlerOption option)
    {
        var url = Url;
        if (option.DegradeHttp)
            url = url.Replace("https://", "http://");
        var requestMessage = new HttpRequestMessage(Method, url);
        if (!string.IsNullOrWhiteSpace(option.XRealIP))
            requestMessage.Headers.Add("X-Real-IP", option.XRealIP);
        requestMessage.Headers.UserAgent.Clear();
        requestMessage.Headers.TryAddWithoutValidation("User-Agent", UserAgentHelper.GetRandomUserAgent(UserAgent ?? option.UserAgent));
        if (Url.Contains("music.163.com"))
            requestMessage.Headers.Referrer = new Uri("https://music.163.com");
        var cookies = option.Cookies.ToDictionary(t => t.Key, t => t.Value);
        foreach (var keyValuePair in Cookies)
        {
            cookies[keyValuePair.Key] = keyValuePair.Value;
        }

        if (cookies.Count > 0)
            requestMessage.Headers.Add("Cookie", string.Join("; ", cookies.Select(c => $"{c.Key}={c.Value}")));
        
        requestMessage.Content = new FormUrlEncodedContent(ActualRequest);
        return Task.FromResult(requestMessage);
    }

    public override async Task<Results<TResponse, ErrorResultBase>> ProcessResponseAsync(HttpResponseMessage response, ApiHandlerOption option)
    {
        if (!response.IsSuccessStatusCode)
            return new ErrorResultBase((int)response.StatusCode, $"请求返回 HTTP 代码: {response.StatusCode}");
        if (response.Headers.TryGetValues("Set-Cookie", out var rawSetCookies))
        {
            foreach (var rawSetCookie in rawSetCookies)
            {
                var arr1 = rawSetCookie.Split(';').ToList();
                var arr2 = arr1[0].TrimStart().Split('=');
                option.Cookies[arr2[0]] = arr2[1];
            }
        }

        var buffer = await response.Content.ReadAsByteArrayAsync();
        if (buffer is null || buffer.Length == 0) return new ErrorResultBase(500, "返回体预读取错误");

        var ret = JsonSerializer.Deserialize<TResponse>(Encoding.UTF8.GetString(buffer), option.JsonSerializerOptions);
        if (ret is null) return new ErrorResultBase(500, "返回 JSON 解析为空");
        if (ret is CodedResponseBase codedResponseBase && codedResponseBase.Code != 200)
            return Results<TResponse, ErrorResultBase>.CreateError(new ErrorResultBase(codedResponseBase.Code, "返回值不为 200")).WithValue(ret);
        return ret;
    }
}
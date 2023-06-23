﻿using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using HyPlayer.NeteaseProvider.Extensions;

namespace HyPlayer.NeteaseProvider.Bases.ApiContractBases;

public abstract class LinuxApiContractBase<TRequest, TResponse, TError, TActualRequest> :
    ApiContractBase<TRequest, TResponse, TError, TActualRequest>
    where TActualRequest : ActualRequestBase
    where TError : ErrorResultBase
    where TRequest : RequestBase

{
    private static readonly byte[] linuxapiKey = "rFgB&h#%2?^eDg:Q"u8.ToArray();


    public override Task<HttpRequestMessage> GenerateRequestMessageAsync(ProviderOption option)
    {
        var url = Url;
        if (option.DegradeHttp)
            url = url.Replace("https://", "http://");
        url = Regex.Replace(url, @"\w*api", "api");
        var requestMessage = new HttpRequestMessage(Method, "https://music.163.com/api/linux/forward");
        if (!string.IsNullOrWhiteSpace(option.XRealIP))
            requestMessage.Headers.Add("X-Real-IP", option.XRealIP);
        requestMessage.Headers.UserAgent.Clear();
        requestMessage.Headers.Add("User-Agent","Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.90 Safari/537.36");
        if (Url.Contains("music.163.com"))
            requestMessage.Headers.Referrer = new Uri("https://music.163.com");
        var cookies = option.Cookies.ToDictionary(t => t.Key, t => t.Value);
        foreach (var keyValuePair in Cookies)
        {
            cookies[keyValuePair.Key] = keyValuePair.Value;
        }

        requestMessage.Headers.Add("Cookie", string.Join("; ", cookies.Select(c => $"{c.Key}={c.Value}")));

        var json = JsonSerializer.Serialize(ActualRequest);
        var preData = JsonSerializer.Serialize(
            new Dictionary<string, string>()
            {
                { "method", "POST" },
                { "url", Regex.Replace(url, @"\w*api", "api") },
                { "params", json }
            });
        var data = new Dictionary<string, string>()
                   {
                       {
                           "eparams",
                           AesEncrypt(preData.ToByteArrayUtf8(), CipherMode.ECB, linuxapiKey, null).ToHexStringUpper()
                       }
                   };
        requestMessage.Content = new FormUrlEncodedContent(data);
        return Task.FromResult(requestMessage);
    }

    public override async Task<Results<TResponse, ErrorResultBase>> ProcessResponseAsync(
        HttpResponseMessage response, ProviderOption option)
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

        var ret = JsonSerializer.Deserialize<TResponse>(Encoding.UTF8.GetString(buffer));
        if (ret is null) return new ErrorResultBase(500, "返回 JSON 解析为空");
        return ret;
    }


    private static byte[] AesEncrypt(byte[] buffer, CipherMode mode, byte[] key, byte[]? iv = null)
    {
        using var aes = Aes.Create();
        aes.BlockSize = 128;
        aes.Key = key;
        if (iv is not null)
            aes.IV = iv;
        aes.Mode = mode;
        using var cryptoTransform = aes.CreateEncryptor();
        return cryptoTransform.TransformFinalBlock(buffer, 0, buffer.Length);
    }
}
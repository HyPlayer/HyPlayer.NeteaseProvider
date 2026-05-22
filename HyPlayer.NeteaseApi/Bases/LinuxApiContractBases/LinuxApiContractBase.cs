using HyPlayer.NeteaseApi.Extensions;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace HyPlayer.NeteaseApi.Bases.LinuxApiContractBases;

public abstract class LinuxApiContractBase<TRequest, TResponse, TError, TActualRequest> :
    ApiContractBase<TRequest, TResponse, TError, TActualRequest>
    where TActualRequest : LinuxApiActualRequestBase
    where TError : ErrorResultBase
    where TRequest : RequestBase
    where TResponse : ResponseBase, new()

{
    private static readonly byte[] linuxapiKey = "rFgB&h#%2?^eDg:Q"u8.ToArray();

    public override Task<HttpRequestMessage> GenerateRequestMessageAsync<TActualRequestModel>(
        TActualRequestModel actualRequest,
        ApiHandlerOption option, CancellationToken cancellationToken = default)
    {
        var url = ApplyHttpDegrade(Url, option);
        url = Regex.Replace(url, @"\w*api", "api");
        var requestMessage = new HttpRequestMessage(Method, "https://music.163.com/api/linux/forward");
        ApplyXRealIp(requestMessage, option);
        ApplyUserAgent(requestMessage, option,
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.90 Safari/537.36");
        ApplyMusic163Referrer(requestMessage);
        ApplyCookieHeader(requestMessage, BuildRequestCookies(option));

        var json = actualRequest is LinuxApiActualRequestBase la
            ? JsonSerializer.Serialize(la, option.JsonSerializerOptions)
            : string.Empty;
        var preData = JsonSerializer.Serialize(
            new Dictionary<string, string>()
            {
                { "method", "POST" },
                { "url", Regex.Replace(url, @"\w*api", "api") },
                { "params", json }
            }, option.JsonSerializerOptions);
        var data = new Dictionary<string, string>()
        {
            {
                "eparams",
                AesEncrypt(preData.ToByteArrayUtf8(), CipherMode.ECB, linuxapiKey, null).ToHexStringUpper()
            }
        };
        requestMessage.Content = new FormUrlEncodedContent(data);
        ApplyAdditionalHeaders(requestMessage, option);

        return Task.FromResult(requestMessage);
    }

    public override async Task<Results<TResponseModel, ErrorResultBase>> ProcessResponseAsync<TResponseModel>(
        HttpResponseMessage response, ApiHandlerOption option,
        CancellationToken cancellationToken = default)
    {
        return await ProcessJsonResponseAsync<TResponseModel>(response, option).ConfigureAwait(false);
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

using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using HyPlayer.NeteaseProvider.Extensions;

namespace HyPlayer.NeteaseProvider.Bases.ApiContractBases;

public abstract class
    EApiContractBase<TRequest, TResponse, TError, TActualRequest> :
        ApiContractBase<TRequest, TResponse, TError, TActualRequest>
    where TActualRequest : EApiActualRequestBase
    where TError : ErrorResultBase
    where TRequest : RequestBase
{
    public static readonly byte[] eapiKey = "e82ckenh8dichen8"u8.ToArray();
    public abstract string ApiPath { get; }

    public override Task<HttpRequestMessage> GenerateRequestMessageAsync(ProviderOption option)
    {
        var url = Regex.Replace(Url, @"\w*api", "eapi");
        if (option.DegradeHttp)
            url = url.Replace("https://", "http://");
        var requestMessage = new HttpRequestMessage(Method, url);
        if (Url.Contains("music.163.com"))
            requestMessage.Headers.Referrer = new Uri("https://music.163.com");
        if (!string.IsNullOrWhiteSpace(option.XRealIP))
            requestMessage.Headers.Add("X-Real-IP", option.XRealIP);
        requestMessage.Headers.UserAgent.Clear();
        requestMessage.Headers.Add("User-Agent",UserAgentHelper.GetRandomUserAgent(UserAgent ?? option.UserAgent) );
        var cookies = option.Cookies.ToDictionary(t => t.Key, t => t.Value);
        foreach (var keyValuePair in Cookies)
        {
            cookies[keyValuePair.Key] = keyValuePair.Value;
        }

        var csrfToken = cookies.GetValueOrDefault("__csrf");
        var subDateTime = DateTime.Now.Subtract(new DateTime(1970, 1, 1));
        var dataHeader = new Dictionary<string, string?>()
                         {
                             { "osver", cookies.GetValueOrDefault("osver", string.Empty) },
                             {
                                 "deviceId", cookies.GetValueOrDefault("deviceId", string.Empty)
                             }, // encrypt.base64.encode(imei + '\t02:00:00:00:00:00\t5106025eb79a5247\t70ffbaac7')
                             { "appver", cookies.GetValueOrDefault("appver", "8.10.10") },
                             { "versioncode", cookies.GetValueOrDefault("versioncode", "140") },
                             { "mobilename", cookies.GetValueOrDefault("mobilename", string.Empty) },
                             {
                                 "buildver",
                                 cookies.GetValueOrDefault(
                                     "buildver", subDateTime.TotalSeconds.ToString(CultureInfo.InvariantCulture))
                             },
                             { "resolution", cookies.GetValueOrDefault("resolution", "1920x1080") },
                             { "__csrf", csrfToken },
                             { "os", cookies.GetValueOrDefault("os", "android") },
                             { "channel", cookies.GetValueOrDefault("channel", string.Empty) },
                             {
                                 "requestId",
                                 $"{subDateTime.TotalMilliseconds}_{Math.Floor(new Random().NextDouble() * 1000).ToString(CultureInfo.InvariantCulture).PadLeft(4, '0')}"
                             },
                         };
        if (!string.IsNullOrEmpty(cookies.GetValueOrDefault("MUSIC_U")))
            dataHeader["MUSIC_U"] = cookies.GetValueOrDefault("MUSIC_U");
        if (!string.IsNullOrEmpty(cookies.GetValueOrDefault("MUSIC_A")))
            dataHeader["MUSIC_A"] = cookies.GetValueOrDefault("MUSIC_A");
        ActualRequest ??= (TActualRequest)new EApiActualRequestBase();
        ActualRequest.Header = JsonSerializer.Serialize(dataHeader);


        var json = JsonSerializer.Serialize(ActualRequest);
        var encryptMessage = $"nobody{ApiPath}use{json}md5forencrypt";
        var digest = encryptMessage.ToByteArrayUtf8().ComputeMd5().ToHexStringLower();
        var requestData = $"{ApiPath}-36cd479b6b5-{json}-36cd479b6b5-{digest}";

        using var aes = Aes.Create();
        aes.BlockSize = 128;
        aes.Key = eapiKey;
        aes.Mode = CipherMode.ECB;
        using var cryptoTransform = aes.CreateDecryptor();
        var reqDataBytes = requestData.ToByteArrayUtf8();
        var reqBytes = cryptoTransform.TransformFinalBlock(reqDataBytes, 0, reqDataBytes.Length);
        requestMessage.Content = new FormUrlEncodedContent(new Dictionary<string, string>()
                                                           { { "params", reqBytes.ToHexStringUpper() } });
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
        try
        {
            if (buffer[0] != 0x7B && buffer[1] != 0x22)
            {
                using var aes = Aes.Create();
                aes.BlockSize = 128;
                aes.Key = eapiKey;
                aes.Mode = CipherMode.ECB;
                using var decryptor = aes.CreateDecryptor();
                buffer = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);
            }

            try
            {
                var ret = JsonSerializer.Deserialize<TResponse>(Encoding.UTF8.GetString(buffer));
                if (ret is null) return new ErrorResultBase(500, "返回 JSON 解析为空");
                return ret;
            }
            catch
            {
                // 别问我为什么这么难看, 原作者就是这么写的, 我也不知道有啥用
                // 他确实就是这样跑了两遍, 我也只能这么抄过来了
                using var aes = Aes.Create();
                aes.BlockSize = 128;
                aes.Key = eapiKey;
                aes.Mode = CipherMode.ECB;
                using var decryptor = aes.CreateDecryptor();
                buffer = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);
                var ret = JsonSerializer.Deserialize<TResponse>(Encoding.UTF8.GetString(buffer));
                if (ret is null) return new ErrorResultBase(500, "返回 JSON 解析为空");
                return ret;
            }
        }
        catch (Exception e)
        {
            return new ExceptionedErrorBase(500, e.Message, e);
        }
    }

    class KeyValuePairKeyComparer : IEqualityComparer<KeyValuePair<string, string>>
    {
        public bool Equals(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
        {
            return x.Key == y.Key;
        }

        public int GetHashCode(KeyValuePair<string, string> obj)
        {
            return obj.Key.GetHashCode();
        }
    }
}
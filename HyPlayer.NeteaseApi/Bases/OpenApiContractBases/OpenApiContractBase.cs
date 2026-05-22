using HyPlayer.NeteaseApi.Extensions;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace HyPlayer.NeteaseApi.Bases.OpenApiContractBases;

public abstract class OpenApiContractBase<TRequest, TResponse, TError, TActualRequest> :
    ApiContractBase<TRequest, TResponse, TError, TActualRequest>
    where TActualRequest : OpenApiActualRequestBase
    where TError : ErrorResultBase
    where TRequest : RequestBase
    where TResponse : ResponseBase, new()
{
    private const string SignType = "RSA_SHA256";

    public override Task<HttpRequestMessage> GenerateRequestMessageAsync<TActualRequestModel>(
        TActualRequestModel actualRequest, ApiHandlerOption option, CancellationToken cancellationToken = default)
    {
        var url = ApplyHttpDegrade(Url, option);

        var requestParameters = BuildRequestParameters(actualRequest, option);
        var requestMessage = Method == HttpMethod.Get
            ? new HttpRequestMessage(Method, AppendQueryString(url, requestParameters))
            : new HttpRequestMessage(Method, url)
            {
                Content = new FormUrlEncodedContent(requestParameters)
            };

        ApplyXRealIp(requestMessage, option);
        ApplyUserAgent(requestMessage, option);
        ApplyCookieHeader(requestMessage, BuildRequestCookies(option));
        ApplyAdditionalHeaders(requestMessage, option);

        return Task.FromResult(requestMessage);
    }

    public override async Task<Results<TResponseModel, ErrorResultBase>> ProcessResponseAsync<TResponseModel>(
        HttpResponseMessage response, ApiHandlerOption option,
        CancellationToken cancellationToken = default)
    {
        return await ProcessJsonResponseAsync<TResponseModel>(response, option).ConfigureAwait(false);
    }

    private static Dictionary<string, string> BuildRequestParameters<TActualRequestModel>(
        TActualRequestModel actualRequest, ApiHandlerOption option)
    {
        if (option.AdditionalParameters.OpenAPIConfig is null)
            throw new InvalidOperationException("OpenAPIConfig is required for OpenAPI requests.");
        if (string.IsNullOrWhiteSpace(option.AdditionalParameters.OpenAPIConfig.AppId))
            throw new InvalidOperationException("OpenAPIConfig.AppId is required for OpenAPI requests.");
        if (string.IsNullOrWhiteSpace(option.AdditionalParameters.OpenAPIConfig.AppSecret))
            throw new InvalidOperationException("OpenAPIConfig.AppSecret is required for OpenAPI requests.");
        if (string.IsNullOrWhiteSpace(option.AdditionalParameters.OpenAPIConfig.RsaPrivateKey))
            throw new InvalidOperationException("OpenAPIConfig.RsaPrivateKey is required for OpenAPI requests.");

        var requestParameters = actualRequest is OpenApiActualRequestBase openApiActualRequest
            ? openApiActualRequest.ToDictionary(t => t.Key, t => t.Value)
            : new Dictionary<string, string>();

        var openApiConfig = option.AdditionalParameters.OpenAPIConfig;
        var appId = openApiConfig.AppId;
        var appSecret = openApiConfig.AppSecret;
        var rsaPrivateKey = openApiConfig.RsaPrivateKey;

        requestParameters["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        requestParameters["signType"] = SignType;
        requestParameters["appId"] = appId!;
        requestParameters["appSecret"] = appSecret!;
        requestParameters["device"] = openApiConfig.DeviceInfo is null
            ? string.Empty
            : JsonSerializer.Serialize(openApiConfig.DeviceInfo,
                ApiHandlerOption.JsonSerializerOptionsOnlyTypeInfo);

        if (!requestParameters.ContainsKey("bizContent"))
            requestParameters["bizContent"] = string.Empty;

        requestParameters.Remove("sign");
        requestParameters["sign"] = RsaSha256Sign(GetSignCheckContent(requestParameters), rsaPrivateKey!);
        return requestParameters;
    }

    private static string GetSignCheckContent(Dictionary<string, string> parameters)
    {
        return string.Join("&", parameters
            .Where(t => t.Key != "sign")
            .OrderBy(t => t.Key, StringComparer.Ordinal)
            .Select(t => $"{t.Key}={t.Value}"));
    }

    private static string RsaSha256Sign(string content, string privateKey)
    {
        using var rsa = RSA.Create();
        rsa.ImportParameters(ParsePkcs8PrivateKey(privateKey));
        return Convert.ToBase64String(rsa.SignData(Encoding.UTF8.GetBytes(content), HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1));
    }

    private static string AppendQueryString(string url, Dictionary<string, string> parameters)
    {
        var separator = url.Contains('?') ? "&" : "?";
        return url + separator + string.Join("&", parameters.Select(t =>
            $"{Uri.EscapeDataString(t.Key)}={Uri.EscapeDataString(t.Value)}"));
    }

    private static RSAParameters ParsePkcs8PrivateKey(string privateKey)
    {
        var keyBytes = Convert.FromBase64String(privateKey
            .Replace("-----BEGIN PRIVATE KEY-----", string.Empty)
            .Replace("-----END PRIVATE KEY-----", string.Empty)
            .Replace("\r", string.Empty)
            .Replace("\n", string.Empty)
            .Trim());
        var reader = new DerReader(keyBytes);
        return reader.ReadSequence(_ =>
        {
            reader.ReadInteger();
            reader.ReadSequence<object?>(__ =>
            {
                reader.ReadObjectIdentifier();
                if (reader.HasData)
                    reader.ReadNull();
                return null;
            });
            var rsaPrivateKey = reader.ReadOctetString();
            var rsaReader = new DerReader(rsaPrivateKey);
            return rsaReader.ReadSequence(__ =>
            {
                rsaReader.ReadInteger();
                return new RSAParameters
                {
                    Modulus = rsaReader.ReadInteger(),
                    Exponent = rsaReader.ReadInteger(),
                    D = rsaReader.ReadInteger(),
                    P = rsaReader.ReadInteger(),
                    Q = rsaReader.ReadInteger(),
                    DP = rsaReader.ReadInteger(),
                    DQ = rsaReader.ReadInteger(),
                    InverseQ = rsaReader.ReadInteger()
                };
            });
        });
    }

    private sealed class DerReader
    {
        private readonly byte[] _data;
        private int _position;

        public DerReader(byte[] data)
        {
            _data = data;
        }

        public bool HasData => _position < _data.Length;

        public T ReadSequence<T>(Func<DerReader, T> read)
        {
            var value = ReadTag(0x30);
            var reader = new DerReader(value);
            return read(reader);
        }

        public byte[] ReadInteger()
        {
            var value = ReadTag(0x02);
            var offset = value.Length > 1 && value[0] == 0 ? 1 : 0;
            var result = new byte[value.Length - offset];
            Buffer.BlockCopy(value, offset, result, 0, result.Length);
            return result;
        }

        public void ReadObjectIdentifier()
        {
            ReadTag(0x06);
        }

        public void ReadNull()
        {
            ReadTag(0x05);
        }

        public byte[] ReadOctetString()
        {
            return ReadTag(0x04);
        }

        private byte[] ReadTag(byte expectedTag)
        {
            if (_position >= _data.Length || _data[_position++] != expectedTag)
                throw new InvalidOperationException("Invalid PKCS#8 private key format.");

            var length = ReadLength();
            if (_position + length > _data.Length)
                throw new InvalidOperationException("Invalid PKCS#8 private key format.");

            var value = new byte[length];
            Buffer.BlockCopy(_data, _position, value, 0, length);
            _position += length;
            return value;
        }

        private int ReadLength()
        {
            if (_position >= _data.Length)
                throw new InvalidOperationException("Invalid PKCS#8 private key format.");

            var length = _data[_position++];
            if ((length & 0x80) == 0)
                return length;

            var byteCount = length & 0x7f;
            if (byteCount == 0 || byteCount > 4 || _position + byteCount > _data.Length)
                throw new InvalidOperationException("Invalid PKCS#8 private key format.");

            var result = 0;
            for (var i = 0; i < byteCount; i++)
            {
                result = (result << 8) | _data[_position++];
            }

            return result;
        }
    }
}

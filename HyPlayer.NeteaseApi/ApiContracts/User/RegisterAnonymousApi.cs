using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Category;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts
{

public static partial class NeteaseApis
{
    public static RegisterAnounymousApi RegisterAnounymousApi => new();
}
}


namespace HyPlayer.NeteaseApi.ApiContracts.Category
{

    public class RegisterAnounymousApi : WeApiContractBase<RegisterAnounymousRequest, RegisterAnounymousResponse, ErrorResultBase, RegisterAnounymousActualRequest>
    {
        public override string IdentifyRoute => "/register/anonimous";
        public override string Url { get; protected set; } = "https://music.163.com/weapi/register/anonimous";
        public override HttpMethod Method => HttpMethod.Post;
        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new RegisterAnounymousActualRequest()
                {
                    Username = EncodeId(Request.DeviceId)
                };
            return Task.CompletedTask;
        }
        
        private const string ID_XOR_KEY_1 = "3go8&$8*3*3h0k(2)2";
        public static string EncodeId(string someId)
        {
            var xoredString = new StringBuilder();
            for (var i = 0; i < someId.Length; i++)
            {
                var xorChar = (char)(someId[i] ^ ID_XOR_KEY_1[i % ID_XOR_KEY_1.Length]);
                xoredString.Append(xorChar);
            }
            using var md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(xoredString.ToString());
            var hashBytes = md5.ComputeHash(inputBytes);
            var enc = Convert.ToBase64String(hashBytes);
            var payload = $"{someId} {enc}";
            var payloadBytes = Encoding.UTF8.GetBytes(payload);
            return Convert.ToBase64String(payloadBytes);
        }
    }

    public class RegisterAnounymousRequest : RequestBase
    {
        public required string DeviceId { get; set; }
    }

    public class RegisterAnounymousResponse : CodedResponseBase
    {
        
    }

    public class RegisterAnounymousActualRequest : WeApiActualRequestBase
    {
        [JsonPropertyName("username")] public required string Username { get; set; }
    }
}
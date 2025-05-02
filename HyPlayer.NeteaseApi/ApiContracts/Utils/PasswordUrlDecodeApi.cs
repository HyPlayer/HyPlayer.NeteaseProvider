using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Category;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        public static PasswordUrlDecodeApi PasswordUrlDecodeApi => new();
    }
}


namespace HyPlayer.NeteaseApi.ApiContracts.Category
{

    public class PasswordUrlDecodeApi : EApiContractBase<PasswordUrlDecodeRequest, PasswordUrlDecodeResponse, ErrorResultBase, PasswordUrlDecodeActualRequest>
    {
        public override string IdentifyRoute => "/password/url/decode";
        public override string Url { get; protected set; } = "https://interface3.music.163.com/eapi/password/url/decode/get";
        public override HttpMethod Method => HttpMethod.Post;

        public override Task MapRequest()
        {
            if (Request is not null)
                ActualRequest = new PasswordUrlDecodeActualRequest
                {
                    EncodedPassword = Request.EncodedPassword
                };
            return Task.CompletedTask;
        }

        public override string ApiPath { get; protected set; } = "/api/password/url/decode/get";
    }

    public class PasswordUrlDecodeRequest : RequestBase
    {
        public required string EncodedPassword { get; set; }
    }

    public class PasswordUrlDecodeResponse : CodedResponseBase
    {

    }

    public class PasswordUrlDecodeActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("encodedPassword")] public required string EncodedPassword { get; set; }
    }
}
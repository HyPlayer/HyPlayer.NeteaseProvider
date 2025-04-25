using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Extensions;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts.Login;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        /// <summary>
        /// 手机登录
        /// </summary>
        public static LoginCellphoneApi LoginCellphoneApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Login
{

    public class LoginCellphoneApi : EApiContractBase<LoginCellphoneRequest, LoginResponse, ErrorResultBase,
        LoginCellphoneActualRequest>
    {
        public override string IdentifyRoute => "/login/cellphone";
        public override string ApiPath { get; protected set; } = "/api/w/login/cellphone";
        public override string Url { get; protected set; } = "https://interface..163.com/eapi/w/login/cellphone";
        public override HttpMethod Method => HttpMethod.Post;

        public override Dictionary<string, string> Cookies => new() { { "os", "pc" }, { "appver", "2.9.8" } };

        public override string UserAgent => "pc";

        public override Task MapRequest()
        {
            if (Request is null) return Task.CompletedTask;
            var md5Password = string.IsNullOrEmpty(Request.Md5Password)
                ? Request.Password!.ToByteArrayUtf8().ComputeMd5().ToHexStringLower()
                : Request.Md5Password!;
            ActualRequest = new LoginCellphoneActualRequest
            {
                Phone = Request.Cellphone,
                CountryCode = Request.CountryCode,
                Md5Password = md5Password
            };
            return Task.CompletedTask;
        }

        public override async Task<Results<LoginResponse, ErrorResultBase>> ProcessResponseAsync(
            HttpResponseMessage response, ApiHandlerOption option, CancellationToken cancellationToken = default)
        {
            return (await base.ProcessResponseAsync(response, option, cancellationToken).ConfigureAwait(false))
                .Match(
                    (success) => success.Code != 200 ? new ErrorResultBase(success.Code, success.Message) : success,
                    Results<LoginResponse, ErrorResultBase>.CreateError
                );
        }
    }

    public class LoginCellphoneActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("phone")] public required string Phone { get; set; }
        [JsonPropertyName("countrycode")] public string CountryCode { get; set; } = "86";
        [JsonPropertyName("password")] public required string Md5Password { get; set; }
        [JsonPropertyName("rememberLogin")] public bool RememberLogin => true;
    }

    public class LoginCellphoneRequest : RequestBase
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public required string Cellphone { get; set; }

        /// <summary>
        /// 国家代码
        /// </summary>
        public string CountryCode { get; set; } = "86";

        /// <summary>
        /// 密码
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// 密码 (MD5 加密)
        /// </summary>
        public string? Md5Password { get; set; }
    }

    public class LoginResponse : CodedResponseBase
    {
        [JsonPropertyName("loginType")] public int LoginType { get; set; }
        [JsonPropertyName("profile")] public UserInfoDto? Profile { get; set; }
    }
}
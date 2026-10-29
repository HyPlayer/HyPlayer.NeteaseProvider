using HyPlayer.NeteaseApi.ApiContracts.Utils;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.RawApiContractBases;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{
    public static partial class NeteaseApis
    {
        public static PcClientLatestVersionApi PcClientLatestVersionApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.Utils
{
    public class PcClientLatestVersionApi : RawApiContractBase<PcClientLatestVersionRequest, PcClientLatestVersionResponse, ErrorResultBase, PcClientLatestVersionActualRequest>
    {
        public override string IdentifyRoute => "/pc/package/download/latest";
        public override string Url { get; protected set; } = "https://music.163.com/api/pc/package/download/latest?productName=music";
        public override HttpMethod Method => HttpMethod.Get;

        public override Task MapRequest(ApiHandlerOption option)
        {
            return Task.CompletedTask;
        }
    }

    public class PcClientLatestVersionRequest : RequestBase
    {
    }

    public class PcClientLatestVersionResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public PcClientLatestVersionResponseData? Data { get; set; }

        public class PcClientLatestVersionResponseData
        {
            [JsonPropertyName("appVer")] public string? AppVer { get; set; }
            [JsonPropertyName("buildVer")] public string? BuildVer { get; set; }
        }
    }

    public class PcClientLatestVersionActualRequest : RawApiActualRequestBase
    {
    }
}

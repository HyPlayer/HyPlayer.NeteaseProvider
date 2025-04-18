﻿using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static DjChannelSubscribedApi DjChannelSubscribedApi => new();
}

public class DjChannelSubscribedApi : EApiContractBase<DjChannelSubscribedRequest, DjChannelSubscribedResponse,
    ErrorResultBase, DjChannelSubscribedActualRequest>
{
    public override string IdentifyRoute => "/dj/sublist";

    public override string Url { get; protected set; } =
        "https://interface.music.163.com/eapi/social/my/subscribed/voicelist/v1";

    public override string ApiPath { get; protected set; } = "/api/social/my/subscribed/voicelist/v1";

    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest()
    {
        if (Request is not null)
            ActualRequest = new DjChannelSubscribedActualRequest
            {
                Limit = Request.Limit
            };
        return Task.CompletedTask;
    }
}

public class DjChannelSubscribedRequest : RequestBase
{
    public int Limit { get; set; } = 200;
}

public class DjChannelSubscribedResponse : CodedResponseBase
{
    [JsonPropertyName("data")] public DjChannelSubscribedResponseData? Data { get; set; }

    public class DjChannelSubscribedResponseData
    {
        [JsonPropertyName("count")] public int Count { get; set; }
        [JsonPropertyName("hasMore")] public bool HasMore { get; set; }
        [JsonPropertyName("data")] public DjVoiceChannelDto[]? Data { get; set; }
    }
}

public class DjChannelSubscribedActualRequest : EApiActualRequestBase
{
    [JsonPropertyName("limit")] public int Limit { get; set; } = 200;
}
﻿using HyPlayer.NeteaseApi.ApiContracts.PersonalFM;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.ApiContracts
{

    public static partial class NeteaseApis
    {
        /// <summary>
        /// 私人 FM
        /// </summary>
        public static PersonalFmApi PersonalFmApi => new();
    }
}

namespace HyPlayer.NeteaseApi.ApiContracts.PersonalFM
{

    public class PersonalFmApi : EApiContractBase<PersonalFmRequest, PersonalFmResponse, ErrorResultBase,
        PersonalFmActualRequest>
    {
        public override string IdentifyRoute => "/personal/fm";
        public override string Url { get; protected set; } = "https://interface3.music.163.com/eapi/v1/radio/get";
        public override HttpMethod Method => HttpMethod.Post;

        public override string ApiPath { get; protected set; } = "/api/v1/radio/get";

        public override Task MapRequest(ApiHandlerOption option)
        {
            if (Request is not null)
                ActualRequest = new PersonalFmActualRequest
                {
                    Mode = Request.Mode,
                    Limit = Request.Limit
                };
            return Task.CompletedTask;
        }
    }

    public class PersonalFmRequest : RequestBase
    {
        public string Mode { get; set; } =
            "FAMILIAR"; // aidj, DEFAULT, FAMILIAR, EXPLORE, SCENE_RCMD ( EXERCISE, FOCUS, NIGHT_EMO  )

        public string? SubMode { get; set; } = null;
        public int Limit { get; set; } = 3;
    }

    public class PersonalFmResponse : CodedResponseBase
    {
        [JsonPropertyName("data")] public PersonalFmDataItem[]? Items { get; set; }

        public class PersonalFmDataItem : SongDto
        {
            [JsonPropertyName("reason")] public string? RecommendedReason { get; set; }
        }
    }

    public class PersonalFmActualRequest : EApiActualRequestBase
    {
        [JsonPropertyName("mode")] public string Mode { get; set; } = "FAMILIAR";
        [JsonPropertyName("subMode")] public string? SubMode { get; set; } = null;
        [JsonPropertyName("limit")] public int Limit { get; set; } = 3;
    }
}
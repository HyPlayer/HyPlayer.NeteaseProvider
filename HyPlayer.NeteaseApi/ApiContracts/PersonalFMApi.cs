using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    /// <summary>
    /// 私人 FM
    /// </summary>
    public static PersonalFmApi PersonalFmApi = new();
}

public class PersonalFmApi : WeApiContractBase<PersonalFmRequest, PersonalFmResponse, ErrorResultBase, PersonalFmActualRequest>
{
    public override string Url => "https://music.163.com/weapi/v1/radio/get"; 
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest(PersonalFmRequest? request)
    {
        return Task.CompletedTask;
    }
}

public class PersonalFmRequest : RequestBase
{

}

public class PersonalFmResponse : CodedResponseBase
{
    [JsonPropertyName("data")] public PersonalFmDataItem[]? Items { get; set; }

    public class PersonalFmDataItem : SongDto
    {
        [JsonPropertyName("reason")] public string? RecommendedReason { get; set; }
    }
}

public class PersonalFmActualRequest : WeApiActualRequestBase
{

}
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts;

public class ToplistApi : RawApiContractBase<ToplistRequest,ToplistResponse, ErrorResultBase,ToplistActualRequest  >
{
    public override string Url => "https://music.163.com/api/toplist";
    public override HttpMethod Method => HttpMethod.Get;
    public override Task MapRequest(ToplistRequest? request)
    {
        return Task.CompletedTask;
    }
}

public class ToplistRequest : RequestBase
{
    
}

public class ToplistResponse : CodedResponseBase
{
    
    [JsonPropertyName("list")] public UserPlaylistResponse.PlaylistItem[]? List { get; set; }
}

public class ToplistActualRequest : RawApiActualRequestBase
{
    
}
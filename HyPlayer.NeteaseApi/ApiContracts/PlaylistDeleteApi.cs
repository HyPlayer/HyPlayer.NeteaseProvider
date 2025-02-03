﻿using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts;

public static partial class NeteaseApis
{
    public static PlaylistDeleteApi PlaylistDeleteApi => new();
}

public class PlaylistDeleteApi : WeApiContractBase<PlaylistDeleteRequest, PlaylistDeleteResponse, ErrorResultBase, PlaylistDeleteActualRequest>
{
    public override string IdentifyRoute => "/playlist/delete";
    public override string Url => "https://music.163.com/weapi/playlist/remove";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest(PlaylistDeleteRequest? request)
    {
        if (request is not null)
            ActualRequest = new PlaylistDeleteActualRequest
            {
                Ids = request.ConvertToIdStringList()
            };
        return Task.CompletedTask;
    }
}

public class PlaylistDeleteRequest : IdOrIdListListRequest
{

}

public class PlaylistDeleteResponse : CodedResponseBase
{

}

public class PlaylistDeleteActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("ids")] public required string Ids { get; set; }
}
using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseApi.Models.ResponseModels;

namespace HyPlayer.NeteaseApi.ApiContracts;

public class UserPlaylistApi : WeApiContractBase<UserPlaylistRequest, UserPlaylistResponse, ErrorResultBase,
    UserPlaylistActualRequest>
{
    public override string Url => "https://music.163.com/api/user/playlist";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest(UserPlaylistRequest? request)
    {
        if (request is not null)
            ActualRequest = new UserPlaylistActualRequest
                            {
                                Uid = request.Uid,
                                Limit = request.Limit,
                                Offset = request.Offset
                            };
        return Task.CompletedTask;
    }
}

public class UserPlaylistRequest : RequestBase
{
    public required string Uid { get; set; }
    public int Limit { get; set; } = 30;
    public int Offset { get; set; } = 0;
}

public class UserPlaylistResponse : CodedResponseBase
{
    [JsonPropertyName("playlist")] public List<PlaylistDto>? Playlists { get; set; }
}

public class UserPlaylistActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("uid")] public string Uid { get; set; }
    [JsonPropertyName("limit")] public int Limit { get; set; } = 30;
    [JsonPropertyName("offset")] public int Offset { get; set; }
    [JsonPropertyName("includeVideo")] public bool IncludeVideo => true;
}
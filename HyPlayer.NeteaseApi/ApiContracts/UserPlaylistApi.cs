using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts;

public class UserPlaylistApi : WeApiContractBase<UserPlaylistRequest,UserPlaylistResponse,ErrorResultBase, UserPlaylistActualRequest>
{
    public override string Url => "https://music.163.com/api/user/playlist";
    public override HttpMethod Method => HttpMethod.Post;
    public override async Task MapRequest(UserPlaylistRequest request)
    {
        ActualRequest = new UserPlaylistActualRequest
                        {
                            Uid = request.Uid,
                            Limit = request.Limit,
                            Offset = request.Offset
                        };
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
    [JsonPropertyName("playlist")] public List<PlaylistItem>? Playlists { get; set; }


    public class PlaylistItem
    {
        [JsonPropertyName("creator")] public LoginResponse.ProfileData? Creator { get; set; }
        [JsonPropertyName("privacy")] public int Privacy { get; set; }
        [JsonPropertyName("trackCount")] public int TrackCount { get; set; }
        [JsonPropertyName("coverImgUrl")] public string? CoverUrl { get; set; }
        [JsonPropertyName("playCount")] public int PlayCount { get; set; }
        [JsonPropertyName("subscribedCount")] public int SubscribedCount { get; set; }
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("id")] public string? Id { get; set; }
        [JsonPropertyName("updateTime")] public long UpdateTime { get; set; }
        [JsonPropertyName("description")] public string? Description { get; set; }
    }
}

public class UserPlaylistActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("uid")] public string Uid { get; set; }
    [JsonPropertyName("limit")] public int Limit { get; set; } = 30;
    [JsonPropertyName("offset")] public int Offset { get; set; }
    [JsonPropertyName("includeVideo")] public bool IncludeVideo  => true;
}
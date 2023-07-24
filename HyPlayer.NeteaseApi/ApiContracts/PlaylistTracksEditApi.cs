using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts;

public class PlaylistTracksEditApi : WeApiContractBase<PlaylistTracksEditRequest, PlaylistTracksEditResponse, ErrorResultBase, PlaylistTracksEditActualRequest>
{
    public override string Url => "https://music.163.com/weapi/playlist/manipulate/tracks";
    public override HttpMethod Method => HttpMethod.Post;
    public override Task MapRequest(PlaylistTracksEditRequest? request)
    {
        if (request is null) return Task.CompletedTask;
        var trackIds = request.TrackIds is not null ? $"[{string.Join(",", request.TrackIds)}]" : request.TrackId!;

        ActualRequest = new PlaylistTracksEditActualRequest
                        {
                            Operation = request.IsAdd ? "add" : "del",
                            PlaylistId = request.PlaylistId,
                            TrackIds = trackIds
                        };
        return Task.CompletedTask;
    }
}

public class PlaylistTracksEditActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("op")] public required string Operation { get; set; }
    [JsonPropertyName("pid")] public required string PlaylistId { get; set; }
    [JsonPropertyName("trackIds")] public required string TrackIds { get; set; }
    [JsonPropertyName("imme")] public bool Imme => true;

}

public class PlaylistTracksEditRequest : RequestBase
{
    public bool IsAdd { get; set; } = true;
    public required string PlaylistId { get; set; }
    public string? TrackId { get; set; }
    public string[]? TrackIds { get; set; }
}
public class PlaylistTracksEditResponse : CodedResponseBase
{
    
}
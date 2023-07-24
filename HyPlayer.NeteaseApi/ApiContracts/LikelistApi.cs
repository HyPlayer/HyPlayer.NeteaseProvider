using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;

namespace HyPlayer.NeteaseApi.ApiContracts;

public class LikelistApi : WeApiContractBase<LikelistRequest, LikelistResponse, ErrorResultBase, LikelistActualRequest>
{
    public override string Url => "https://music.163.com/weapi/song/like/get";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest(LikelistRequest? request)
    {
        if (request is not null)
            ActualRequest = new LikelistActualRequest
                            {
                                Uid = request.Uid
                            };
        return Task.CompletedTask;
    }
}

public class LikelistRequest : RequestBase
{
    public required string Uid { get; set; }
}

public class LikelistResponse : CodedResponseBase
{
    [JsonPropertyName("ids")] public string[] TrackIds { get; set; }
    [JsonPropertyName("checkPoint")] public long CheckPoint { get; set; }
}

public class LikelistActualRequest : WeApiActualRequestBase
{
    [JsonPropertyName("uid")] public required string Uid { get; set; }
}
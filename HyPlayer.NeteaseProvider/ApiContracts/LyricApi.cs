using HyPlayer.NeteaseProvider.ActualRequests;
using HyPlayer.NeteaseProvider.Bases;
using HyPlayer.NeteaseProvider.Bases.ApiContractBases;
using HyPlayer.NeteaseProvider.Requests;
using HyPlayer.NeteaseProvider.Responses;

namespace HyPlayer.NeteaseProvider.ApiContracts;

public class LyricApi : EApiContractBase<LyricRequest, LyricResponse, ErrorResultBase, LyricActualRequest>
{
    public override string Url => "https://interface3.music.163.com/eapi/song/lyric/v1";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest(LyricRequest request)
    {
        ActualRequest = new()
                        {
                            Id = request.Id
                        };
        return Task.CompletedTask;
    }

    public override string ApiPath => "/api/song/lyric/v1";
}
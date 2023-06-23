using HyPlayer.NeteaseProvider.ActualRequests;
using HyPlayer.NeteaseProvider.Bases;
using HyPlayer.NeteaseProvider.Bases.ApiContractBases;
using HyPlayer.NeteaseProvider.Requests;
using HyPlayer.NeteaseProvider.Responses;

namespace HyPlayer.NeteaseProvider.ApiContracts;

public class PlaylistDetailApi : RawApiContractBase<PlaylistDetailRequest, PlaylistDetailResponse, ErrorResultBase,
    PlaylistDetailActualRequest>
{
    public override string Url => "https://music.163.com/api/v6/playlist/detail";
    public override HttpMethod Method => HttpMethod.Post;

    public override Task MapRequest(PlaylistDetailRequest request)
    {
        ActualRequest = new()
                        {
                            {"id", request.Id},
                            {"n", "100000"},
                            {"s", "8"}
                        };
        return Task.CompletedTask;
    }
}
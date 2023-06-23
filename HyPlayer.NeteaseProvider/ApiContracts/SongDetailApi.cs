using HyPlayer.NeteaseProvider.ActualRequests;
using HyPlayer.NeteaseProvider.Bases;
using HyPlayer.NeteaseProvider.Bases.ApiContractBases;
using HyPlayer.NeteaseProvider.Requests;
using HyPlayer.NeteaseProvider.Responses;

namespace HyPlayer.NeteaseProvider.ApiContracts;

public class SongDetailApi : WeApiContractBase<SongDetailRequest, SongDetailResponse, ErrorResultBase, SongDetailActualRequest>
{
    public override string Url => "https://music.163.com/weapi/v3/song/detail";
    public override HttpMethod Method => HttpMethod.Post;
    public override Task MapRequest(SongDetailRequest request)
    {
        var requestIds = string.IsNullOrWhiteSpace(request.Id)
            ? $"[{string.Join(",", request.IdList.Select(id => $$"""{"id":'{{id}}'}"""))}]"
            : $$"""[{"id": '{{request.Id}}'}]""";
        ActualRequest = new SongDetailActualRequest { Ids = requestIds };
        return Task.CompletedTask;
    }
}
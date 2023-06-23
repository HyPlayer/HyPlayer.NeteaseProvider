using HyPlayer.NeteaseProvider.ActualRequests;
using HyPlayer.NeteaseProvider.Bases;
using HyPlayer.NeteaseProvider.Bases.ApiContractBases;
using HyPlayer.NeteaseProvider.Requests;
using HyPlayer.NeteaseProvider.Responses;

namespace HyPlayer.NeteaseProvider.ApiContracts;

public class SongUrlApi : EApiContractBase<SongUrlRequest, SongUrlResponse, ErrorResultBase, SongUrlActualRequest>
{
    public override string Url => "https://interface.music.163.com/eapi/song/enhance/player/url/v1";
    public override HttpMethod Method => HttpMethod.Post;

    public override async Task MapRequest(SongUrlRequest request)
    {
        var ids = string.IsNullOrWhiteSpace(request.Id) ? $"[{string.Join(",", request.IdList!)}]" : $"[{request.Id}]";
        ActualRequest = new SongUrlActualRequest
                        {
                            Ids = ids,
                            Level = request.Level
                        };
    }

    public override Dictionary<string, string> Cookies =>
        new()
        {
            { "os", "android" },
            { "appver", "8.10.05" }
        };

    public override string ApiPath => "/api/song/enhance/player/url/v1";
}
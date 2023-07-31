using HyPlayer.NeteaseApi;
using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.PlayCore.Abstraction.Interfaces.PlayListContainer;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseSearchContainer : LinerContainerBase, IProgressiveLoadingContainer
{
    public override string ProviderId => "ncm";
    public override string TypeId => "se"; // search


    public required int SearchTypeId { get; set; }
    public required string SearchKeyword { get; set; }

    public NeteaseSearchContainer()
    {
        Name = "搜索结果";
    }

    public override async Task<List<ProvidableItemBase>> GetAllItems()
    {
        return (await GetProgressiveItemsList(0, MaxProgressiveCount)).Item2;
    }

    public async Task<(bool, List<ProvidableItemBase>)> GetProgressiveItemsList(int start, int count)
    {
        switch (SearchTypeId)
        {
            case 1:
                var result = await NeteaseProvider.Instance
                                                  .RequestAsync<SearchSongResponse, SearchRequest, SearchResponse,
                                                      ErrorResultBase, SearchActualRequest>(
                                                      NeteaseApis.SearchApi,
                                                      new SearchRequest
                                                      {
                                                          Keyword = SearchKeyword,
                                                          Type = SearchTypeId,
                                                          Limit = count,
                                                          Offset = start
                                                      });
                return result.Match(success => (success.Result?.Count > start + count, success.Result?.Items
                                                    ?.Select(t => (ProvidableItemBase)t.MapToNeteaseMusic())
                                                    .ToList() ?? new()),
                                    _ => (false, new()));
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public int MaxProgressiveCount => 30;
}
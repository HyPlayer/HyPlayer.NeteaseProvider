using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Models;
using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.PlayCore.Abstraction.Interfaces.PlayListContainer;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseSearchContainer : LinerContainerBase, IProgressiveLoadingContainer
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.SearchResult; // search


    public required NeteaseResourceType SearchTypeId { get; set; }
    public required string SearchKeyword { get; set; }

    public NeteaseSearchContainer()
    {
        Name = "搜索结果";
    }

    public override async Task<List<ProvidableItemBase>> GetAllItemsAsync(CancellationToken ctk = new())
    {
        return (await GetProgressiveItemsListAsync(0, MaxProgressiveCount, ctk)).Item2;
    }

    public async Task<(bool, List<ProvidableItemBase>)> GetProgressiveItemsListAsync(int start, int count, CancellationToken ctk = new())
    {
        switch (SearchTypeId)
        {
            case NeteaseResourceType.Song:
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
                                                      }, ctk);
                return result.Match(success => (success.Result?.Count > start + count, success.Result?.Items
                                                    ?.Select(t => (ProvidableItemBase)t.MapToNeteaseMusic())
                                                    .ToList() ?? new()),
                                    _ => (false, new()));
            case NeteaseResourceType.MV:
                var mvResult = await NeteaseProvider.Instance
                                                     .RequestAsync<SearchMVResponse, SearchRequest, SearchResponse,
                                                         ErrorResultBase, SearchActualRequest>(
                                                         NeteaseApis.SearchApi,
                                                         new SearchRequest
                                                         {
                                                             Keyword = SearchKeyword,
                                                             Type = SearchTypeId,
                                                             Limit = count,
                                                             Offset = start
                                                         }, ctk);
                return mvResult.Match(success => (success.Result?.Count > start + count, success.Result?.Items
                                                     ?.Select(t => (ProvidableItemBase)t.MapToNeteaseMV())
                                                     .ToList() ?? new()),
                                     _ => (false, new()));
            case NeteaseResourceType.Playlist:
                var playlistResult = await NeteaseProvider.Instance
                                                          .RequestAsync<SearchPlaylistResponse, SearchRequest, SearchResponse,
                                                              ErrorResultBase, SearchActualRequest>(
                                                              NeteaseApis.SearchApi,
                                                              new SearchRequest
                                                              {
                                                                  Keyword = SearchKeyword,
                                                                  Type = SearchTypeId,
                                                                  Limit = count,
                                                                  Offset = start
                                                              }, ctk);
                return playlistResult.Match(success => (success.Result?.Count > start + count, success.Result?.Items
                                                          ?.Select(t => (ProvidableItemBase)t.MapToNeteasePlaylist())
                                                          .ToList() ?? new()),
                                          _ => (false, new()));
            case NeteaseResourceType.Album:
                var albumResult = await NeteaseProvider.Instance
                                                       .RequestAsync<SearchAlbumResponse, SearchRequest, SearchResponse,
                                                           ErrorResultBase, SearchActualRequest>(
                                                           NeteaseApis.SearchApi,
                                                           new SearchRequest
                                                           {
                                                               Keyword = SearchKeyword,
                                                               Type = SearchTypeId,
                                                               Limit = count,
                                                               Offset = start
                                                           }, ctk);
                return albumResult.Match(success => (success.Result?.Count > start + count, success.Result?.Items
                                                    ?.Select(t => (ProvidableItemBase)t.MapToNeteaseAlbum())
                                                    .ToList() ?? new()),
                                    _ => (false, new()));
            case NeteaseResourceType.Artist:
                var artistResult = await NeteaseProvider.Instance
                                                        .RequestAsync<SearchArtistResponse, SearchRequest, SearchResponse,
                                                            ErrorResultBase, SearchActualRequest>(
                                                            NeteaseApis.SearchApi,
                                                            new SearchRequest
                                                            {
                                                                Keyword = SearchKeyword,
                                                                Type = SearchTypeId,
                                                                Limit = count,
                                                                Offset = start
                                                            }, ctk);
                return artistResult.Match(success => (success.Result?.Count > start + count, success.Result?.Items
                                                     ?.Select(t => (ProvidableItemBase)t.MapToNeteaseArtist())
                                                     .ToList() ?? new()),
                                     _ => (false, new()));

            case NeteaseResourceType.RadioChannel:
                var radioResult = await NeteaseProvider.Instance
                                                       .RequestAsync<SearchRadioResponse, SearchRequest, SearchResponse,
                                                           ErrorResultBase, SearchActualRequest>(
                                                           NeteaseApis.SearchApi,
                                                           new SearchRequest
                                                           {
                                                               Keyword = SearchKeyword,
                                                               Type = SearchTypeId,
                                                               Limit = count,
                                                               Offset = start
                                                           }, ctk);
                return radioResult.Match(success => (success.Result?.Count > start + count, success.Result?.Items
                                                    ?.Select(t => (ProvidableItemBase)t.MapToNeteaseRadioChannel())
                                                    .ToList() ?? new()),
                                    _ => (false, new()));
                break;
            case NeteaseResourceType.Video:
            case NeteaseResourceType.MLog:
                var videoResult = await NeteaseProvider.Instance
                                                       .RequestAsync<SearchVideoResponse, SearchRequest, SearchResponse,
                                                           ErrorResultBase, SearchActualRequest>(
                                                           NeteaseApis.SearchApi,
                                                           new SearchRequest
                                                           {
                                                               Keyword = SearchKeyword,
                                                               Type = SearchTypeId,
                                                               Limit = count,
                                                               Offset = start
                                                           }, ctk);
                return videoResult.Match(success => (success.Result?.Count > start + count, success.Result?.Items
                                                    ?.Select(t => (ProvidableItemBase)t.MapToNeteaseVideo())
                                                    .ToList() ?? new()),
                                    _ => (false, new()));
                
                break;
            case NeteaseResourceType.User:
                var userResult = await NeteaseProvider.Instance
                                                      .RequestAsync<SearchUserResponse, SearchRequest, SearchResponse,
                                                          ErrorResultBase, SearchActualRequest>(
                                                          NeteaseApis.SearchApi,
                                                          new SearchRequest
                                                          {
                                                              Keyword = SearchKeyword,
                                                              Type = SearchTypeId,
                                                              Limit = count,
                                                              Offset = start
                                                          }, ctk);
                return userResult.Match(success => (success.Result?.Count > start + count, success.Result?.Items
                                                   ?.Select(t => (ProvidableItemBase)t.MapToNeteaseUser())
                                                   .ToList() ?? new()),
                                       _ => (false, new()));
                break;
            case NeteaseResourceType.Lyric:
                 var lyricResult = await NeteaseProvider.Instance
                                                       .RequestAsync<SearchLyricResponse, SearchRequest, SearchResponse,
                                                           ErrorResultBase, SearchActualRequest>(
                                                           NeteaseApis.SearchApi,
                                                           new SearchRequest
                                                           {
                                                               Keyword = SearchKeyword,
                                                               Type = SearchTypeId,
                                                               Limit = count,
                                                               Offset = start
                                                           }, ctk);
                return lyricResult.Match(success => (success.Result?.Count > start + count, success.Result?.Items
                                                    ?.Select(t => (ProvidableItemBase)t.MapNeteaseLyricSearchItem())
                                                    .ToList() ?? new()),
                                    _ => (false, new()));
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public int MaxProgressiveCount => 30;
}
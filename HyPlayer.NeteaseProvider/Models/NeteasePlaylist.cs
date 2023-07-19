using HyPlayer.NeteaseApi;
using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.PlayCore.Abstraction.Interfaces.PlayListContainer;
using HyPlayer.PlayCore.Abstraction.Interfaces.ProvidableItem;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.Resources;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteasePlaylist : LinerContainerBase, IProgressiveLoadingContainer, IHasCover, IHasDescription, IHasCreators
{
    public override string ProviderId => "ncm";
    public override string TypeId => "pl";

    private string[]? _trackIds;
    private string? _coverUrl;
    

    public async Task UpdatePlaylistInfo()
    {
        if (_trackIds is null)
        {
            var results = await NeteaseProvider.Instance.RequestAsync(
                NeteaseApis.PlaylistDetailApi,
                new PlaylistDetailRequest
                {
                    Id = ActualId
                });
            results.Match(
                success =>
                {
                    _coverUrl = success.Playlist?.CoverUrl;
                    Description = success.Playlist?.Description;
                    if (!string.IsNullOrEmpty(success.Playlist?.Name))
                        Name = success.Playlist?.Name!;
                    
                    // TODO
                    return true;
                },error=>false);
        }
    }

    public override async Task<List<ProvidableItemBase>> GetAllItems()
    {
        throw new NotImplementedException();
    }

    public async Task<(bool, List<ProvidableItemBase>)> GetProgressiveItemsList(int start, int count)
    {
        throw new NotImplementedException();
    }

    public int MaxProgressiveCount => 200;
    public async Task<ImageResourceBase?> GetCover()
    {
        throw new NotImplementedException();
    }

    public string? Description { get; set; }
    public async Task<List<PersonBase>?> GetCreators()
    {
        throw new NotImplementedException();
    }

    public List<string>? CreatorList { get; init; }
}
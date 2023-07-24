using HyPlayer.NeteaseApi;
using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.PlayCore.Abstraction.Interfaces.PlayListContainer;
using HyPlayer.PlayCore.Abstraction.Interfaces.ProvidableItem;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.Resources;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteasePlaylist : LinerContainerBase, IProgressiveLoadingContainer, IHasCover, IHasDescription,
                               IHasCreators
{
    public override string ProviderId => "ncm";
    public override string TypeId => "pl";

    private string[]? _trackIds;
    public string? CoverUrl;
    public NeteaseUser? Creator;
    public bool Subscribed { get; set; }
    public long UpdateTime { get; set; }
    public int TrackCount { get; set; }
    public long PlayCount { get; set; }
    public long SubscribedCount { get; set; }

    public async Task UpdatePlaylistInfo()
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
                CoverUrl = success.Playlist?.CoverUrl;
                Description = success.Playlist?.Description;
                if (!string.IsNullOrEmpty(success.Playlist?.Name))
                    Name = success.Playlist?.Name!;
                _trackIds = success.Playlist?.TrackIds?.Select(t => t.Id).ToArray();
                Creator = success.Playlist?.Creator?.MapToNeteaseUser();
                CreatorList?.Clear();
                CreatorList?.Add(Creator?.Name!);
                Subscribed = success.Playlist?.Subscribed is true;
                UpdateTime = success.Playlist?.UpdateTime ?? 0;
                TrackCount = success.Playlist?.TrackCount ?? 0;
                PlayCount = success.Playlist?.PlayCount ?? 0;
                SubscribedCount = success.Playlist?.SubscribedCount ?? 0;
                return true;
            }, error => false);
    }

    public override async Task<List<ProvidableItemBase>> GetAllItems()
    {
        if (_trackIds is null)
            await UpdatePlaylistInfo();
        if (_trackIds is not null)
            return await NeteaseProvider.Instance.GetProvidableItemsRange(_trackIds.ToList());
        return new List<ProvidableItemBase>();
    }

    public async Task<(bool, List<ProvidableItemBase>)> GetProgressiveItemsList(int start, int count)
    {
        if (_trackIds is null)
            await UpdatePlaylistInfo();
        if (_trackIds is not null)
            return (start + count < _trackIds.Length,
                    await NeteaseProvider.Instance.GetProvidableItemsRange(_trackIds.ToList()));
        return (false, new List<ProvidableItemBase>());
    }

    public int MaxProgressiveCount => 200;

    public Task<ImageResourceBase?> GetCover()
    {
        return Task.FromResult<ImageResourceBase?>(
            new NeteaseImageResource
            {
                Url = CoverUrl,
            });
    }

    public string? Description { get; set; }

    public Task<List<PersonBase>?> GetCreators()
    {
        return Task.FromResult(new List<PersonBase> { Creator! })!;
    }

    public List<string>? CreatorList { get; init; } = new();
}
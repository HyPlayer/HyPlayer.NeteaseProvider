using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.ApiContracts.Playlist;
using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.PlayCore.Abstraction.Interfaces.PlayListContainer;
using HyPlayer.PlayCore.Abstraction.Interfaces.ProvidableItem;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.Resources;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteasePlaylist : LinerContainerBase, IProgressiveLoadingContainer, IHasCover, IHasDescription,
                               IHasCreators, IHasLibraryState
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.Playlist;

    private PlaylistTracksGetResponse.PlaylistWithTracksInfoDto.TrackIdItem[]? _trackIds;
    private List<ProvidableItemBase>? _allTrackDetails;
    private readonly Dictionary<(int Start, int Count), (bool HasMore, List<ProvidableItemBase> Items)> _trackPageCache = [];
    public string? CoverUrl;
    public NeteaseUser? Creator;
    public bool Subscribed { get; set; }
    public bool IsOwnedByCurrentUser => !Subscribed;
    public bool IsInCurrentUserLibrary => Subscribed;
    public long UpdateTime { get; set; }
    public int TrackCount { get; set; }
    public long PlayCount { get; set; }
    public long SubscribedCount { get; set; }
    public long CommentCount { get; set; }
    public long ShareCount { get; set; }
    public bool IsNewImported { get; set; }

    public async Task UpdatePlaylistInfoAsync(CancellationToken ctk = default)
    {
        var results = await NeteaseProvider.Instance.RequestAsync(
            NeteaseApis.PlaylistDetailApi,
            new PlaylistDetailRequest
            {
                Id = ActualId
            }, ctk);
        results.Match(
            success =>
            {
                var playlist = success.Playlists?.FirstOrDefault();
                if (playlist is null)
                    return false;

                CoverUrl = playlist.CoverUrl;
                Description = playlist.Description;
                if (!string.IsNullOrEmpty(playlist.Name))
                    Name = playlist.Name!;
                Creator = playlist.Creator?.MapToNeteaseUser();
                CreatorList?.Clear();
                CreatorList?.Add(Creator?.Name!);
                Subscribed = playlist.Subscribed is true;
                UpdateTime = playlist.UpdateTime;
                TrackCount = playlist.TrackCount;
                PlayCount = playlist.PlayCount;
                SubscribedCount = playlist.SubscribedCount;
                CommentCount = playlist.CommentCount;
                ShareCount = playlist.ShareCount;
                IsNewImported = playlist.IsNewImported;

                return true;
            }, error => false);
    }

    public async Task UpdateTrackListAsync(CancellationToken ctk = default)
    {
        if (ActualId == null) throw new ArgumentNullException();
        _trackIds = (await NeteaseProvider.Instance.RequestAsync(NeteaseApis.PlaylistTracksGetApi,
                                                                 new PlaylistTracksGetRequest()
                                                                 {
                                                                     Id = ActualId
                                                                 }, ctk)).Match(
            success => success.Playlist?.TrackIds,
            error => { return Array.Empty<PlaylistTracksGetResponse.PlaylistWithTracksInfoDto.TrackIdItem>(); });
    }

    public override async Task<List<ProvidableItemBase>> GetAllItemsAsync(CancellationToken ctk = default)
    {
        if (_allTrackDetails is not null)
            return _allTrackDetails;

        if (_trackIds is null)
            await UpdateTrackListAsync(ctk);
        if (_trackIds is { Length: > 0 })
        {
            _allTrackDetails = (await NeteaseProvider.Instance.GetSingleSongRangeByIds(_trackIds.Select(t => t.Id).ToList(), ctk))
                   .Select(t => (ProvidableItemBase)t).ToList();
            return _allTrackDetails;
        }
        return new List<ProvidableItemBase>();
    }

    public async Task<(bool, List<ProvidableItemBase>)> GetProgressiveItemsListAsync(
        int start, int count, CancellationToken ctk = default)
    {
        var cacheKey = (start, count);
        if (_trackPageCache.TryGetValue(cacheKey, out var cached))
            return (cached.HasMore, cached.Items);

        if (_trackIds is null)
            await UpdateTrackListAsync(ctk);
        if (_trackIds is not null)
        {
            if (_allTrackDetails is not null)
            {
                var cachedPage = (start + count < _allTrackDetails.Count,
                    _allTrackDetails.Skip(start).Take(count).ToList());
                _trackPageCache[cacheKey] = cachedPage;
                return cachedPage;
            }

            var page = (start + count < _trackIds.Length,
                (await NeteaseProvider.Instance.GetSingleSongRangeByIds(
                    _trackIds.Skip(start).Take(count).Select(t => t.Id).ToList(), ctk))
                .Select(t => (ProvidableItemBase)t).ToList());
            _trackPageCache[cacheKey] = page;
            return page;
        }
        return (false, new List<ProvidableItemBase>());
    }

    public int MaxProgressiveCount => 200;

    public string? Description { get; set; }

    public Task<List<PersonBase>?> GetCreatorsAsync(CancellationToken ctk = default)
    {
        return Task.FromResult(new List<PersonBase> { Creator! })!;
    }

    public Task<ResourceResultBase> GetCoverAsync(ImageResourceQualityTag? qualityTag = null, CancellationToken ctk = default)
    {
        if (qualityTag is NeteaseImageResourceQualityTag neteaseImageResourceQualityTag)
        {
            var result = new NeteaseImageResourceResult()
            {
                ExternalException = null,
                ResourceStatus = ResourceStatus.Success,
                Uri = new Uri($"{CoverUrl}?{neteaseImageResourceQualityTag.ToString()}")
            };
            return Task.FromResult(result as ResourceResultBase);
        }
        else
        {
            var result = new NeteaseImageResourceResult()
            {
                ExternalException = null,
                ResourceStatus = ResourceStatus.Success,
                Uri = new Uri($"{CoverUrl}")
            };
            return Task.FromResult(result as ResourceResultBase);
        }
    }

    public List<string>? CreatorList { get; init; } = new();
}

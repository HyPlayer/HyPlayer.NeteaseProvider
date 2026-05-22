using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.ApiContracts.Artist;
using HyPlayer.NeteaseApi.Extensions;
using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.PlayCore.Abstraction.Interfaces.PlayListContainer;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseArtistSubContainer : LinerContainerBase, IProgressiveLoadingContainer
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.Artist;

    public override async Task<List<ProvidableItemBase>> GetAllItemsAsync(CancellationToken ctk = default)
    {
        if (ActualId != null)
        {
            var items = new List<ProvidableItemBase>();
            var start = 0;
            bool hasMore;

            do
            {
                var result = await GetProgressiveItemsListAsync(start, MaxProgressiveCount, ctk);
                hasMore = result.Item1;
                items.AddRange(result.Item2);
                start += MaxProgressiveCount;
            } while (hasMore);

            return items;
        }
        else throw new ArgumentNullException();
    }

    public async Task<(bool, List<ProvidableItemBase>)> GetProgressiveItemsListAsync(int start = 0, int count = 50, CancellationToken ctk = default)
    {
        if(ActualId!= null) {
        var itemType = ActualId.Substring(0, 3);
        var artistId = ActualId.Substring(3);
        switch (itemType)
        {
            case "hot":
                if (start > 0) return (false, new List<ProvidableItemBase>());
                var hotResult = await NeteaseProvider.Instance.RequestAsync(
                    NeteaseApis.ArtistDetailApi,
                    new ArtistDetailRequest
                    {
                        ArtistId = artistId,
                        TopSong = count
                    }, ctk);
                return hotResult.Match(
                    success => (false,
                        success.HotSongs?.Select(song => (ProvidableItemBase)song.MapToNeteaseMusic()).ToList()
                        ?? new List<ProvidableItemBase>()),
                    error => (false, new List<ProvidableItemBase>()));
            case "tim":
                var resTime = await NeteaseProvider.Instance.RequestAsync(
                        NeteaseApis.ArtistSongsApi,
                        new ArtistSongsRequest
                        {
                            ArtistId = artistId,
                            Offset = start,
                            Limit = count
                        }, ctk);
                return resTime.Match(
                    success => (success.HasMore,
                        success.Songs?.Select(song => (ProvidableItemBase)song.MapToNeteaseMusic()).ToList() ?? new List<ProvidableItemBase>()),
                    error => (false, new List<ProvidableItemBase>()));
            case "alb":
                var albumResult = await NeteaseProvider.Instance.RequestAsync(
                    NeteaseApis.ArtistAlbumsApi,
                    new ArtistAlbumsRequest
                    {
                        ArtistId = artistId,
                        Start = start,
                        Limit = count
                    }, ctk);
                return albumResult.Match(
                    success => (success.HasMore,
                        success.Albums?.Select(album => (ProvidableItemBase)album.MapToNeteaseAlbum()!).ToList()
                        ?? new List<ProvidableItemBase>()),
                    error => (false, new List<ProvidableItemBase>()));
            default:
                return (false, new List<ProvidableItemBase>());
        }
        }
        else throw new ArgumentNullException();
    }

    public int MaxProgressiveCount => 50;
}

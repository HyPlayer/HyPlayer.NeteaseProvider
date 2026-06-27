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
        return (await GetProgressiveItemsListAsync(0, MaxProgressiveCount, ctk)).Item2;
    }

    public async Task<(bool, List<ProvidableItemBase>)> GetProgressiveItemsListAsync(int start = 0, int count = 50, CancellationToken ctk = default)
    {
        if (ActualId is null)
            throw new ArgumentNullException();

        var itemType = ActualId.Substring(0, 3);
        var artistId = ActualId.Substring(3);

        switch (itemType)
        {
            case "hot":
            case "tim":
                var songs = await NeteaseProvider.Instance.RequestAsync(
                    NeteaseApis.ArtistTopSongApi,
                    new ArtistTopSongRequest
                    {
                        ArtistId = artistId,
                        OrderType = itemType == "tim" ? ArtistSongsOrderType.Time : ArtistSongsOrderType.Hot,
                        Offset = start,
                        Limit = count
                    }, ctk);
                return songs.Match(
                    success => (success.More,
                        success.Songs?.Select(song => (ProvidableItemBase)song.MapToNeteaseMusic()).ToList() ?? []),
                    error => (false, []));
            case "alb":
                var albums = await NeteaseProvider.Instance.RequestAsync(
                    NeteaseApis.ArtistAlbumsApi,
                    new ArtistAlbumsRequest
                    {
                        ArtistId = artistId,
                        Start = start,
                        Limit = count
                    }, ctk);
                return albums.Match(
                    success => (success.HasMore,
                        success.Albums?.Select(album => (ProvidableItemBase)album.MapToNeteaseAlbum()!).ToList() ?? []),
                    error => (false, []));
            default:
                return (false, []);
        }
    }

    public int MaxProgressiveCount => 50;
}

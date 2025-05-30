using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.ApiContracts.Artist;
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

    public override async Task<List<ProvidableItemBase>> GetAllItemsAsync(CancellationToken ctk = new())
    {
        if (ActualId != null)
        {
            var itemType = ActualId.Substring(0, 3);
            var artistId = ActualId.Substring(3);
            switch (itemType)
            {
                case "hot":
                default:
                    var result = await NeteaseProvider.Instance.RequestAsync(
                        NeteaseApis.ArtistSongsApi,
                        new ArtistSongsRequest
                        {
                            ArtistId = artistId,
                            OrderType = ArtistSongsOrderType.Hot,
                            Offset = 0,
                            Limit = 50
                        });
                    return result.Match(
                        success =>
                            success.Songs.Select(song => (ProvidableItemBase)song.MapToNeteaseMusic()).ToList(),
                        error => new List<ProvidableItemBase>()
                    );
                case "tim":
                    var resTime = await NeteaseProvider.Instance.RequestAsync(
                        NeteaseApis.ArtistSongsApi,
                        new ArtistSongsRequest
                        {
                            ArtistId = artistId,
                            OrderType = ArtistSongsOrderType.Time,
                            Offset = 0,
                            Limit = 50
                        });
                    return resTime.Match(
                        success =>
                            success.Songs.Select(song => (ProvidableItemBase)song.MapToNeteaseMusic()).ToList(),
                        error => new List<ProvidableItemBase>()
                    );
                case "alb":
                    var resAlbum =
                        await NeteaseProvider.Instance.RequestAsync(
                            NeteaseApis.ArtistAlbumsApi,
                            new ArtistAlbumsRequest()
                            {
                                ArtistId = artistId,
                                Limit = 50,
                                Start = 0
                            });
                    return resAlbum.Match(
                        success =>
                            success.Albums?.Select(alb => (ProvidableItemBase)alb.MapToNeteaseAlbum()!).ToList() ?? new(),
                        error => new List<ProvidableItemBase>()
                    );
            }
        }
        else throw new ArgumentNullException();
    }

    public async Task<(bool, List<ProvidableItemBase>)> GetProgressiveItemsListAsync(int start = 0, int count = 50, CancellationToken ctk = new())
    {
        if(ActualId!= null) {
        var itemType = ActualId.Substring(0, 3);
        var artistId = ActualId.Substring(3);
        switch (itemType)
        {
            case "hot":
            default:
                var result = await NeteaseProvider.Instance.RequestAsync(
                    NeteaseApis.ArtistSongsApi,
                    new ArtistSongsRequest
                    {
                        ArtistId = artistId,
                        OrderType = ArtistSongsOrderType.Hot,
                        Offset = start,
                        Limit = count
                    });
                return result.Match(
                    success => (success.HasMore,
                        success.Songs?.Select(song => (ProvidableItemBase)song.MapToNeteaseMusic()).ToList() ?? new List<ProvidableItemBase>()),
                    error => (false, new List<ProvidableItemBase>())
                );
            case "tim":
                var resTime = await NeteaseProvider.Instance.RequestAsync(
                    NeteaseApis.ArtistSongsApi,
                    new ArtistSongsRequest
                    {
                        ArtistId = artistId,
                        OrderType = ArtistSongsOrderType.Hot,
                        Offset = 0,
                        Limit = 50
                    });
                return resTime.Match(
                    success => (success.HasMore,
                                    success.Songs?.Select(song => (ProvidableItemBase)song.MapToNeteaseMusic()).ToList() ?? new List<ProvidableItemBase>()),
                                error => (false, new List<ProvidableItemBase>())
                );

            }
        }
        else throw new ArgumentNullException();
    }

    public int MaxProgressiveCount => 50;
}
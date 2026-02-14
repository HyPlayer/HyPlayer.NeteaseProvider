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
            var itemType = ActualId.Substring(0, 3);
            var artistId = ActualId.Substring(3);
            var resTime = await NeteaseProvider.Instance.RequestAsync(
                        NeteaseApis.ArtistSongsApi,
                        new ArtistSongsRequest
                        {
                            ArtistId = artistId,
                            Offset = 0,
                            Limit = 50
                        });
            return resTime.Match(
                success =>
                    success.Songs?.Select(song => (ProvidableItemBase)song.MapToNeteaseMusic()).ToList() ?? [],
                error => new List<ProvidableItemBase>()
            );
        }
        else throw new ArgumentNullException();
    }

    public async Task<(bool, List<ProvidableItemBase>)> GetProgressiveItemsListAsync(int start = 0, int count = 50, CancellationToken ctk = default)
    {
        if(ActualId!= null) {
        var itemType = ActualId.Substring(0, 3);
        var artistId = ActualId.Substring(3);
            var resTime = await NeteaseProvider.Instance.RequestAsync(
                        NeteaseApis.ArtistSongsApi,
                        new ArtistSongsRequest
                        {
                            ArtistId = artistId,
                            Offset = 0,
                            Limit = 50
                        });
            return resTime.Match(
                    success => (success.HasMore,
                        success.Songs?.Select(song => (ProvidableItemBase)song.MapToNeteaseMusic()).ToList() ?? new List<ProvidableItemBase>()),
                    error => (false, new List<ProvidableItemBase>()));
        }
        else throw new ArgumentNullException();
    }

    public int MaxProgressiveCount => 50;
}
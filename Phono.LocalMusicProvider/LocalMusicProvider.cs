using HyPlayer.PlayCore.Abstraction;
using HyPlayer.PlayCore.Abstraction.Interfaces.Provider;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Lyric;
using HyPlayer.PlayCore.Abstraction.Models.Resources;
using HyPlayer.PlayCore.Abstraction.Models.SingleItems;


namespace Phono.LocalMusicProvider
{
    public class LocalMusicProvider : ProviderBase,
                               ILyricProvidable,
                               IMusicResourceProvidable,
                               IProvidableItemProvidable,
                               IProvidableItemRangeProvidable,
                               ISearchableProvider
    {
        public override string Name => "本地音乐";

        public override string Id => "local";

        public override List<ProvidableTypeId> ProvidableTypeIds => throw new NotImplementedException();

        public Task<List<RawLyricInfo>> GetLyricInfoAsync(SingleSongBase song, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<MusicResourceBase?> GetMusicResourceAsync(SingleSongBase song, ResourceQualityTag qualityTag, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<ProvidableItemBase?> GetProvidableItemByIdAsync(string inProviderId, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProvidableItemBase>> GetProvidableItemsRangeAsync(List<string> inProviderIds, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<ContainerBase> SearchProvidableItemsAsync(string keyword, string typeId, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

    }
}

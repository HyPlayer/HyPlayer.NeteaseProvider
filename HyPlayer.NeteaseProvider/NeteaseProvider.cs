using System.Collections.ObjectModel;
using HyPlayer.NeteaseProvider.Bases;
using HyPlayer.PlayCore.Abstraction;
using HyPlayer.PlayCore.Abstraction.Interfaces.Provider;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Lyric;
using HyPlayer.PlayCore.Abstraction.Models.Resources;
using HyPlayer.PlayCore.Abstraction.Models.Songs;

namespace HyPlayer.NeteaseProvider;

public class NeteaseProvider : ProviderBase,
                               ILyricProvidable,
                               IMusicResourceProvidable,
                               IProvableItemLikable,
                               IProvidableItemProvidable,
                               IProvidableItemRangeProvidable,
                               ISearchableProvider
{

    public ProviderOption Option { get; set; } = new();
    private readonly NeteaseCloudMusicApiHandler _handler = new();
    
    public NeteaseProvider()
    {
        Name = "网易云音乐";
        Id = "ncm";
    }
    
    public async Task<List<RawLyricInfo>> GetLyricInfo(SingleSongBase song)
    {
        throw new NotImplementedException();
    }

    public async Task<MusicResourceBase?> GetMusicResource(SingleSongBase song)
    {
        throw new NotImplementedException();
    }

    public async Task LikeProvidableItem(string inProviderId, string? targetId)
    {
        throw new NotImplementedException();
    }

    public async Task UnlikeProvidableItem(string inProviderId, string? targetId)
    {
        throw new NotImplementedException();
    }

    public async Task<ReadOnlyCollection<string>> GetLikedProvidableIds(string typeId)
    {
        throw new NotImplementedException();
    }

    public async Task<ProvidableItemBase> GetProvidableItemById(string inProviderId)
    {
        throw new NotImplementedException();
    }

    public async Task<ReadOnlyCollection<ProvidableItemBase>> GetProvidableItemsRange(List<string> inProviderIds)
    {
        throw new NotImplementedException();
    }

    public async Task<ContainerBase> SearchProvidableItems(string keyword, string typeId)
    {
        throw new NotImplementedException();
    }
    
}
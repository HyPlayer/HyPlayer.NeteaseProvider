using System.Collections.ObjectModel;
using HyPlayer.PlayCore.Abstraction.Interfaces.PlayListContainer;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseArtistSubContainer : LinerContainerBase, IProgressiveLoadingContainer
{
    public override string ProviderId => "ncm";
    public override string TypeId => "sg";
    public override async Task<ReadOnlyCollection<ProvidableItemBase>> GetAllItems()
    {
        throw new NotImplementedException();
    }

    public async Task<(bool, ReadOnlyCollection<ProvidableItemBase>)> GetProgressiveItemsList(int start, int count)
    {
        throw new NotImplementedException();
    }

    public int MaxProgressiveCount => 50;
}
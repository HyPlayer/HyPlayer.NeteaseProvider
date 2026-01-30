using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.PlayCore.Abstraction.Interfaces.PlayListContainer;
using HyPlayer.PlayCore.Abstraction.Interfaces.ProvidableItem;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.Resources;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseRadioChannel : LinerContainerBase, IProgressiveLoadingContainer, IHasCover, IHasDescription,
IHasCreators
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.RadioChannel;
    public override async Task<List<ProvidableItemBase>> GetAllItemsAsync(CancellationToken ctk = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public int MaxProgressiveCount => 100;
    public async Task<(bool, List<ProvidableItemBase>)> GetProgressiveItemsListAsync(int start, int count, CancellationToken ctk = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public async Task<ResourceResultBase> GetCoverAsync(ImageResourceQualityTag? qualityTag = null, CancellationToken ctk = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public string? Description { get; set; }
    public List<string>? CreatorList { get; init; }
    public async Task<List<PersonBase>?> GetCreatorsAsync(CancellationToken ctk = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}
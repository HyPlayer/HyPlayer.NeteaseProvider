using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseActionGettableContainer : LinerContainerBase
{
    public NeteaseActionGettableContainer(Func<Task<List<ProvidableItemBase>>> getter)
    {
        Getter = getter;
    }

    public override string ProviderId => "ncm";
    public override string TypeId => "ag";
    
    public Func<Task<List<ProvidableItemBase>>> Getter { get; set; }
    
    public override async Task<List<ProvidableItemBase>> GetAllItems()
    {
        return await Getter.Invoke();
    }
}
using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.PlayCore.Abstraction.Interfaces.PlayListContainer;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseActionGettableContainer : LinerContainerBase
{
    public NeteaseActionGettableContainer()
    {
    }

    public NeteaseActionGettableContainer(Func<Task<List<ProvidableItemBase>>> getter)
    {
        Getter = getter;
    }

    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.ActionGettableSongContainer;

    public Func<Task<List<ProvidableItemBase>>>? Getter { get; set; }

    public override async Task<List<ProvidableItemBase>> GetAllItems()
    {
        return await (Getter?.Invoke() ?? Task.FromResult(new List<ProvidableItemBase>()));
    }
}

public class NeteaseActionGettableProgressiveContainer : NeteaseActionGettableContainer, IProgressiveLoadingContainer
{
    public NeteaseActionGettableProgressiveContainer(
        Func<int, int, Task<(bool, List<ProvidableItemBase>)>> progressiveGetter)
    {
        ProgressiveGetter = progressiveGetter;
    }

    public Func<int, int, Task<(bool, List<ProvidableItemBase>)>>? ProgressiveGetter { get; set; }


    public async Task<(bool, List<ProvidableItemBase>)> GetProgressiveItemsList(int start, int count)
    {
        return await (ProgressiveGetter?.Invoke(start, count) ??
                      Task.FromResult((false, new List<ProvidableItemBase>())));
    }

    public override async Task<List<ProvidableItemBase>> GetAllItems()
    {
        return (await GetProgressiveItemsList(0,MaxProgressiveCount)).Item2;
    }

    public int MaxProgressiveCount { get; set; } = 30;
}
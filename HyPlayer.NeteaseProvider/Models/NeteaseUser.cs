using System.Collections.ObjectModel;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseUser : PersonBase
{
    public override string ProviderId { get; }
    public override string TypeId { get; }
    public override async Task<ReadOnlyCollection<ContainerBase>> GetSubContainer()
    {
        throw new NotImplementedException();
    }
}
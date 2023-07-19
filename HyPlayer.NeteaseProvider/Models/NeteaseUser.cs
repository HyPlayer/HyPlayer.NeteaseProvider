using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseUser : PersonBase
{
    public override string ProviderId => "ncm";
    public override string TypeId => "us";
    public override async Task<List<ContainerBase>> GetSubContainer()
    {
        throw new NotImplementedException();
    }
}
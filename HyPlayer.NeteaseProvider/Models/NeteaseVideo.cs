using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.PlayCore.Abstraction.Models;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseVideo : ProvidableItemBase
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.MBlog;
}
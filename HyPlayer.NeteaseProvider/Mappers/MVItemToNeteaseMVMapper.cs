using HyPlayer.NeteaseApi.Models.ResponseModels;
using HyPlayer.NeteaseProvider.Models;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class MVItemToNeteaseMVMapper
{
    public static NeteaseMv MapToNeteaseMV(this MVDto item)
    {
        return new NeteaseMv
        {
            Name = item.Name ?? "未知MV",
            ActualId = item.Id!,
            CoverUrl = item.Cover,
        };
    }
}
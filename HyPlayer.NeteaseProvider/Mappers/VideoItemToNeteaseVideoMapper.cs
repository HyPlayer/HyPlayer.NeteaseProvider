using HyPlayer.NeteaseApi.Models.ResponseModels;
using HyPlayer.NeteaseProvider.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class VideoItemToNeteaseVideoMapper
{
    public static NeteaseVideo MapToNeteaseVideo(this VideoDto videoItem)
    {
        var creators = videoItem.Artists?
            .Where(creator => !string.IsNullOrEmpty(creator.UserId) && !string.IsNullOrEmpty(creator.UserName))
            .ToList() ?? [];
        return new NeteaseVideo
        {
            ActualId = videoItem.Id,
            Name = videoItem.Title ?? "",
            CoverUrl = videoItem.CoverUrl,
            Duration = videoItem.Duration,
            PlayCount = videoItem.PlayTime,
            CreatorList = creators.Select(creator => creator.UserName!).ToList(),
            Creators = creators.Select(PersonBase (creator) => new NeteaseUser
            {
                ActualId = creator.UserId!,
                Name = creator.UserName!
            }).ToList()
        };
    }
}
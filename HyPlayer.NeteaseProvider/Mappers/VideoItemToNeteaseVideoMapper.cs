using HyPlayer.NeteaseApi.Models.ResponseModels;
using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.NeteaseProvider.Models;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class VideoItemToNeteaseVideoMapper
{
    public static NeteaseVideo MapToNeteaseVideo(this VideoDto videoItem)
    {
        // TODO: Add more properties
        return new NeteaseVideo
        {
            ActualId = videoItem.Id,
            Name = videoItem.Title,
        };
    }
}
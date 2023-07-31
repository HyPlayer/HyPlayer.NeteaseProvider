using HyPlayer.NeteaseApi.Models.ResponseModels;
using HyPlayer.NeteaseProvider.Models;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class RecommendPlaylistToNeteasePlaylistMapper
{
    public static NeteasePlaylist MapToNeteasePlaylist(this RecommendPlaylistDto dto)
    {
        return new NeteasePlaylist
               {
                   Name = dto.Name ?? "未知歌单",
                   ActualId = dto.Id!,
                   CoverUrl = dto.CoverUrl,
                   Creator = dto.Creator?.MapToNeteaseUser(),
                   Subscribed = false,
                   UpdateTime = 0,
                   TrackCount = dto.TrackCount,
                   PlayCount = dto.PlayCount,
                   SubscribedCount = 0,
                   CreatorList = new List<string>() { dto.Creator?.Nickname! }
               };
    }
}
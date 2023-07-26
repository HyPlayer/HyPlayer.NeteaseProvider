using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using HyPlayer.NeteaseProvider.Models;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class PlaylistItemToNeteasePlaylistMapper
{
    public static NeteasePlaylist MapToNeteasePlaylist(this PlaylistDto item)
    {
        return new NeteasePlaylist
               {
                   Name = item.Name ??"未知歌单",
                   ActualId = item.Id!,
                   Description = item.Description,
                   CreatorList = new List<string>() { item.Creator?.Nickname! },
                   Creator = item.Creator?.MapToNeteaseUser(),
                   Subscribed = item.Subscribed is true,
                   UpdateTime = item.UpdateTime,
                   TrackCount = item.TrackCount,
                   PlayCount = item.PlayCount,
                   SubscribedCount = item.SubscribedCount,
                   CoverUrl = item.CoverUrl
               };
    }
}
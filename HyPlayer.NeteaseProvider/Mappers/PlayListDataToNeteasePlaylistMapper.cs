using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseProvider.Models;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class PlayListDataToNeteasePlaylistMapper
{
    public static NeteasePlaylist MapToNeteasePlaylist(this PlaylistDetailResponse.PlayListData data)
    {
        return new NeteasePlaylist
               {
                   Name = data.Name ??"未知歌单",
                   ActualId = data.Id,
                   Description = data.Description,
                   CreatorList = new List<string>() { data.Creator?.Nickname! },
                   Creator = data.Creator?.MapToNeteaseUser()
               };
    }
}
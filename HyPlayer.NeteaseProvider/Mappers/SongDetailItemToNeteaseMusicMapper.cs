using System.Collections.ObjectModel;
using HyPlayer.NeteaseProvider.ApiContracts;
using HyPlayer.NeteaseProvider.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class SongDetailItemToNeteaseMusicMapper
{
    public static NeteaseMusic MapToNeteaseMusic(this SongDetailResponse.SongItem item)
    {
        return new NeteaseMusic
               {
                   Name = item.Name ?? "未知歌曲",
                   ActualId = item.Id!,
                   Album = item.Album.MapToNeteaseAlbum(),
                   CreatorList = item.Artists is { Length: > 0 }
                       ? new ReadOnlyCollection<string>(item.Artists.Select(ar => ar.Name).ToList()!)
                       : new ReadOnlyCollection<string>(new List<string>()),
                   Duration = item.Duration,
                   Available = true,
                   Artists = item.Artists?.Select(ar=>(PersonBase)ar.MapToNeteaseArtist()).ToList()
               };
    }
}
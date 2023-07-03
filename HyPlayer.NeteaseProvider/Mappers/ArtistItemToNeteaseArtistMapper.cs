using HyPlayer.NeteaseProvider.ApiContracts;
using HyPlayer.NeteaseProvider.Models;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class ArtistItemToNeteaseArtistMapper
{
    public static NeteaseArtist MapToNeteaseArtist(this SongDetailResponse.SongItem.ArtistItem item)
    {
        return new NeteaseArtist
               {
                   Name = item.Name!,
                   ActualId = item.Id!
               };
    }
}
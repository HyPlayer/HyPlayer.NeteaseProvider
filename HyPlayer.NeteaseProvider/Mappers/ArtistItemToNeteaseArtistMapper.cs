using HyPlayer.NeteaseApi.Models.ResponseModels;
using HyPlayer.NeteaseProvider.Models;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class ArtistItemToNeteaseArtistMapper
{
    public static NeteaseArtist MapToNeteaseArtist(this ArtistDto item)
    {
        return new NeteaseArtist
        {
            Name = item.Name!,
            ActualId = item.Id!,
            Translation = item.Translation,
            Alias = item.Alias,
            CoverUrl = item.Img1v1Url ?? item.PicUrl,
            AlbumSize = item.AlbumSize,
            MvSize = item.MvSize
        };
    }
}

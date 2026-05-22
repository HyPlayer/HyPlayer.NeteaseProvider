using HyPlayer.NeteaseApi.Models.ResponseModels;
using HyPlayer.NeteaseProvider.Models;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class AlbumDataToNeteaseAlbumMapper
{
    public static NeteaseAlbum? MapToNeteaseAlbum(this AlbumDto? data)
    {
        if (data is null) return null;
        return new NeteaseAlbum
        {
            Name = data.Name ?? "未知专辑",
            ActualId = data.Id!,
            PictureUrl = data.PictureUrl,
            Alias = data.Alias?.ToList(),
            Company = data.Company,
            BriefDescription = data.BriefDescription,
            SubType = data.SubType,
            Artists = data.Artists?.Select(t => t.MapToNeteaseArtist()).ToList(),
            Translation = data.Translation,
            Description = data.Description,
            CreatorList = data.Artists?.Select(t => t.Name ?? "未知歌手").ToList()
        };

    }
}

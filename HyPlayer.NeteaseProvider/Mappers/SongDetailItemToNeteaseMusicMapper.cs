using HyPlayer.NeteaseApi.Models.ResponseModels;
using HyPlayer.NeteaseProvider.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class SongDetailItemToNeteaseMusicMapper
{
    public static NeteaseSong MapToNeteaseMusic(this EmittedSongDto item)
    {
        return CreateSong(
            item.Name,
            item.Id,
            item.Album,
            item.Artists,
            item.Duration,
            true,
            item.Alias,
            string.Join(" / ", item.Translations ?? []),
            item.MvId,
            item.CdName,
            item.TrackNumber);
    }

    public static NeteaseSong MapToNeteaseMusic(this EmittedSongDtoWithPrivilege item)
    {
        return CreateSong(
            item.Name,
            item.Id,
            item.Album,
            item.Artists,
            item.Duration,
            item.Privilege?.PlayLevel is not (null or "none"),
            item.Alias,
            string.Join(" / ", item.Translations ?? []),
            item.MvId,
            item.CdName,
            item.TrackNumber);
    }

    public static NeteaseSong MapToNeteaseMusic(this SongDto item)
    {
        return CreateSong(
            item.Name,
            item.Id,
            item.Album,
            item.Artists,
            item.Duration,
            true,
            item.Alias,
            item.Translation,
            item.VideoInfo?.Video?.Vid ?? item.MvId,
            item.CdName,
            item.TrackNumber);
    }

    public static NeteaseSong MapToNeteaseMusic(this SongWithPrivilegeDto item)
    {
        return CreateSong(
            item.Name,
            item.Id,
            item.Album,
            item.Artists,
            item.Duration,
            item.Privilege?.PlayLevel is not (null or "none"),
            item.Alias,
            item.Translation,
            item.VideoInfo?.Video?.Vid ?? item.MvId,
            item.CdName,
            item.TrackNumber);
    }
    public static NeteaseSong MapToNeteaseMusic(this ArtistSongDto item)
    {
        return CreateSong(
            item.Name,
            item.Id,
            item.Album,
            item.Artists,
            item.Duration,
            item.Privilege?.PlayLevel is not (null or "none"),
            item.Alias,
            string.Join(" / ", item.Translations ?? []),
            item.MvId,
            null,
            item.TrackNumber);
    }

    private static NeteaseSong CreateSong(
        string? name,
        string? id,
        AlbumDto? album,
        ArtistDto[]? artists,
        long duration,
        bool available,
        string[]? alias,
        string? translation,
        string? mvId,
        string? cdName,
        int trackNumber)
    {
        return new NeteaseSong
        {
            Name = name ?? "未知歌曲",
            ActualId = id!,
            Album = album.MapToNeteaseAlbum(),
            CreatorList = artists is { Length: > 0 }
                       ? artists.Select(ar => ar.Name ?? "未知歌手").ToList()
                       : new List<string>(),
            Duration = duration,
            Available = available,
            Artists = artists?.Select(ar => (PersonBase)ar.MapToNeteaseArtist()).ToList(),
            CoverUrl = album?.PictureUrl,
            Alias = alias,
            Translation = translation,
            MvId = mvId,
            CdName = cdName,
            TrackNumber = trackNumber
        };
    }
}

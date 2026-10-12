using HyPlayer.NeteaseApi.ApiContracts.Recommend;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using HyPlayer.NeteaseProvider.Models;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class LyricSearchResultToNeteaseLyricSearchItemMapper
{
    public static NeteaseLyricSearchItem MapNeteaseLyricSearchItem(this SearchLyricResponse.SearchLyricResult.LyricSearchResultDto result)
    {
        var song = ((EmittedSongDtoWithPrivilege)result).MapToNeteaseMusic();
        return new NeteaseLyricSearchItem
        {
            Name = song.Name,
            ActualId = song.ActualId,
            Album = song.Album,
            CreatorList = song.CreatorList,
            Duration = song.Duration,
            Available = song.Available,
            Artists = song.Artists,
            CoverUrl = song.CoverUrl,
            Alias = song.Alias,
            Translation = song.Translation,
            MvId = song.MvId,
            CdName = song.CdName,
            TrackNumber = song.TrackNumber,
            MatchedLyrics = result.Lyrics?
                .Select(line => line.Replace("<b>", string.Empty).Replace("</b>", string.Empty).Trim())
                .Where(line => line.Length > 0)
                .ToList() ?? []
        };
    }
}
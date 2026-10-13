using HyPlayer.NeteaseApi.ApiContracts.Recommend;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using HyPlayer.NeteaseProvider.Models;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class LyricSearchResultToNeteaseLyricSearchItemMapper
{
    private const string MatchStart = "<b>";
    private const string MatchEnd = "</b>";

    public static NeteaseLyricSearchItem MapNeteaseLyricSearchItem(
        this SearchLyricResponse.SearchLyricResult.LyricSearchResultDto result,
        string? keyword = null)
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
            MatchedLyrics = SelectMatchedLyrics(result.Lyrics, keyword)
        };
    }

    public static IReadOnlyList<string> SelectMatchedLyrics(IReadOnlyList<string>? lines, string? keyword)
    {
        if (lines is null || lines.Count == 0)
            return [];

        var marked = lines
            .Where(line => line is not null && line.Contains(MatchStart))
            .Select(RemoveMatchMarks)
            .Where(line => line.Length > 0)
            .ToList();
        if (marked.Count > 0)
            return marked;

        var query = keyword?.Trim();
        if (string.IsNullOrEmpty(query))
            return [];

        var plain = lines
            .Where(line => line is not null)
            .Select(RemoveMatchMarks)
            .Where(line => line.Length > 0)
            .ToList();
        var whole = plain.Where(line => ContainsText(line, query)).ToList();
        if (whole.Count > 0)
            return whole;

        var terms = query.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        if (terms.Length < 2)
            return [];

        var every = plain
            .Where(line => terms.All(term => ContainsText(line, term)))
            .ToList();
        return every.Count > 0
            ? every
            : plain.Where(line => terms.Any(term => ContainsText(line, term))).ToList();
    }

    private static string RemoveMatchMarks(string line)
    {
        return line.Replace(MatchStart, string.Empty).Replace(MatchEnd, string.Empty).Trim();
    }

    private static bool ContainsText(string line, string text)
    {
        return line.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0;
    }
}
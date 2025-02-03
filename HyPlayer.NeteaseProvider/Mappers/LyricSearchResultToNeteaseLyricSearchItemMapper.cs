using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseProvider.Models;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class LyricSearchResultToNeteaseLyricSearchItemMapper
{
    public static NeteaseLyricSearchItem MapNeteaseLyricSearchItem(this SearchLyricResponse.SearchLyricResult.LyricSearchResultDto result)
    {
        // TODO: Implement mapping
        return new NeteaseLyricSearchItem
        {
            ActualId = "",
            Name = ""
        };
    }
}
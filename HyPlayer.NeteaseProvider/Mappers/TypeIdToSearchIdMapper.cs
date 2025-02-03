using HyPlayer.NeteaseApi.Extensions;
using HyPlayer.NeteaseApi.Models;
using HyPlayer.NeteaseProvider.Constants;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class TypeIdToSearchIdMapper
{
    public static Dictionary<string, NeteaseResourceType> ResourceMap =
        new()
        {
            { NeteaseTypeIds.SingleSong, NeteaseResourceType.Song },
            { NeteaseTypeIds.Playlist, NeteaseResourceType.Playlist },
            { NeteaseTypeIds.Album, NeteaseResourceType.Album },
            { NeteaseTypeIds.Artist, NeteaseResourceType.Artist },
            { NeteaseTypeIds.User, NeteaseResourceType.User },
            { NeteaseTypeIds.RadioChannel, NeteaseResourceType.RadioChannel },
            { NeteaseTypeIds.MBlog, NeteaseResourceType.MLog },
            { NeteaseTypeIds.Mv, NeteaseResourceType.MV },
            { NeteaseTypeIds.Lyric, NeteaseResourceType.Lyric },
            { NeteaseTypeIds.Dynamic, NeteaseResourceType.Dynamic },
        };

    public static NeteaseResourceType MapToResourceId(string typeId)
    {
        return ResourceMap.GetValueOrDefault(typeId);
    }
}
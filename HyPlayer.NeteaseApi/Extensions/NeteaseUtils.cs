using HyPlayer.NeteaseApi.Models;

namespace HyPlayer.NeteaseApi.Extensions;

public class NeteaseUtils
{
    public static string CommentTypeToThreadPrefix(NeteaseResourceType type)
    {
        switch (type)
        {
            case NeteaseResourceType.Song: return "R_SO_4_";
            case NeteaseResourceType.MV: return "R_MV_5_";
            case NeteaseResourceType.Playlist: return "A_PL_0_";
            case NeteaseResourceType.Album: return "R_AL_3_";
            case NeteaseResourceType.RadioChannel: return "A_DJ_1_";
            case NeteaseResourceType.Video: return "R_VI_62_";
            case NeteaseResourceType.Dynamic: return "A_EV_2_";
            case NeteaseResourceType.MLog: return "R_MLOG_1001_";
            default: throw new ArgumentOutOfRangeException(nameof(type));
        }
    }
}
using HyPlayer.NeteaseApi.Extensions;
using HyPlayer.NeteaseProvider.Constants;

namespace HyPlayer.NeteaseProvider.Mappers;

public class TypeIdToResourceIdMapper
{
    public static Dictionary<string, string> ResourceMap =
        new()
        {
            { "sg", "R_SO_4_" },
            { "mv", "R_MV_5_" },
            { NeteaseTypeIds.Playlist, "A_PL_0_" },
            { NeteaseTypeIds.Album, "R_AL_3_" },
            { NeteaseTypeIds.RadioProgram, "A_DJ_1_" },
            { "vd", "R_VI_62_" },
            { "dy", "A_EV_2_" },
            { NeteaseTypeIds.RadioChannel, "A_DR_14_" }
        };

    public static string MapToResourceId(string typeId)
    {
        return ResourceMap.GetValueOrDefault(typeId) ?? string.Empty;
    }
}
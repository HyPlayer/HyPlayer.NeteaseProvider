using HyPlayer.NeteaseApi.Extensions;

namespace HyPlayer.NeteaseProvider.Mappers;

public class TypeIdToResourceIdMapper
{
    public static Dictionary<string, string> ResourceMap =
        new()
        {
            { "sg", "R_SO_4_" },
            { "mv", "R_MV_5_" },
            { "pl", "A_PL_0_" },
            { "al", "R_AL_3_" },
            { "rd", "A_DJ_1_" },
            { "vd", "R_VI_62_" },
            { "dy", "A_EV_2_" },
            { "dj", "A_DR_14_" }
        };

    public static string MapToResourceId(string typeId)
    {
        return ResourceMap.GetValueOrDefault(typeId) ?? string.Empty;
    }
}
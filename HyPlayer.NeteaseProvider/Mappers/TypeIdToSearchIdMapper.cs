using HyPlayer.NeteaseApi.Extensions;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class TypeIdToSearchIdMapper
{
    public static Dictionary<string, int> ResourceMap =
        new()
        {
            { "sg", 1 },
            { "pl", 1000 },
            { "al", 10 },
            { "ar", 100 },
            { "us", 1002 },
            { "rd", 1009 },
            { "vd", 1014 },
            { "mv", 1004 },
            { "lr", 1006 },
            { "dy", 2000 },
        };

    public static int MapToResourceId(string typeId)
    {
        return ResourceMap.GetValueOrDefault(typeId, 1);
    }
}
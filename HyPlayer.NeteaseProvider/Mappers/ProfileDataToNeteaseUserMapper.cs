using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseProvider.Models;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class ProfileDataToNeteaseUserMapper
{
    public static NeteaseUser MapToNeteaseUser(this LoginResponse.ProfileData profileData)
    {
        return new NeteaseUser
               {
                   Name = profileData.Nickname!,
                   ActualId = profileData.UserId!,
                   Gender = profileData.Gender,
                   BackgroundUrl = profileData.BackgroundUrl,
                   Followed = profileData.Followed is true,
                   VipType = profileData.VipType,
                   AvatarUrl = profileData.AvatarUrl,
                   Description = profileData.Signature
               };
    }
}
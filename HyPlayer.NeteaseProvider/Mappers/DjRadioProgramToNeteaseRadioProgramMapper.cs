using HyPlayer.NeteaseApi.Models.ResponseModels;
using HyPlayer.NeteaseProvider.Models;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class DjRadioProgramToNeteaseRadioProgramMapper
{
    public static NeteaseRadioProgram MapToNeteaseRadioProgram(this DjRadioProgramDto program)
    {
        return new NeteaseRadioProgram
        {
            ActualId = program.Id!,
            Name = program.Name ?? "未知节目",
            PictureUrl = program.PictureUrl,
            CoverUrl = program.CoverUrl,
            Description = program.Description,
            ProgramDuration = program.Duration,
            RadioChannel = program.Radio?.MapToNeteaseRadioChannel(),
            MainSong = program.MainSong?.MapToNeteaseMusic(),
            Bought = program.Bought,
            ListenerCount = program.ListenerCount,
            SubscribedCount = program.SubscribedCount,
            CommentCount = program.CommentCount,
            ShareCount = program.ShareCount,
            LikedCount = program.LikedCount,
            CreateTime = program.CreateTime,
            SerialNum = program.SerialNum,
            Host = program.Owner?.MapToNeteaseUser(),
            CreatorList = program.Owner != null
                ? new List<string> { program.Owner.Nickname ?? program.Owner.UserId ?? "未知主播" }
                : new List<string>(),
            Duration = program.MainSong?.Duration ?? program.Duration,
            Available = true
        };
    }
}

using HyPlayer.NeteaseApi.Models.ResponseModels;
using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.SingleItems;

namespace HyPlayer.NeteaseProvider.Models;

public sealed class NeteaseComment : CommentBase
{
    public override string ProviderId => "ncm";
    public override string TypeId => "cm";
}

public sealed class NeteaseSearchSuggestion : ProvidableItemBase
{
    public override string ProviderId => "ncm";
    public override string TypeId => "ss";
    public int SuggestionType { get; set; }
    public string? Algorithm { get; set; }
    public string? LastKeyword { get; set; }
    public string? Feature { get; set; }
}

public sealed class NeteaseCloudLibraryItem : CloudLibraryItemBase
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.SingleSong;
    public SingleSongBase? Song { get; set; }
    public string? FileName { get; set; }
}

public static class ProviderContractModelMappers
{
    public static NeteaseComment MapToNeteaseComment(this CommentDto comment)
    {
        return new NeteaseComment
        {
            ActualId = comment.CommentId,
            Name = comment.Content ?? comment.CommentId ?? string.Empty,
            Content = comment.RichContent ?? comment.Content,
            SendDate = comment.Time,
            LikedCount = comment.LikedCount,
            Sender = comment.User?.MapToNeteaseUser()
        };
    }

    public static NeteaseCloudLibraryItem MapToNeteaseCloudLibraryItem(this CloudMusicDto cloudSong)
    {
        return new NeteaseCloudLibraryItem
        {
            ActualId = cloudSong.SongId,
            Name = cloudSong.SongName ?? cloudSong.FileName ?? cloudSong.SongId ?? string.Empty,
            FileName = cloudSong.FileName,
            FileSize = cloudSong.FileSize,
            UploadedAt = cloudSong.AddTime > 0 ? DateTimeOffset.FromUnixTimeMilliseconds(cloudSong.AddTime) : null,
            Song = cloudSong.Song?.MapToNeteaseMusic()
        };
    }
}

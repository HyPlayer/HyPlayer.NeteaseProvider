using HyPlayer.NeteaseApi.Models.ResponseModels;
using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.PlayCore.Abstraction.Interfaces.ProvidableItem;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.Resources;
using HyPlayer.PlayCore.Abstraction.Models.SingleItems;
using System;
using System.Diagnostics.CodeAnalysis;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseComment : CommentBase, IHasCover
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.Comment;

    public NeteaseComment thisComment => this;

    public NeteaseComment()
    {
        Name = string.Empty;
        ActualId = string.Empty;
    }

    public string? AvatarUrl { get; set; }

    public Task<ResourceResultBase> GetCoverAsync(ImageResourceQualityTag? qualityTag = null, CancellationToken ctk = default)
    {
        if (qualityTag is NeteaseImageResourceQualityTag neteaseImageResourceQualityTag)
        {
            var result = new NeteaseImageResourceResult()
            {
                ExternalException = null,
                ResourceStatus = ResourceStatus.Success,
                Uri = new Uri($"{AvatarUrl}?{neteaseImageResourceQualityTag}")
            };
            return Task.FromResult(result as ResourceResultBase);
        }
        else
        {
            var result = new NeteaseImageResourceResult()
            {
                ExternalException = null,
                ResourceStatus = ResourceStatus.Success,
                Uri = new Uri($"{AvatarUrl}")
            };
            return Task.FromResult(result as ResourceResultBase);
        }
    }
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

public sealed class NeteaseCloudLibraryItem : CloudLibraryItemBase, IHasCover, IHasTranslation, IHasAliases, IHasTrackMetadata, IHasRichMediaReference
{
    public override string ProviderId => "ncm";
    public override string TypeId => NeteaseTypeIds.SingleSong;
    public string? FileName { get; set; }
    public List<PersonBase>? Artists { get; init; }
    public IReadOnlyList<string>? Aliases { get; init; }
    public string? RichMediaId { get; init; }
    public string? DiscName { get; init; }
    public int TrackNumber { get; init; }
    public string? CoverUrl { get; init; }
    public string? Translation { get; set; }

    public override Task<List<PersonBase>?> GetCreatorsAsync(CancellationToken ctk = default)
    {
        return Task.FromResult(Artists);
    }

    public Task<ResourceResultBase> GetCoverAsync(ImageResourceQualityTag? qualityTag = null, CancellationToken ctk = default)
    {
        return Task.FromResult<ResourceResultBase>(new NeteaseImageResourceResult
        {
            ExternalException = null,
            ResourceStatus = string.IsNullOrWhiteSpace(CoverUrl) ? ResourceStatus.Fail : ResourceStatus.Success,
            Uri = string.IsNullOrWhiteSpace(CoverUrl) ? null : new Uri(CoverUrl)
        });
    }
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
            ReplyCount = comment.ReplyCount,
            HasLiked = comment.Liked,
            Sender = comment.User?.MapToNeteaseUser()
        };
    }

    public static NeteaseCloudLibraryItem MapToNeteaseCloudLibraryItem(this CloudMusicDto cloudSong)
    {
        var song = cloudSong.Song?.MapToNeteaseMusic();
        return new NeteaseCloudLibraryItem
        {
            ActualId = song?.ActualId ?? cloudSong.SongId,
            Name = song?.Name ?? cloudSong.SongName ?? cloudSong.FileName ?? cloudSong.SongId ?? string.Empty,
            FileName = cloudSong.FileName,
            FileSize = cloudSong.FileSize,
            UploadedAt = cloudSong.AddTime > 0 ? DateTimeOffset.FromUnixTimeMilliseconds(cloudSong.AddTime) : null,
            Album = song?.Album,
            Artists = song?.Artists,
            CreatorList = song?.CreatorList,
            Duration = song?.Duration ?? 0,
            Available = song?.Available ?? true,
            Aliases = song?.Alias,
            RichMediaId = song?.MvId,
            DiscName = song?.CdName,
            TrackNumber = song?.TrackNumber ?? 0,
            CoverUrl = song?.CoverUrl,
            Translation = song?.Translation
        };
    }
}

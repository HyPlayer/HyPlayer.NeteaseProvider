using HyPlayer.NeteaseProvider.Constants;
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

    [SetsRequiredMembers]
    public NeteaseComment()
    {
        Name = string.Empty;
        ActualId = string.Empty;
    }

    public int ReplyCount { get; set; }
    public bool HasLiked { get; set; }
    public bool IsMainComment { get; set; } = true;
    public string? ResourceId { get; set; }
    public string? ResourceTypeId { get; set; }

    public string? AvatarUrl { get; set; }

    public DateTime SendTime => SendDate > 0 ? DateTimeOffset.FromUnixTimeMilliseconds(SendDate).LocalDateTime : DateTime.MinValue;

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

using HyPlayer.NeteaseApi.Models.ResponseModels;
using HyPlayer.NeteaseProvider.Models;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class CommentDtoToNeteaseCommentMapper
{
    public static NeteaseComment MapToNeteaseComment(this CommentDto commentDto, string? resourceId = null, string? resourceTypeId = null)
    {
        return new NeteaseComment
        {
            Name = commentDto.User?.Nickname ?? string.Empty,
            ActualId = commentDto.CommentId ?? string.Empty,
            Content = commentDto.Content,
            SendDate = commentDto.Time,
            LikedCount = commentDto.LikedCount,
            ReplyCount = commentDto.ReplyCount,
            HasLiked = commentDto.Liked,
            IsMainComment = string.IsNullOrEmpty(commentDto.ParentCommentId),
            ResourceId = resourceId,
            ResourceTypeId = resourceTypeId,
            AvatarUrl = commentDto.User?.AvatarUrl,
            Sender = new NeteaseUser
            {
                Name = commentDto.User?.Nickname ?? string.Empty,
                ActualId = commentDto.User?.UserId ?? string.Empty,
                AvatarUrl = commentDto.User?.AvatarUrl
            }
        };
    }
}

using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.ApiContracts.Playlist;
using HyPlayer.NeteaseApi.Models.ResponseModels;
using HyPlayer.NeteaseProvider.Models;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class PlaylistItemToNeteasePlaylistMapper
{
    public static NeteasePlaylist MapToNeteasePlaylist(this PlaylistDto item)
    {
        return CreatePlaylist(
            item.Name,
            item.Id,
            item.Description,
            item.Creator,
            item.Subscribed is true,
            item.UpdateTime,
            item.TrackCount,
            item.PlayCount,
            item.SubscribedCount,
            item.CoverUrl);
    }

    public static NeteasePlaylist MapToNeteasePlaylist(this RecommendPlaylistDto dto)
    {
        return CreatePlaylist(
            dto.Name,
            dto.Id,
            null,
            dto.Creator,
            false,
            0,
            dto.TrackCount,
            dto.PlayCount,
            0,
            dto.CoverUrl);
    }

    public static NeteasePlaylist MapToNeteasePlaylist(this PlaylistDetailResponse.PlayListData data)
    {
        var playlist = CreatePlaylist(
            data.Name,
            data.Id,
            data.Description,
            data.Creator,
            data.Subscribed is true,
            data.UpdateTime,
            data.TrackCount,
            data.PlayCount,
            data.SubscribedCount,
            data.CoverUrl);
        playlist.CommentCount = data.CommentCount;
        playlist.ShareCount = data.ShareCount;
        playlist.IsNewImported = data.IsNewImported;
        return playlist;
    }

    private static NeteasePlaylist CreatePlaylist(
        string? name,
        string? id,
        string? description,
        UserInfoDto? creator,
        bool subscribed,
        long updateTime,
        int trackCount,
        long playCount,
        long subscribedCount,
        string? coverUrl)
    {
        var creatorName = creator?.Nickname;
        return new NeteasePlaylist
        {
            Name = name ?? "未知歌单",
            ActualId = id!,
            Description = description,
            CreatorList = string.IsNullOrWhiteSpace(creatorName) ? [] : [creatorName],
            Creator = creator?.MapToNeteaseUser(),
            Subscribed = subscribed,
            UpdateTime = updateTime,
            TrackCount = trackCount,
            PlayCount = playCount,
            SubscribedCount = subscribedCount,
            CoverUrl = coverUrl
        };
    }
}

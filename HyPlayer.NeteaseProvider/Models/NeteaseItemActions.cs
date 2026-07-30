using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.ApiContracts.Album;
using HyPlayer.NeteaseApi.ApiContracts.Artist;
using HyPlayer.NeteaseApi.ApiContracts.DjChannel;
using HyPlayer.NeteaseApi.ApiContracts.Playlist;
using HyPlayer.NeteaseApi.ApiContracts.Song;
using HyPlayer.NeteaseApi.ApiContracts.User;
using HyPlayer.NeteaseApi.ApiContracts.Video;
using HyPlayer.NeteaseProvider.Constants;

namespace HyPlayer.NeteaseProvider.Models;

public static class NeteaseItemActions
{
    public static Task LikeAsync(this NeteaseSong song, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.LikeApi, new LikeRequest
        {
            TrackId = RequireId(song.ActualId, "Song"),
            Like = true,
            UserId = RequireId(NeteaseProvider.Instance.LoginedUser?.ActualId, "Logged-in user")
        }, ctk);
    }

    public static Task UnlikeAsync(this NeteaseSong song, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.LikeApi, new LikeRequest
        {
            TrackId = RequireId(song.ActualId, "Song"),
            Like = false,
            UserId = RequireId(NeteaseProvider.Instance.LoginedUser?.ActualId, "Logged-in user")
        }, ctk);
    }

    public static Task AddSongAsync(this NeteasePlaylist playlist, string songId, CancellationToken ctk = default)
    {
        return EditPlaylistTracksAsync(playlist.ActualId, songId, true, ctk);
    }

    public static Task RemoveSongAsync(this NeteasePlaylist playlist, string songId, CancellationToken ctk = default)
    {
        return RemoveSongFromPlaylistAsync(playlist.ActualId, songId, ctk);
    }

    public static Task RemoveSongFromPlaylistAsync(string? playlistId, string songId, CancellationToken ctk = default)
    {
        return EditPlaylistTracksAsync(playlistId, songId, false, ctk);
    }

    public static Task SubscribeAsync(this NeteasePlaylist playlist, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.PlaylistSubscribeApi, new PlaylistSubscribeRequest
        {
            IsSubscribe = true,
            PlaylistId = RequireId(playlist.ActualId, "Playlist")
        }, ctk);
    }

    public static Task UnsubscribeAsync(this NeteasePlaylist playlist, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.PlaylistSubscribeApi, new PlaylistSubscribeRequest
        {
            IsSubscribe = false,
            PlaylistId = RequireId(playlist.ActualId, "Playlist")
        }, ctk);
    }

    public static Task SubscribeAsync(this NeteaseAlbum album, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.AlbumSubscribeApi, new AlbumSubscribeRequest
        {
            Id = RequireId(album.ActualId, "Album"),
            IsSubscribe = true
        }, ctk);
    }

    public static Task UnsubscribeAsync(this NeteaseAlbum album, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.AlbumSubscribeApi, new AlbumSubscribeRequest
        {
            Id = RequireId(album.ActualId, "Album"),
            IsSubscribe = false
        }, ctk);
    }

    public static Task SubscribeAsync(this NeteaseArtist artist, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.ArtistSubscribeApi, new ArtistSubscribeRequest
        {
            ArtistId = RequireId(artist.ActualId, "Artist")
        }, ctk);
    }

    public static Task UnsubscribeAsync(this NeteaseArtist artist, CancellationToken ctk = default)
    {
        if (!long.TryParse(artist.ActualId, out var id)) return Task.CompletedTask;

        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.ArtistUnsubscribeApi, new ArtistUnsubscribeRequest
        {
            ArtistIds = [id]
        }, ctk);
    }

    public static Task SubscribeAsync(this NeteaseRadioChannel channel, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.DjChannelSubscribeApi, new DjChannelSubscribeRequest
        {
            Id = RequireId(channel.ActualId, "Radio channel"),
            IsSubscribe = true
        }, ctk);
    }

    public static Task UnsubscribeAsync(this NeteaseRadioChannel channel, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.DjChannelSubscribeApi, new DjChannelSubscribeRequest
        {
            Id = RequireId(channel.ActualId, "Radio channel"),
            IsSubscribe = false
        }, ctk);
    }

    public static Task FollowAsync(this NeteaseUser user, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.UserFollowApi, new UserFollowRequest
        {
            Id = RequireId(user.ActualId, "User")
        }, ctk);
    }

    public static Task UnfollowAsync(this NeteaseUser user, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.UserUnfollowApi, new UserUnfollowRequest
        {
            Id = RequireId(user.ActualId, "User")
        }, ctk);
    }

    public static Task SubscribeMvAsync(string mvId, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.VideoSubscribeApi, new VideoSubscribeRequest
        {
            MvId = mvId
        }, ctk);
    }

    public static Task UnsubscribeMvAsync(string mvId, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.VideoUnsubscribeApi, new VideoUnsubscribeRequest
        {
            IdList = [mvId]
        }, ctk);
    }

    private static Task EditPlaylistTracksAsync(string? playlistId, string songId, bool isAdd, CancellationToken ctk)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.PlaylistTracksEditApi, new PlaylistTracksEditRequest
        {
            IsAdd = isAdd,
            PlaylistId = RequireId(playlistId, "Playlist"),
            Id = songId.StartsWith(NeteaseTypeIds.SingleSong) ? songId.Substring(2) : songId
        }, ctk);
    }

    private static string RequireId(string? id, string itemName)
    {
        if (id is null || string.IsNullOrWhiteSpace(id))
            throw new InvalidOperationException($"{itemName} does not have a valid NetEase ID.");

        return id;
    }
}

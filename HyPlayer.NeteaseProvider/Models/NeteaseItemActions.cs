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
            TrackId = song.ActualId,
            Like = true,
            UserId = NeteaseProvider.Instance.LoginedUser?.ActualId!
        }, ctk);
    }

    public static Task UnlikeAsync(this NeteaseSong song, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.LikeApi, new LikeRequest
        {
            TrackId = song.ActualId,
            Like = false,
            UserId = NeteaseProvider.Instance.LoginedUser?.ActualId!
        }, ctk);
    }

    public static Task AddSongAsync(this NeteasePlaylist playlist, string songId, CancellationToken ctk = default)
    {
        return EditPlaylistTracksAsync(playlist.ActualId, songId, true, ctk);
    }

    public static Task RemoveSongAsync(this NeteasePlaylist playlist, string songId, CancellationToken ctk = default)
    {
        return EditPlaylistTracksAsync(playlist.ActualId, songId, false, ctk);
    }

    public static Task SubscribeAsync(this NeteasePlaylist playlist, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.PlaylistSubscribeApi, new PlaylistSubscribeRequest
        {
            IsSubscribe = true,
            PlaylistId = playlist.ActualId
        }, ctk);
    }

    public static Task UnsubscribeAsync(this NeteasePlaylist playlist, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.PlaylistSubscribeApi, new PlaylistSubscribeRequest
        {
            IsSubscribe = false,
            PlaylistId = playlist.ActualId
        }, ctk);
    }

    public static Task SubscribeAsync(this NeteaseAlbum album, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.AlbumSubscribeApi, new AlbumSubscribeRequest
        {
            Id = album.ActualId,
            IsSubscribe = true
        }, ctk);
    }

    public static Task UnsubscribeAsync(this NeteaseAlbum album, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.AlbumSubscribeApi, new AlbumSubscribeRequest
        {
            Id = album.ActualId,
            IsSubscribe = false
        }, ctk);
    }

    public static Task SubscribeAsync(this NeteaseArtist artist, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.ArtistSubscribeApi, new ArtistSubscribeRequest
        {
            ArtistId = artist.ActualId
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
            Id = channel.ActualId,
            IsSubscribe = true
        }, ctk);
    }

    public static Task UnsubscribeAsync(this NeteaseRadioChannel channel, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.DjChannelSubscribeApi, new DjChannelSubscribeRequest
        {
            Id = channel.ActualId,
            IsSubscribe = false
        }, ctk);
    }

    public static Task FollowAsync(this NeteaseUser user, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.UserFollowApi, new UserFollowRequest
        {
            Id = user.ActualId
        }, ctk);
    }

    public static Task UnfollowAsync(this NeteaseUser user, CancellationToken ctk = default)
    {
        return NeteaseProvider.Instance.RequestAsync(NeteaseApis.UserUnfollowApi, new UserUnfollowRequest
        {
            Id = user.ActualId
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
            PlaylistId = playlistId,
            Id = songId.StartsWith(NeteaseTypeIds.SingleSong) ? songId[2..] : songId
        }, ctk);
    }
}

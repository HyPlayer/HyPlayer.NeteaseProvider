using HyPlayer.NeteaseApi.ApiContracts;

namespace HyPlayer.NeteaseApi;

public static class NeteaseApis
{
    public static LoginEmailApi LoginEmailApi => new();
    public static LoginCellphoneApi LoginCellphoneApi => new();
    public static SongDetailApi SongDetailApi => new();
    public static SongUrlApi SongUrlApi => new();
    public static PlaylistDetailApi PlaylistDetailApi => new();
    public static LyricApi LyricApi => new();
    public static LikeApi LikeApi => new();
    public static PlaylistTracksEditApi PlaylistTracksEditApi => new();
    public static ArtistSongsApi ArtistSongsApi => new();
    public static ArtistAlbumsApi ArtistAlbumsApi => new();
    public static PlaylistSubscribeApi PlaylistSubscribeApi => new();
    public static UserPlaylistApi UserPlaylistApi => new();
    public static LikelistApi LikelistApi => new();
    public static ToplistApi ToplistApi => new();
}
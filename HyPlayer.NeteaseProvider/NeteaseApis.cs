using HyPlayer.NeteaseProvider.ApiContracts;


namespace HyPlayer.NeteaseProvider;

public static class NeteaseApis
{
    public static LoginEmailApi LoginEmailApi => new();
    public static LoginCellphoneApi LoginCellphoneApi => new();
    public static SongDetailApi SongDetailApi => new();
    public static SongUrlApi SongUrlApi => new();
    public static PlaylistDetailApi PlaylistDetailApi => new();
}
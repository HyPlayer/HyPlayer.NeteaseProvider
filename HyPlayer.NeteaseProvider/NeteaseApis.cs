using HyPlayer.NeteaseProvider.ApiContracts;


namespace HyPlayer.NeteaseProvider;

public static class NeteaseApis
{
    public static LoginEmailApi LoginEmailApi => new LoginEmailApi();
    public static LoginCellphoneApi LoginCellphoneApi => new LoginCellphoneApi();
    public static SongDetailApi SongDetailApi => new SongDetailApi();
    public static SongUrlApi SongUrlApi => new SongUrlApi();
}
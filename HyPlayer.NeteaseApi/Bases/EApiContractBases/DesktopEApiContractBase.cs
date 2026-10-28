namespace HyPlayer.NeteaseApi.Bases.EApiContractBases;

public static class DesktopClientVersion
{
    public const string Fallback = "3.1.42.205594";

    private static string _current = Fallback;

    public static string Current
    {
        get => Volatile.Read(ref _current);
        set => Volatile.Write(ref _current, string.IsNullOrWhiteSpace(value) ? Fallback : value);
    }
}

public abstract class DesktopEApiContractBase<TRequest, TResponse, TError, TActualRequest> :
    EApiContractBase<TRequest, TResponse, TError, TActualRequest>
    where TActualRequest : EApiActualRequestBase
    where TError : ErrorResultBase
    where TRequest : RequestBase
    where TResponse : ResponseBase, new()
{
    public override Dictionary<string, string> Cookies { get; } = new()
    {
        ["os"] = "pc",
        ["appver"] = DesktopClientVersion.Current,
        ["osver"] = "Microsoft-Windows-10-Professional-build-19045-64bit",
        ["channel"] = "netease",
        ["deviceType"] = "pc",
        ["mobilename"] = string.Empty
    };

    public override string? UserAgent =>
        "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Safari/537.36 Chrome/91.0.4472.164 NeteaseMusicDesktop/" +
        DesktopClientVersion.Current;
}

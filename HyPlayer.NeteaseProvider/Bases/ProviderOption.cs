using System.Net;

namespace HyPlayer.NeteaseProvider.Bases;

public class ProviderOption
{
    public Dictionary<string, string> Cookies { get; } = new();
    // ReSharper disable once InconsistentNaming
    public string? XRealIP { get; set; } = null;
    public bool UseProxy { get; set; } = false;
    public IWebProxy? Proxy { get; set; } = null;
    public string? UserAgent { get; set; } = null;
    public bool DegradeHttp { get; set; } = false;

}
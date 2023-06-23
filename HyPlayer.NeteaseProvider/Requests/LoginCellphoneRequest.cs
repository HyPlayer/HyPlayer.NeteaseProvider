using HyPlayer.NeteaseProvider.Bases;

namespace HyPlayer.NeteaseProvider.Requests;

public class LoginCellphoneRequest : RequestBase
{
    public required string Cellphone { get; set; }
    public string CountryCode { get; set; } = "86";
    public string? Password { get; set; }
    public string? Md5Password { get; set; }
}
using HyPlayer.NeteaseProvider.Bases;

namespace HyPlayer.NeteaseProvider.Requests;

public class LoginEmailRequest : RequestBase
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
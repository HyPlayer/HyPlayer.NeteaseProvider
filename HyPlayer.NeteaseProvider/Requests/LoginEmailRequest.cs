﻿using HyPlayer.NeteaseProvider.Bases;

namespace HyPlayer.NeteaseProvider.Requests;

public class LoginEmailRequest : RequestBase
{
    public required string Email { get; set; }
    public string? Password { get; set; }
    public string? Md5Password { get; set; }
}
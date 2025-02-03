﻿using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.Bases;

public class EApiActualRequestBase : ActualRequestBase
{
    [JsonPropertyName("header")] public string? Header { get; set; }

    [JsonPropertyName("e_r")] public bool Error => true;
}
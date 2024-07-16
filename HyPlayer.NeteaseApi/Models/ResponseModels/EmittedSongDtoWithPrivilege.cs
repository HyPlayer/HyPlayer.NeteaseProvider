using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.Models.ResponseModels;

public class EmittedSongDtoWithPrivilege : EmittedSongDto
{
    [JsonPropertyName("privilege")] public PrivilegeDto? Privilege { get; set; }
}
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.Models.ResponseModels;

public class CloudMusicDto
{
    [JsonPropertyName("album")] public string? Album { get; set; }
    [JsonPropertyName("artist")] public string? Artist { get; set; }
    [JsonPropertyName("bitrate")] public int Bitrate { get; set; }
    [JsonPropertyName("songId")] public string? SongId { get; set; }
    [JsonPropertyName("songName")] public string? SongName { get; set; }
    [JsonPropertyName("addTime")] public long AddTime { get; set; }
    [JsonPropertyName("fileSize")] public long FileSize { get; set; }
    [JsonPropertyName("fileName")] public string? FileName { get; set; }
    [JsonPropertyName("simpleSong")] public EmittedSongDtoWithPrivilege? Song { get; set; }
}
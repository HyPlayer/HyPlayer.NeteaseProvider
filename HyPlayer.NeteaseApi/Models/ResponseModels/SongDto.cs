using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.Models.ResponseModels;

public class SongDto
{
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("alias")] public string[]? Alias { get; set; }
    [JsonPropertyName("duration")] public long Duration { get; set; }
    [JsonPropertyName("transName")] public string? Translation { get; set; }
    [JsonPropertyName("mvid")] public string? MvId { get; set; }
    [JsonPropertyName("disc")] public string? CdName { get; set; }
    [JsonPropertyName("no")] public int TrackNumber { get; set; }
    [JsonPropertyName("album")] public AlbumDto? Album { get; set; }
    [JsonPropertyName("s_id")] public string? Sid { get; set; }
    [JsonPropertyName("artists")] public ArtistDto[]? Artists { get; set; }
    [JsonPropertyName("videoInfo")] public SongInfoVideoInfoDto? VideoInfo { get; set; }

    public class SongInfoVideoInfoDto
    {
        [JsonPropertyName("video")] public SongInfoVideoDto? Video { get; set; }


        public class SongInfoVideoDto
        {
            [JsonPropertyName("vid")] public string? Vid { get; set; }
        }
    }
}

public class FlattedSongDto
{
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("coverUrl")] public string? CoverUrl { get; set; }
    [JsonPropertyName("duration")] public long Duration { get; set; }
    [JsonPropertyName("transName")] public string? Translation { get; set; }
    [JsonPropertyName("albumName")] public string? Album { get; set; }
    [JsonPropertyName("artists")] public FlattedArtistDto[]? Artists { get; set; }
    [JsonPropertyName("isLiked")] public bool IsLiked { get; set; }

    public class FlattedArtistDto
    {
        [JsonPropertyName("artistId")] public string? Id { get; set; }
        [JsonPropertyName("artistName")] public string? Name { get; set; }
    }

}

public class FlattedSongWithPrivilegeDto : FlattedSongDto
{
    [JsonPropertyName("privilege")] public PrivilegeDto? Privilege { get; set; }
}
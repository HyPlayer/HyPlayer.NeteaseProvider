using System.Text.Json.Serialization;
using HyPlayer.NeteaseProvider.Bases;

namespace HyPlayer.NeteaseProvider.Responses;

public class SongDetailResponse : CodedResponseBase
{
    [JsonPropertyName("songs")] public SongItem[] Songs { get; set; }
    [JsonPropertyName("privileges")] public PrivilegeItem[] Privileges { get; set; }

    public class SongItem
    {
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("id")] public string? Id { get; set; }
        [JsonPropertyName("dt")] public long Duration { get; set; }
        [JsonPropertyName("alia")] public string[]? Alias { get; set; }
        [JsonPropertyName("tns")] public string[]? Translations { get; set; }
        [JsonPropertyName("copyright")] public int Copyright { get; set; }
        [JsonPropertyName("mv")] public string? MvId { get; set; }
        [JsonPropertyName("cd")] public string? CdName { get; set; }
        [JsonPropertyName("no")] public int TrackNumber { get; set; }
        [JsonPropertyName("al")] public AlbumData? Album { get; set; }
        [JsonPropertyName("ar")] public ArtistItem[]? Artists { get; set; }


        public class ArtistItem
        {
            [JsonPropertyName("id")] public string? Id { get; set; }
            [JsonPropertyName("name")] public string? Name { get; set; }
        }

        public class AlbumData
        {
            [JsonPropertyName("id")] public string? Id { get; set; }
            [JsonPropertyName("name")] public string? Name { get; set; }
            [JsonPropertyName("picUrl")] public string? PictureUrl { get; set; }
        }
    }

    public class PrivilegeItem
    {
        [JsonPropertyName("id")] public string? id { get; set; }
        [JsonPropertyName("payed")] public int Payed { get; set; }
    }
}
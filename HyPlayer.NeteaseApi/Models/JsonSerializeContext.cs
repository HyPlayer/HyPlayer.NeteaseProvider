﻿using System.Text.Json.Serialization;
using HyPlayer.NeteaseApi.ApiContracts;

namespace HyPlayer.NeteaseApi.Serialization
{
    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(AiDjContentRcmdInfoResponse))]
    [JsonSerializable(typeof(AiDjSkipResponse))]
    [JsonSerializable(typeof(AlbumResponse))]
    [JsonSerializable(typeof(AlbumDetailDynamicResponse))]
    [JsonSerializable(typeof(AlbumSublistResponse))]
    [JsonSerializable(typeof(AlbumSubscribeResponse))]
    [JsonSerializable(typeof(ArtistAlbumsResponse))]
    [JsonSerializable(typeof(ArtistDetailResponse))]
    [JsonSerializable(typeof(ArtistSongsResponse))]
    [JsonSerializable(typeof(ArtistTopSongResponse))]
    [JsonSerializable(typeof(ArtistVideoResponse))]
    [JsonSerializable(typeof(CloudDeleteResponse))]
    [JsonSerializable(typeof(CloudGetResponse))]
    [JsonSerializable(typeof(CloudPubResponse))]
    [JsonSerializable(typeof(CloudUploadCheckResponse))]
    [JsonSerializable(typeof(CloudUploadCoverTokenAllocResponse))]
    [JsonSerializable(typeof(CloudUploadInfoResponse))]
    [JsonSerializable(typeof(CloudUploadTokenAllocResponse))]
    [JsonSerializable(typeof(CommentFloorResponse))]
    [JsonSerializable(typeof(CommentLikeResponse))]
    [JsonSerializable(typeof(CommentsResponse))]
    [JsonSerializable(typeof(DjChannelDetailResponse))]
    [JsonSerializable(typeof(DjChannelProgramsResponse))]
    [JsonSerializable(typeof(FmTrashResponse))]
    [JsonSerializable(typeof(LikeResponse))]
    [JsonSerializable(typeof(LikelistResponse))]
    [JsonSerializable(typeof(LoginResponse))]
    [JsonSerializable(typeof(LoginQrCodeCheckResponse))]
    [JsonSerializable(typeof(LoginQrCodeUnikeyResponse))]
    [JsonSerializable(typeof(LoginStatusResponse))]
    [JsonSerializable(typeof(LyricResponse))]
    [JsonSerializable(typeof(MusicFirstListenInfoResponse))]
    [JsonSerializable(typeof(PersonalFmResponse))]
    [JsonSerializable(typeof(PlaylistCategoryListResponse))]
    [JsonSerializable(typeof(PlaylistCreateResponse))]
    [JsonSerializable(typeof(PlaylistDeleteResponse))]
    [JsonSerializable(typeof(PlaylistDetailResponse))]
    [JsonSerializable(typeof(PlaylistPrivacyResponse))]
    [JsonSerializable(typeof(PlaylistSubscribeResponse))]
    [JsonSerializable(typeof(PlaylistTracksEditResponse))]
    [JsonSerializable(typeof(PlaylistTracksGetResponse))]
    [JsonSerializable(typeof(RecommendPlaylistsResponse))]
    [JsonSerializable(typeof(RecommendResourceResponse))]
    [JsonSerializable(typeof(RecommendSongsResponse))]
    [JsonSerializable(typeof(SearchResponse))]
    [JsonSerializable(typeof(SearchPlaylistResponse))]
    [JsonSerializable(typeof(SearchAlbumResponse))]
    [JsonSerializable(typeof(SearchUserResponse))]
    [JsonSerializable(typeof(SearchLyricResponse))]
    [JsonSerializable(typeof(SearchSongResponse))]
    [JsonSerializable(typeof(SearchMVResponse))]
    [JsonSerializable(typeof(SearchArtistResponse))]
    [JsonSerializable(typeof(SearchVideoResponse))]
    [JsonSerializable(typeof(SearchRadioResponse))]
    [JsonSerializable(typeof(SearchSuggestionResponse))]
    [JsonSerializable(typeof(SongDetailResponse))]
    [JsonSerializable(typeof(SongUrlResponse))]
    [JsonSerializable(typeof(SongWikiSummaryResponse))]
    [JsonSerializable(typeof(ToplistResponse))]
    [JsonSerializable(typeof(UserCloudResponse))]
    [JsonSerializable(typeof(UserCloudDeleteResponse))]
    [JsonSerializable(typeof(UserPlaylistResponse))]
    [JsonSerializable(typeof(UserRecordResponse))]
    [JsonSerializable(typeof(VideoDetailResponse))]
    [JsonSerializable(typeof(VideoUrlResponse))]
    [JsonSerializable(typeof(UserDetailResponse))]
    public partial class JsonSerializeContext : JsonSerializerContext
    {
    }
}

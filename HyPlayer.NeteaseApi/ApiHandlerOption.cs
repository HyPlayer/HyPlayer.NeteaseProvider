using HyPlayer.NeteaseApi.ApiContracts.Album;
using HyPlayer.NeteaseApi.ApiContracts.Artist;
using HyPlayer.NeteaseApi.ApiContracts.Cloud;
using HyPlayer.NeteaseApi.ApiContracts.Comment;
using HyPlayer.NeteaseApi.ApiContracts.DjChannel;
using HyPlayer.NeteaseApi.ApiContracts.ListenTogether;
using HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Dual;
using HyPlayer.NeteaseApi.ApiContracts.Login;
using HyPlayer.NeteaseApi.ApiContracts.PersonalFM;
using HyPlayer.NeteaseApi.ApiContracts.Playlist;
using HyPlayer.NeteaseApi.ApiContracts.Recommend;
using HyPlayer.NeteaseApi.ApiContracts.Song;
using HyPlayer.NeteaseApi.ApiContracts.User;
using HyPlayer.NeteaseApi.ApiContracts.Utils;
using HyPlayer.NeteaseApi.ApiContracts.Video;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Extensions.JsonSerializer;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi;

public class ApiHandlerOption
{
    public Dictionary<string, string> Cookies { get; } = new();

    // ReSharper disable once InconsistentNaming
    public string? XRealIP { get; set; } = null;
    public bool UseProxy { get; set; } = false;
    public IWebProxy? Proxy { get; set; } = null;
    public string? UserAgent { get; set; } = null;
    public bool DegradeHttp { get; set; } = false;
    /// <summary>
    /// 启用需要使用CheckToken的接口, 当 AdditionalParameters 被设定时不进行检查
    /// </summary>
    public bool BypassCheckTokenApi { get; set; } = false;

    public AdditionalParameters AdditionalParameters { get; set; } = new();

    public bool FakeCheckToken { get; set; }

    public JsonSerializerOptions JsonSerializerOptions =
        new(JsonSerializerOptions.Web)
        {
            NumberHandling = JsonNumberHandling.WriteAsString |
                             JsonNumberHandling.AllowReadingFromString,
            AllowTrailingCommas = true,
            Converters = { new NumberToStringConverter(), new JsonBooleanConverter(), new JsonObjectStringConverter() },
            TypeInfoResolver = DefaultContext.Default

        };
    public static readonly JsonSerializerOptions JsonSerializerOptionsOnlyTypeInfo =
        new(JsonSerializerOptions.Default)
        {
            TypeInfoResolver = DefaultContext.Default
        };
    public static readonly JsonSerializerOptions JsonSerializerOptionsWebOnlyTypeInfo =
        new(JsonSerializerOptions.Web)
        {
            TypeInfoResolver = DefaultContext.Default
        };
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSerializable(typeof(Dictionary<string, int>))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(ErrorResultBase))]
[JsonSerializable(typeof(AlbumActualRequest))]
[JsonSerializable(typeof(AlbumResponse))]
[JsonSerializable(typeof(AlbumDetailDynamicActualRequest))]
[JsonSerializable(typeof(AlbumDetailDynamicResponse))]
[JsonSerializable(typeof(AlbumSublistActualRequest))]
[JsonSerializable(typeof(AlbumSublistResponse))]
[JsonSerializable(typeof(AlbumSubscribeActualRequest))]
[JsonSerializable(typeof(AlbumSubscribeResponse))]
[JsonSerializable(typeof(ArtistAlbumsActualRequest))]
[JsonSerializable(typeof(ArtistAlbumsResponse))]
[JsonSerializable(typeof(ArtistDetailActualRequest))]
[JsonSerializable(typeof(ArtistDetailResponse))]
[JsonSerializable(typeof(ArtistSongsActualRequest))]
[JsonSerializable(typeof(ArtistSongsResponse))]
[JsonSerializable(typeof(ArtistSublistActualRequest))]
[JsonSerializable(typeof(ArtistSublistResponse))]
[JsonSerializable(typeof(ArtistTopSongActualRequest))]
[JsonSerializable(typeof(ArtistTopSongResponse))]
[JsonSerializable(typeof(ArtistVideoActualRequest))]
[JsonSerializable(typeof(ArtistVideoResponse))]
[JsonSerializable(typeof(CloudDeleteActualRequest))]
[JsonSerializable(typeof(CloudDeleteResponse))]
[JsonSerializable(typeof(CloudGetActualRequest))]
[JsonSerializable(typeof(CloudGetResponse))]
[JsonSerializable(typeof(CloudPubActualRequest))]
[JsonSerializable(typeof(CloudPubResponse))]
[JsonSerializable(typeof(CloudUploadCheckActualRequest))]
[JsonSerializable(typeof(CloudUploadCheckResponse))]
[JsonSerializable(typeof(CloudUploadCoverTokenAllocActualRequest))]
[JsonSerializable(typeof(CloudUploadCoverTokenAllocResponse))]
[JsonSerializable(typeof(CloudUploadInfoActualRequest))]
[JsonSerializable(typeof(CloudUploadInfoResponse))]
[JsonSerializable(typeof(CloudUploadTokenAllocActualRequest))]
[JsonSerializable(typeof(CloudUploadTokenAllocResponse))]
[JsonSerializable(typeof(NeteaseUploadLoadBalancerGetActualRequest))]
[JsonSerializable(typeof(NeteaseUploadLoadBalancerGetResponse))]
[JsonSerializable(typeof(UserCloudActualRequest))]
[JsonSerializable(typeof(UserCloudResponse))]
[JsonSerializable(typeof(UserCloudDeleteActualRequest))]
[JsonSerializable(typeof(UserCloudDeleteResponse))]
[JsonSerializable(typeof(CommentFloorActualRequest))]
[JsonSerializable(typeof(CommentFloorResponse))]
[JsonSerializable(typeof(CommentLikeActualRequest))]
[JsonSerializable(typeof(CommentLikeResponse))]
[JsonSerializable(typeof(CommentsActualRequest))]
[JsonSerializable(typeof(CommentsResponse))]
[JsonSerializable(typeof(DjChannelDetailActualRequest))]
[JsonSerializable(typeof(DjChannelDetailResponse))]
[JsonSerializable(typeof(DjChannelProgramsActualRequest))]
[JsonSerializable(typeof(DjChannelProgramsResponse))]
[JsonSerializable(typeof(DjChannelSubscribedActualRequest))]
[JsonSerializable(typeof(DjChannelSubscribedResponse))]
[JsonSerializable(typeof(ListenTogetherEndActualRequest))]
[JsonSerializable(typeof(ListenTogetherEndResponse))]
[JsonSerializable(typeof(ListenTogetherHeartBeatActualRequest))]
[JsonSerializable(typeof(ListenTogetherHeartBeatResponse))]
[JsonSerializable(typeof(ListenTogetherInvitationAcceptActualRequest))]
[JsonSerializable(typeof(ListenTogetherInvitationAcceptResponse))]
[JsonSerializable(typeof(ListenTogetherPlayCommandActualRequest))]
[JsonSerializable(typeof(ListenTogetherPlayCommandResponse))]
[JsonSerializable(typeof(ListenTogetherRoomCheckActualRequest))]
[JsonSerializable(typeof(ListenTogetherRoomCheckResponse))]
[JsonSerializable(typeof(ListenTogetherRoomCreateActualRequest))]
[JsonSerializable(typeof(ListenTogetherRoomCreateResponse))]
[JsonSerializable(typeof(ListenTogetherStatusActualRequest))]
[JsonSerializable(typeof(ListenTogetherStatusResponse))]
[JsonSerializable(typeof(ListenTogetherSyncListReportActualRequest))]
[JsonSerializable(typeof(ListenTogetherSyncListReportResponse))]
[JsonSerializable(typeof(ListenTogetherSyncListGetActualRequest))]
[JsonSerializable(typeof(ListenTogetherSyncListGetResponse))]
[JsonSerializable(typeof(LoginCellphoneActualRequest))]
[JsonSerializable(typeof(LoginResponse))]
[JsonSerializable(typeof(LoginEmailActualRequest))]
[JsonSerializable(typeof(LoginQrCodeCheckActualRequest))]
[JsonSerializable(typeof(LoginQrCodeCheckResponse))]
[JsonSerializable(typeof(LoginQrCodeUnikeyActualRequest))]
[JsonSerializable(typeof(LoginQrCodeUnikeyResponse))]
[JsonSerializable(typeof(LoginStatusActualRequest))]
[JsonSerializable(typeof(LoginStatusResponse))]
[JsonSerializable(typeof(AiDjContentRcmdInfoActualRequest))]
[JsonSerializable(typeof(AiDjContentRcmdInfoResponse))]
[JsonSerializable(typeof(AiDjSkipActualRequest))]
[JsonSerializable(typeof(AiDjSkipResponse))]
[JsonSerializable(typeof(PersonalFmActualRequest))]
[JsonSerializable(typeof(PersonalFmResponse))]
[JsonSerializable(typeof(FmTrashActualRequest))]
[JsonSerializable(typeof(FmTrashResponse))]
[JsonSerializable(typeof(LikelistActualRequest))]
[JsonSerializable(typeof(LikelistResponse))]
[JsonSerializable(typeof(PlaylistCategoryListActualRequest))]
[JsonSerializable(typeof(PlaylistCategoryListResponse))]
[JsonSerializable(typeof(PlaylistCreateActualRequest))]
[JsonSerializable(typeof(PlaylistCreateResponse))]
[JsonSerializable(typeof(PlaylistDeleteActualRequest))]
[JsonSerializable(typeof(PlaylistDeleteResponse))]
[JsonSerializable(typeof(PlaylistDetailActualRequest))]
[JsonSerializable(typeof(PlaylistDetailResponse))]
[JsonSerializable(typeof(PlaylistPrivacyActualRequest))]
[JsonSerializable(typeof(PlaylistPrivacyResponse))]
[JsonSerializable(typeof(PlaylistSubscribeActualRequest))]
[JsonSerializable(typeof(PlaylistSubscribeResponse))]
[JsonSerializable(typeof(PlaylistTracksEditActualRequest))]
[JsonSerializable(typeof(PlaylistTracksEditResponse))]
[JsonSerializable(typeof(PlaylistTracksGetActualRequest))]
[JsonSerializable(typeof(PlaylistTracksGetResponse))]
[JsonSerializable(typeof(PlaymodeIntelligenceListActualRequest))]
[JsonSerializable(typeof(PlaymodeIntelligenceListResponse))]
[JsonSerializable(typeof(ToplistActualRequest))]
[JsonSerializable(typeof(ToplistResponse))]
[JsonSerializable(typeof(RecommendPlaylistsActualRequest))]
[JsonSerializable(typeof(RecommendPlaylistsResponse))]
[JsonSerializable(typeof(RecommendResourceActualRequest))]
[JsonSerializable(typeof(RecommendResourceResponse))]
[JsonSerializable(typeof(RecommendSongsActualRequest))]
[JsonSerializable(typeof(RecommendSongsResponse))]
[JsonSerializable(typeof(SearchActualRequest))]
[JsonSerializable(typeof(SearchResponse))]
[JsonSerializable(typeof(SearchSuggestionActualRequest))]
[JsonSerializable(typeof(SearchSuggestionResponse))]
[JsonSerializable(typeof(LikeActualRequest))]
[JsonSerializable(typeof(LikeResponse))]
[JsonSerializable(typeof(LyricActualRequest))]
[JsonSerializable(typeof(LyricResponse))]
[JsonSerializable(typeof(MusicFirstListenInfoActualRequest))]
[JsonSerializable(typeof(MusicFirstListenInfoResponse))]
[JsonSerializable(typeof(SongChorusActualRequest))]
[JsonSerializable(typeof(SongChorusResponse))]
[JsonSerializable(typeof(SongDetailActualRequest))]
[JsonSerializable(typeof(SongDetailResponse))]
[JsonSerializable(typeof(SongUrlActualRequest))]
[JsonSerializable(typeof(SongUrlResponse))]
[JsonSerializable(typeof(SongWikiSummaryActualRequest))]
[JsonSerializable(typeof(SongWikiSummaryResponse))]
[JsonSerializable(typeof(UserDetailActualRequest))]
[JsonSerializable(typeof(UserDetailResponse))]
[JsonSerializable(typeof(UserPlaylistActualRequest))]
[JsonSerializable(typeof(UserPlaylistResponse))]
[JsonSerializable(typeof(UserRecordActualRequest))]
[JsonSerializable(typeof(UserRecordResponse))]
[JsonSerializable(typeof(BatchActualRequest))]
[JsonSerializable(typeof(BatchResponse))]
[JsonSerializable(typeof(LoginAnnounceDeviceActualRequest))]
[JsonSerializable(typeof(LoginAnnounceDeviceResponse))]
[JsonSerializable(typeof(PasswordUrlDecodeActualRequest))]
[JsonSerializable(typeof(PasswordUrlDecodeResponse))]
[JsonSerializable(typeof(RegisterAnonymousActualRequest))]
[JsonSerializable(typeof(RegisterAnonymousResponse))]
[JsonSerializable(typeof(MlogDetailActualRequest))]
[JsonSerializable(typeof(MlogDetailResponse))]
[JsonSerializable(typeof(MlogRcmdFeedListActualRequest))]
[JsonSerializable(typeof(MlogRcmdFeedListResponse))]
[JsonSerializable(typeof(MlogUrlActualRequest))]
[JsonSerializable(typeof(MlogUrlResponse))]
[JsonSerializable(typeof(VideoDetailActualRequest))]
[JsonSerializable(typeof(VideoDetailResponse))]
[JsonSerializable(typeof(VideoUrlActualRequest))]
[JsonSerializable(typeof(VideoUrlResponse))]
public partial class DefaultContext : JsonSerializerContext
{ 
}

public class AdditionalParameters
{
    public Dictionary<string, string?> Cookies { get; set; } = [];
    public Dictionary<string, string?> Headers { get; set; } = [];
    public Dictionary<string, string?> EApiHeaders { get; set; } = [];
    public Dictionary<string, string?> DataTokens { get; set; } = [];
    public OpenAPIConfigData? OpenAPIConfig { get; set; } = null;
    public bool HasValue()
    {
        if (Cookies.Count > 0 ||
            Headers.Count > 0 ||
            EApiHeaders.Count > 0 ||
            DataTokens.Count > 0) return true;
        return false;
    }


    public class OpenAPIConfigData
    {
        public string? AppId { get; set; }
        public string? AppSecret { get; set; }
        public string? RsaPrivateKey { get; set; }
        public DeviceInfoData? DeviceInfo { get; set; }

        public class DeviceInfoData
        {
            public string? Channel { get; set; }
            public string? DeviceId { get; set; }
            public string? DeviceType { get; set; }
            public string? AppVer { get; set; }
            public string? OS { get; set; }
            public string? OSVer { get; set; }
            public string? Brand { get; set; }
            public string? Model { get; set; }
            public string? ClientIp { get; set; }
        }
    }
}
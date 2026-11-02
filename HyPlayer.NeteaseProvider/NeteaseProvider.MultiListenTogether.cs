using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.ApiContracts.ListenTogether.Multi;
using HyPlayer.PlayCore.Abstraction.Models;
using System.Linq;
using System.Text.RegularExpressions;

namespace HyPlayer.NeteaseProvider;

public partial class NeteaseProvider
{
    private const string CheckTokenUrl = "https://ac.dun.163yun.com/v3/b?pn=YD00000558929251";
    private const int MultiRoomExpiredCode = 488;
    private static readonly Regex CheckTokenRegex = new(@"null\(\[(\d+),\d+,""([^""]+)""\]\)", RegexOptions.CultureInvariant);

    public async Task<ProviderMultiListenTogetherRoom> CreateMultiListenTogetherRoomAsync(string seedItemId,
        CancellationToken ctk = default)
    {
        await EnsureDesktopClientVersionAsync(ctk);
        var token = await GetCheckTokenAsync(ctk);
        var result = await RequestAsync(NeteaseApis.MultiListenTogetherRoomCreateApi,
            new MultiListenTogetherRoomCreateRequest { SongId = seedItemId, CheckToken = token }, ctk);
        return result.Match(success =>
            {
                EnsureListenTogetherSuccess(success.Code, success.Message);
                if (success.Data is not { Success: true, Snapshot: { } snapshot } data)
                    throw new InvalidOperationException(success.Data?.FailedMessage is { Length: > 0 } message
                        ? message
                        : "创建多人一起听失败");
                return MapMultiRoom(snapshot, data.Permission, null);
            },
            error => throw error);
    }

    public async Task<ProviderMultiListenTogetherRoom> JoinMultiListenTogetherRoomAsync(string roomId, string inviterId,
        CancellationToken ctk = default)
    {
        await EnsureDesktopClientVersionAsync(ctk);
        var token = await GetCheckTokenAsync(ctk);
        var result = await RequestAsync(NeteaseApis.MultiListenTogetherJoinApi,
            new MultiListenTogetherJoinRequest { RoomId = roomId, InviterId = inviterId, CheckToken = token }, ctk);
        var joined = result.Match(success =>
            {
                EnsureListenTogetherSuccess(success.Code, success.Message);
                if (success.Data?.Success is false)
                    throw new InvalidOperationException(success.Data.FailedMessage is { Length: > 0 } message
                        ? message
                        : "加入多人一起听失败");
                if (success.Data?.RoomId is { Length: > 0 } joinedRoom && joinedRoom != roomId)
                    throw new InvalidOperationException("加入的房间与邀请不一致");
                return success.Data?.Snapshot is { } snapshot ? MapMultiRoom(snapshot, success.Data.Permission, roomId) : null;
            },
            error => throw error);
        if (joined is { Playback: not null }) return joined;
        return await GetMultiListenTogetherRoomAsync(ctk) is { } room && room.RoomId == roomId
            ? room
            : throw new InvalidOperationException("没有进入这个多人一起听房间");
    }

    public async Task<ProviderMultiListenTogetherRoom?> GetMultiListenTogetherRoomAsync(CancellationToken ctk = default)
    {
        var result = await RequestAsync(NeteaseApis.MultiListenTogetherStatusApi, new MultiListenTogetherStatusRequest(), ctk);
        return result.Match(success =>
            {
                EnsureListenTogetherSuccess(success.Code, success.Message);
                return success.Data?.Snapshot is { } snapshot && !string.IsNullOrEmpty(snapshot.RoomId ?? snapshot.RoomInfo?.RoomId)
                    ? MapMultiRoom(snapshot, success.Data.Permission, null)
                    : null;
            },
            error => throw error);
    }

    public async Task<ProviderMultiListenTogetherHeartbeat> SendMultiListenTogetherHeartbeatAsync(string roomId,
        CancellationToken ctk = default)
    {
        var result = await RequestAsync(NeteaseApis.MultiListenTogetherHeartbeatApi,
            new MultiListenTogetherHeartbeatRequest { RoomId = roomId }, ctk);
        return result.Match(success =>
            {
                EnsureListenTogetherSuccess(success.Code, success.Message);
                return new ProviderMultiListenTogetherHeartbeat
                {
                    Playback = MapMultiPlayback(success.Data?.PlayInfo),
                    Interval = success.Data?.HeartbeatSeconds is int seconds and > 0
                        ? TimeSpan.FromSeconds(seconds)
                        : null,
                    IsClosed = success.Data?.RoomInfo?.RoomClosed is true
                };
            },
            error => error.ErrorCode == MultiRoomExpiredCode
                ? new ProviderMultiListenTogetherHeartbeat { IsClosed = true }
                : throw error);
    }

    public async Task<List<ProviderMultiListenTogetherQueueItem>> GetMultiListenTogetherQueueAsync(string roomId,
        CancellationToken ctk = default)
    {
        var result = await RequestAsync(NeteaseApis.MultiListenTogetherWaitListApi,
            new MultiListenTogetherWaitListRequest { RoomId = roomId }, ctk);
        return result.Match(success =>
            {
                EnsureListenTogetherSuccess(success.Code, success.Message);
                return success.Data?.Songs?
                    .Where(song => song.SongInfo is { ResourceId.Length: > 0, BizId.Length: > 0 })
                    .Select(song => new ProviderMultiListenTogetherQueueItem
                    {
                        ItemId = song.SongInfo!.ResourceId!,
                        EntryId = song.SongInfo.BizId!,
                        Title = song.SongInfo.Title ?? string.Empty,
                        Artists = string.Join(" / ", song.SongInfo.ArtistNames ?? []),
                        CoverUrl = song.SongInfo.CoverUrl,
                        RecommenderId = song.RecommenderId,
                        RecommenderName = song.Nickname ?? string.Empty,
                        IsSystemRecommendation = song.RecommendType == 3,
                        IsPlaying = song.Status == 1
                    })
                    .ToList() ?? [];
            },
            error => throw error);
    }

    public Task<ProviderMultiListenTogetherOperation> RecommendMultiListenTogetherItemAsync(string roomId, string itemId,
        CancellationToken ctk = default)
    {
        return OperateMultiListenTogetherSongAsync(roomId, itemId, "0", MultiListenTogetherSongOperation.Add, ctk);
    }

    public Task<ProviderMultiListenTogetherOperation> RequestMultiListenTogetherSkipAsync(string roomId, string itemId,
        string entryId, CancellationToken ctk = default)
    {
        return OperateMultiListenTogetherSongAsync(roomId, itemId, entryId, MultiListenTogetherSongOperation.Switch, ctk);
    }

    public async Task LeaveMultiListenTogetherRoomAsync(string roomId, CancellationToken ctk = default)
    {
        var result = await RequestAsync(NeteaseApis.MultiListenTogetherExitApi,
            new MultiListenTogetherExitRequest { RoomId = roomId }, ctk);
        _ = result.Match(success =>
            {
                EnsureListenTogetherSuccess(success.Code, success.Message);
                return success.Data?.Success is not false
                    ? true
                    : throw new InvalidOperationException("网易云没有确认退出多人一起听");
            },
            error => throw error);
    }

    private async Task<ProviderMultiListenTogetherOperation> OperateMultiListenTogetherSongAsync(string roomId,
        string itemId, string entryId, MultiListenTogetherSongOperation operation, CancellationToken ctk)
    {
        var token = await GetCheckTokenAsync(ctk);
        var result = await RequestAsync(NeteaseApis.MultiListenTogetherSongOperateApi,
            new MultiListenTogetherSongOperateRequest
            {
                RoomId = roomId,
                SongId = itemId,
                BizId = entryId,
                Operate = operation,
                CheckToken = token
            }, ctk);
        return result.Match(success =>
            {
                EnsureListenTogetherSuccess(success.Code, success.Message);
                var succeeded = success.Data?.FailedCode is null or 0 && success.Data?.Result is not false;
                return new ProviderMultiListenTogetherOperation
                {
                    Succeeded = succeeded,
                    Message = success.Data?.Message ?? (succeeded ? string.Empty : "操作失败"),
                    Playback = MapMultiPlayback(success.Data?.PlayInfo)
                };
            },
            error => throw error);
    }

    private async Task<string> GetCheckTokenAsync(CancellationToken ctk)
    {
        using var response = await Handler.HttpClient.GetAsync(new Uri(CheckTokenUrl), ctk);
        var text = await response.Content.ReadAsStringAsync();
        var match = CheckTokenRegex.Match(text);
        if (!response.IsSuccessStatusCode || !match.Success || match.Groups[1].Value != "200")
            throw new InvalidOperationException("无法获取网易云安全校验，请稍后再试");
        return match.Groups[2].Value;
    }

    private static ProviderMultiListenTogetherRoom MapMultiRoom(MultiListenTogetherSnapshot snapshot,
        MultiListenTogetherPermission? permission, string? fallbackRoomId)
    {
        return new ProviderMultiListenTogetherRoom
        {
            RoomId = snapshot.RoomId ?? snapshot.RoomInfo?.RoomId ?? fallbackRoomId ?? string.Empty,
            CreatorId = snapshot.RoomInfo?.CreatorId,
            Users = snapshot.Users?.OnlineUsers?
                .Where(user => !string.IsNullOrEmpty(user.UserId))
                .Select(user => new ProviderListenTogetherUser
                {
                    UserId = user.UserId!,
                    Nickname = user.Nickname ?? string.Empty,
                    AvatarUrl = user.AvatarUrl ?? string.Empty
                })
                .ToList() ?? [],
            Playback = MapMultiPlayback(snapshot.PlayInfo),
            CanRecommend = permission?.AddSong is not false,
            CanRequestSkip = permission?.SwitchSong is not false
        };
    }

    private static ProviderMultiListenTogetherPlayback? MapMultiPlayback(MultiListenTogetherPlayInfo? info)
    {
        if (info?.PlaySong is not { SongId.Length: > 0, SongBizId.Length: > 0 } song) return null;
        return new ProviderMultiListenTogetherPlayback
        {
            ItemId = song.SongId!,
            EntryId = song.SongBizId!,
            RecommenderId = song.RecommenderId,
            Position = TimeSpan.FromMilliseconds(Math.Max(0, info.PlayedTime ?? 0)),
            Duration = TimeSpan.FromMilliseconds(Math.Max(0, info.SongDuration ?? 0)),
            Version = info.Version ?? 0,
            NextItemIds = info.NextSongs?
                .Select(next => next.SongId)
                .Where(id => !string.IsNullOrEmpty(id))
                .Select(id => id!)
                .ToList() ?? []
        };
    }
}

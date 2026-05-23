using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.ApiContracts.Album;
using HyPlayer.NeteaseApi.ApiContracts.Artist;
using HyPlayer.NeteaseApi.ApiContracts.Cloud;
using HyPlayer.NeteaseApi.ApiContracts.DjChannel;
using HyPlayer.NeteaseApi.ApiContracts.Playlist;
using HyPlayer.NeteaseApi.ApiContracts.User;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.PlayCore.Abstraction.Interfaces.PlayListContainer;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;

namespace HyPlayer.NeteaseProvider.Models;

public class NeteaseUserLibrarySubContainer : LinerContainerBase, IProgressiveLoadingContainer
{
    public const string CloudKind = "cloud";
    public const string ListeningHistoryRecentKind = "history-recent";
    public const string ListeningHistoryAllKind = "history-all";
    public const string LikedSongsKind = "liked-songs";

    public override string ProviderId => "ncm";
    public override string TypeId => Kind;

    public required string Kind { get; init; }
    public string? UserId { get; init; }
    public int MaxProgressiveCount { get; init; } = 200;

    public override async Task<List<ProvidableItemBase>> GetAllItemsAsync(CancellationToken ctk = default)
    {
        var (_, items) = await GetProgressiveItemsListAsync(0, MaxProgressiveCount, ctk);
        return items;
    }

    public async Task<(bool, List<ProvidableItemBase>)> GetProgressiveItemsListAsync(int start, int count, CancellationToken ctk = default)
    {
        return Kind switch
        {
            CloudKind => await GetCloudItemsAsync(start, count, ctk),
            NeteaseTypeIds.RadioChannel => await GetSubscribedRadioChannelsAsync(count, ctk),
            NeteaseTypeIds.Artist => await GetSubscribedArtistsAsync(start, count, ctk),
            NeteaseTypeIds.Album => await GetSubscribedAlbumsAsync(start, count, ctk),
            ListeningHistoryRecentKind => (false, await GetListeningHistoryAsync(UserRecordType.WeekData, ctk)),
            ListeningHistoryAllKind => (false, await GetListeningHistoryAsync(UserRecordType.All, ctk)),
            LikedSongsKind => (false, await GetLikedSongsAsync(ctk)),
            _ => (false, [])
        };
    }

    public async Task DeleteCloudItemAsync(string itemId, CancellationToken ctk = default)
    {
        if (Kind != CloudKind)
            throw new InvalidOperationException("Only cloud library containers can delete cloud items.");

        await NeteaseProvider.Instance.RequestAsync(NeteaseApis.CloudDeleteApi, new CloudDeleteRequest { Id = itemId }, ctk);
    }

    private static async Task<(bool HasMore, List<ProvidableItemBase> Items)> GetCloudItemsAsync(int offset, int count, CancellationToken ctk)
    {
        var result = await NeteaseProvider.Instance.RequestAsync(NeteaseApis.CloudGetApi,
            new CloudGetRequest { Offset = offset, Limit = count }, ctk);

        return result.Match(success =>
            (success.HasMore,
             success.Songs?.Select(song => (ProvidableItemBase)song.MapToNeteaseCloudLibraryItem()).ToList() ?? []),
            _ => (false, []));
    }

    private static async Task<(bool HasMore, List<ProvidableItemBase> Items)> GetSubscribedRadioChannelsAsync(int count, CancellationToken ctk)
    {
        var result = await NeteaseProvider.Instance.RequestAsync(NeteaseApis.DjChannelSubscribedApi,
            new DjChannelSubscribedRequest { Limit = count }, ctk);

        return result.Match(success =>
            (success.Data?.HasMore ?? false,
             success.Data?.Data?.Select(channel => (ProvidableItemBase)new NeteaseRadioChannel
             {
                 ActualId = channel.Id,
                 Name = channel.Name ?? channel.Id ?? string.Empty,
                 Description = channel.Description,
                 CoverUrl = channel.CoverUrl,
                 LastProgramCreateTime = channel.LastProgramCreateTime,
                 LastProgramId = channel.LastProgramId,
                 LastProgramName = channel.LastVoiceName,
                 ProgramCount = channel.VoiceCount,
                 SubscribedCount = channel.SubscribedCount,
                 PlayCount = channel.PlayCount,
                 CategoryId = channel.CategoryId,
                 Category = channel.Category,
                 SecondCategoryId = channel.SecondCategoryId,
                 SecondCategory = channel.SecondCategory,
                 CreatorList = string.IsNullOrWhiteSpace(channel.UserName) ? [] : [channel.UserName]
             }).ToList() ?? []),
            _ => (false, []));
    }

    private static async Task<(bool HasMore, List<ProvidableItemBase> Items)> GetSubscribedArtistsAsync(int offset, int count, CancellationToken ctk)
    {
        var result = await NeteaseProvider.Instance.RequestAsync(NeteaseApis.ArtistSublistApi,
            new ArtistSublistRequest { Offset = offset, Limit = count }, ctk);

        return result.Match(success =>
            (success.HasMore,
             success.Artists?.Select(artist => (ProvidableItemBase)artist.MapToNeteaseArtist()).ToList() ?? []),
            _ => (false, []));
    }

    private static async Task<(bool HasMore, List<ProvidableItemBase> Items)> GetSubscribedAlbumsAsync(int offset, int count, CancellationToken ctk)
    {
        var result = await NeteaseProvider.Instance.RequestAsync(NeteaseApis.AlbumSublistApi,
            new AlbumSublistRequest { Offset = offset, Limit = count }, ctk);

        return result.Match(success =>
            (success.HasMore,
             success.Data?.Select(album => album.MapToNeteaseAlbum()).Where(album => album is not null).Cast<ProvidableItemBase>().ToList() ?? []),
            _ => (false, []));
    }

    private async Task<List<ProvidableItemBase>> GetListeningHistoryAsync(UserRecordType recordType, CancellationToken ctk)
    {
        var userId = UserId ?? NeteaseProvider.Instance.LoginedUser?.ActualId ?? string.Empty;
        if (recordType == UserRecordType.WeekData)
        {
            var result = await NeteaseProvider.Instance.RequestAsync<UserRecordWeekResponse, UserRecordRequest, UserRecordResponse, ErrorResultBase, UserRecordActualRequest>(
                NeteaseApis.UserRecordApi,
                new UserRecordRequest { UserId = userId, RecordType = UserRecordType.WeekData, Count = 120 }, ctk);
            return result.Match(
                success => success.WeekData?.Select(item => item.Song).Where(song => song is not null).Select(song => (ProvidableItemBase)song!.MapToNeteaseMusic()).ToList() ?? [],
                _ => []);
        }

        var allResult = await NeteaseProvider.Instance.RequestAsync<UserRecordAllResponse, UserRecordRequest, UserRecordResponse, ErrorResultBase, UserRecordActualRequest>(
            NeteaseApis.UserRecordApi,
            new UserRecordRequest { UserId = userId, RecordType = UserRecordType.All, Count = 120 }, ctk);
        return allResult.Match(
            success => success.AllData?.Select(item => item.Song).Where(song => song is not null).Select(song => (ProvidableItemBase)song!.MapToNeteaseMusic()).ToList() ?? [],
            _ => []);
    }

    private static async Task<List<ProvidableItemBase>> GetLikedSongsAsync(CancellationToken ctk)
    {
        var result = await NeteaseProvider.Instance.RequestAsync(NeteaseApis.LikelistApi, new LikelistRequest
        {
            Uid = NeteaseProvider.Instance.LoginedUser?.ActualId!
        }, ctk);

        var ids = result.Match(
            success => success.TrackIds?.Select(id => NeteaseTypeIds.SingleSong + id).ToList() ?? [],
            _ => []);

        return await NeteaseProvider.Instance.GetProvidableItemsRangeAsync(ids, ctk);
    }
}

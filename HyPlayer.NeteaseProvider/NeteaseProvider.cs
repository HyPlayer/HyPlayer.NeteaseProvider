using HyPlayer.NeteaseApi;
using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.ApiContracts.Login;
using HyPlayer.NeteaseApi.ApiContracts.Playlist;
using HyPlayer.NeteaseApi.ApiContracts.Artist;
using HyPlayer.NeteaseApi.ApiContracts.Album;
using HyPlayer.NeteaseApi.ApiContracts.Video;
using HyPlayer.NeteaseApi.ApiContracts.User;
using HyPlayer.NeteaseApi.ApiContracts.Recommend;
using HyPlayer.NeteaseApi.ApiContracts.Song;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Bases.ApiContractBases;
using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.NeteaseProvider.Models;
using HyPlayer.PlayCore.Abstraction;
using HyPlayer.PlayCore.Abstraction.Interfaces.Provider;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Lyric;
using HyPlayer.PlayCore.Abstraction.Models.Resources;
using HyPlayer.PlayCore.Abstraction.Models.SingleItems;
using HyPlayer.NeteaseApi.Extensions;
using HyPlayer.NeteaseApi.ApiContracts.DjChannel;
using System.Collections.Generic;
using System.Linq;

namespace HyPlayer.NeteaseProvider;

public partial class NeteaseProvider : ProviderBase,
                               ILyricProvidable,
                               IMusicResourceProvidable,
                                 IProvidableItemProvidable,
                                IProvidableItemRangeProvidable,
                                ISearchableProvider,
                                 IAuthenticationProvidable,
                                IContainerManagementProvidable,
                                ISearchSuggestionProvidable,
                                 IContainerPageProvidable,
                                 IQrAuthenticationProvidable,
                                 ICommentProvidable,
                                  IProvidableItemCommentProvidable,
                                   IListenTogetherProvidable,
                                  ICloudUploadProvidable,
                                  IRichMediaProvidable,
                                  IProvidableItemDynamicMetadataProvidable
{
    public readonly NeteaseCloudMusicApiHandler Handler = new();

    public void ConfigureAdditionalParameters(AdditionalParameters additionalParameters)
    {
        Handler.Option.AdditionalParameters = additionalParameters;
    }

    public void ConfigureFakeCheckToken(bool enabled)
    {
        Handler.Option.FakeCheckToken = enabled;
    }

    public void ConfigureXRealIP(string? xRealIp)
    {
        Handler.Option.XRealIP = xRealIp;
    }

    public void ConfigureDegradeHttp(bool enabled)
    {
        Handler.Option.DegradeHttp = enabled;
    }

    public bool HasAdditionalCookies => Handler.Option.AdditionalParameters.Cookies.Count > 0;

    public Dictionary<string, string> GetRuntimeCookiesSnapshot()
    {
        return Handler.Option.Cookies.ToDictionary();
    }

    public void ClearRuntimeCookies()
    {
        Handler.Option.Cookies.Clear();
    }

    public void SetRuntimeCookie(string name, string value)
    {
        Handler.Option.Cookies[name] = value;
    }
    public override string Name => "网易云音乐";
    public override string Id => "ncm";

    public static NeteaseProvider Instance = null!;

    public NeteaseUser? LoginedUser { get; set; }

    public NeteaseProvider()
    {
        Instance = this;
    }

    public override List<ProvidableTypeId> ProvidableTypeIds =>
        new()
        {
            new(NeteaseTypeIds.SingleSong, "歌曲", true),
            new(NeteaseTypeIds.Playlist, "歌单", true),
            new(NeteaseTypeIds.Artist, "歌手", true),
            new(NeteaseTypeIds.Album, "专辑", true),
            new(NeteaseTypeIds.User, "用户", true),
            new(NeteaseTypeIds.RadioProgram, "节目", true), // 电台节目
            new(NeteaseTypeIds.RadioChannel, "电台", true),
        };


    public async Task<Results<TResponse, ErrorResultBase>> RequestAsync<TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract,
        CancellationToken cancellationToken = default)
        where TError : ErrorResultBase
        where TActualRequest : ActualRequestBase
        where TRequest : RequestBase
        where TResponse : ResponseBase, new()
    {
        try
        {
            return await Handler.RequestAsync(contract, cancellationToken);
        }
        catch (Exception ex)
        {
            return Results<TResponse, ErrorResultBase>.CreateError(new ExceptionedErrorBase(-500, ex.Message, ex));
        }
    }

    public async Task<Results<TResponse, ErrorResultBase>> RequestAsync<TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, TRequest request,
        CancellationToken cancellationToken = default)
        where TError : ErrorResultBase
        where TActualRequest : ActualRequestBase
        where TRequest : RequestBase
        where TResponse : ResponseBase, new()
    {
        try
        {
            return await Handler.RequestAsync(contract, request, cancellationToken);
        }
        catch (Exception ex)
        {
            return Results<TResponse, ErrorResultBase>.CreateError(new ExceptionedErrorBase(-500, ex.Message, ex));
        }
    }
    
    public async Task<Results<TCustomResponse, ErrorResultBase>> RequestAsync<
        TCustomResponse, TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, TRequest? request,
        CancellationToken cancellationToken = default)
        where TError : ErrorResultBase
        where TActualRequest : ActualRequestBase
        where TRequest : RequestBase
        where TResponse : ResponseBase, new()
        where TCustomResponse : ResponseBase, new()
    {
        try
        {
            return await Handler.RequestAsync<TCustomResponse, TRequest, TResponse, TError, TActualRequest>(
                contract, request, cancellationToken);
        }
        catch (Exception ex)
        {
            return Results<TCustomResponse, ErrorResultBase>.CreateError(
                new ExceptionedErrorBase(-500, ex.Message, ex));
        }
    }

    public async Task<Results<TCustomResponse, ErrorResultBase>> RequestAsync<
        TCustomRequest, TCustomResponse, TRequest, TResponse, TError, TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, bool differ, TCustomRequest? request,
        CancellationToken cancellationToken = default)
        where TError : ErrorResultBase
        where TActualRequest : ActualRequestBase
        where TRequest : RequestBase
        where TResponse : ResponseBase, new()
        where TCustomResponse : ResponseBase, new()
    {
        try
        {
            return await Handler
                .RequestAsync<TCustomRequest, TCustomResponse, TRequest, TResponse, TError, TActualRequest>(
                    contract, request, cancellationToken);
        }
        catch (Exception ex)
        {
            return Results<TCustomResponse, ErrorResultBase>.CreateError(
                new ExceptionedErrorBase(-500, ex.Message, ex));
        }
    }

    public async Task<Results<TResponse, ErrorResultBase>> RequestAsync<TCustomRequest, TRequest, TResponse, TError,
                                                                        TActualRequest>(
        ApiContractBase<TRequest, TResponse, TError, TActualRequest> contract, bool differ, TCustomRequest? request,
        ApiHandlerOption option, CancellationToken cancellationToken = default)
        where TError : ErrorResultBase
        where TActualRequest : ActualRequestBase
        where TRequest : RequestBase
        where TResponse : ResponseBase, new()
    {
        try
        {
            return await Handler.RequestAsync(
                contract, true, request, option, cancellationToken);
        }
        catch (Exception ex)
        {
            return Results<TResponse, ErrorResultBase>.CreateError(
                new ExceptionedErrorBase(-500, ex.Message, ex));
        }
    }

    public async Task<bool> LoginEmailAsync(string email, string password, bool isMd5 = false,
                                            CancellationToken ctk = default)
    {
        var request = new LoginEmailRequest()
        {
            Email = email,
        };
        if (isMd5)
            request.Md5Password = password;
        else
            request.Password = password;
        var result = await RequestAsync(NeteaseApis.LoginEmailApi, request, ctk);
        return result.Match(
            success =>
            {
                LoginedUser = success.Profile?.MapToNeteaseUser();
                return true;
            },
            _ => false
        );
    }

    public async Task<bool> LoginCellphoneAsync(string cellphone, string password, bool isMd5 = false,
                                                CancellationToken ctk = default)
    {
        var request = new LoginCellphoneRequest()
        {
            Cellphone = cellphone
        };
        if (isMd5)
            request.Md5Password = password;
        else
            request.Password = password;
        var result = await RequestAsync(NeteaseApis.LoginCellphoneApi, request, ctk);
        return result.Match(
            success =>
            {
                LoginedUser = success.Profile?.MapToNeteaseUser();
                return true;
            },
            _ => false
        );
    }


    public async Task<List<RawLyricInfo>> GetLyricInfoAsync(SingleSongBase song, CancellationToken ctk = default)
    {
        if (song.ActualId == null) throw new ArgumentNullException();
        var results = await RequestAsync(NeteaseApis.LyricApi, new LyricRequest() { Id = song.ActualId }, ctk);
        return results.Match(
            success => success.Map(),
            _ => new()
        ).Select(t => (RawLyricInfo)t).ToList();
    }

    public async Task<MusicResourceBase?> GetMusicResourceAsync(SingleSongBase song, ResourceQualityTag? qualityTag = null,
                                                                CancellationToken ctk = default)
    {
        var quality = "exhigh";
        if (qualityTag is NeteaseMusicQualityTag neteaseMusicQualityTag)
            quality = neteaseMusicQualityTag.Quality;
        var results = await RequestAsync(NeteaseApis.SongUrlApi,
                                         new SongUrlRequest
                                         {
                                             Id = song.ActualId,
                                             Level = quality
                                         }, ctk);

        NeteaseMusicResource? MatchSuccess(SongUrlResponse response)
        {
            if (response.Code is not 200) return null;
            if (response.SongUrls is not { Length: > 0 }) return null;
            return new NeteaseMusicResource
            {
                Md5 = response.SongUrls[0].Md5,
                Size = response.SongUrls[0].Size,
                BitRate = response.SongUrls[0].BitRate,
                EncodeType = response.SongUrls[0].EncodeType,
                Time = response.SongUrls[0].Time,
                MusicType = response.SongUrls[0].Type,
                Level = response.SongUrls[0].Level,
                Uri = new Uri(response.SongUrls[0].Url ?? string.Empty)
            };
        }


        return results.Match(
            MatchSuccess,
            _ => null
        );
    }

    public async Task<ProvidableItemBase?> GetProvidableItemByIdAsync(string inProviderId,
                                                                      CancellationToken ctk = default)
    {
        var typeId = inProviderId.Substring(0, 2);
        var actualId = inProviderId.Substring(2);
        switch (typeId)
        {
            case NeteaseTypeIds.SingleSong:
                return await GetSingleSongById(actualId, ctk);
            case NeteaseTypeIds.Playlist:
                return await GetPlaylistById(actualId, ctk);
            case NeteaseTypeIds.Album:
                return await GetAlbumById(actualId, ctk);
            case NeteaseTypeIds.Artist:
                return await GetArtistById(actualId, ctk);
            case NeteaseTypeIds.User:
                return new NeteaseUser
                {
                    ActualId = actualId,
                    Name = actualId
                };
            case NeteaseTypeIds.RadioChannel:
                return await GetRadioChannelById(actualId, ctk);
            case NeteaseTypeIds.PersonalFm:
                return new NeteasePersonalFMContainer
                {
                    ActualId = actualId,
                    Name = "私人 FM"
                };
        }

        return null;
    }

    public async Task<List<ProvidableItemBase>> GetProvidableItemsRangeAsync(
        List<string> inProviderIds, CancellationToken ctk = default)
    {
        if (inProviderIds.Count == 0) return new List<ProvidableItemBase>();
        var grouped = inProviderIds.GroupBy(t => t.Substring(0, 2)).ToList();
        if (grouped.Count > 1)
        {
            var result = new List<ProvidableItemBase>();
            foreach (var group in grouped)
                result.AddRange(await GetProvidableItemsRangeAsync(group.ToList(), ctk));
            return result;
        }

        switch (grouped[0].Key)
        {
            case NeteaseTypeIds.SingleSong:
                return (await GetSingleSongRangeByIds(inProviderIds.Select(t => t.Substring(2)).ToList(), ctk))
                       .Select(t => (ProvidableItemBase)t).ToList();
            case NeteaseTypeIds.Playlist:
                return (await GetPlaylistRangeByIds(inProviderIds.Select(t => t.Substring(2)).ToList(), ctk))
                       .Select(t => (ProvidableItemBase)t).ToList();
            case NeteaseTypeIds.Album:
            case NeteaseTypeIds.Artist:
            case NeteaseTypeIds.RadioChannel:
            case NeteaseTypeIds.User:
                var result = new List<ProvidableItemBase>();
                foreach (var id in inProviderIds)
                {
                    var item = await GetProvidableItemByIdAsync(id, ctk);
                    if (item is not null) result.Add(item);
                }
                return result;
        }

        return new List<ProvidableItemBase>();
    }

    #region ProvidableItemGet

    public async Task<NeteaseSong?> GetSingleSongById(string id, CancellationToken cancellationToken = default)
    {
        var songResult = await RequestAsync(NeteaseApis.SongDetailApi,
                                            new SongDetailRequest()
                                            {
                                                Id = id
                                            }, cancellationToken);
        return songResult.Match(
            success => success.Songs?[0].MapToNeteaseMusic(),
            _ => null
        );
    }

    public async Task<NeteasePlaylist?> GetPlaylistById(string id, CancellationToken cancellationToken = default)
    {
        var songResult = await RequestAsync(NeteaseApis.PlaylistDetailApi,
                                            new PlaylistDetailRequest()
                                            {
                                                Id = id
                                            }, cancellationToken);
        return songResult.Match(
            success => success.Playlists?[0].MapToNeteasePlaylist(),
            _ => null
        );
    }

    public async Task<NeteaseAlbum?> GetAlbumById(string id, CancellationToken cancellationToken = default)
    {
        var albumResult = await RequestAsync(NeteaseApis.AlbumApi,
                                             new AlbumRequest
                                             {
                                                 Id = id
                                             }, cancellationToken);
        return albumResult.Match(
            success => success.Album.MapToNeteaseAlbum(),
            _ => null
        );
    }

    public async Task<NeteaseArtist?> GetArtistById(string id, CancellationToken cancellationToken = default)
    {
        var artistResult = await RequestAsync(NeteaseApis.ArtistDetailApi,
                                              new ArtistDetailRequest
                                              {
                                                  ArtistId = id
                                              }, cancellationToken);
        return artistResult.Match(
            success => success.Artist is null
                ? null
                : new NeteaseArtist
                {
                    ActualId = success.Artist.Id,
                    Name = success.Artist.Name ?? "未知歌手"
                },
            _ => null
        );
    }

    public async Task<NeteaseRadioChannel?> GetRadioChannelById(string id, CancellationToken cancellationToken = default)
    {
        var radioResult = await RequestAsync(NeteaseApis.DjChannelDetailApi,
                                             new DjChannelDetailRequest
                                             {
                                                 Id = id
                                             }, cancellationToken);
        return radioResult.Match(
            success => success.RadioData?.MapToNeteaseRadioChannel(),
            _ => null
        );
    }

    public async Task<List<NeteaseSong>> GetSingleSongRangeByIds(List<string> idList,
                                                                 CancellationToken cancellationToken = default)
    {
        var songResult = await RequestAsync(NeteaseApis.SongDetailApi,
                                            new SongDetailRequest()
                                            {
                                                IdList = idList
                                            }, cancellationToken);
        return songResult.Match(
            success => success.Songs?.Select(t => t.MapToNeteaseMusic()).ToList() ??
                       new List<NeteaseSong>(),
            _ => new List<NeteaseSong>()
        );
    }

    public async Task<List<NeteasePlaylist>> GetPlaylistRangeByIds(List<string> idList,
                                                                   CancellationToken cancellationToken = default)
    {
        var songResult = await RequestAsync(NeteaseApis.PlaylistDetailApi,
                                            new PlaylistDetailRequest()
                                            {
                                                IdList = idList
                                            }, cancellationToken);
        return songResult.Match(
            success => success.Playlists?.Select(t => t.MapToNeteasePlaylist()).ToList() ??
                       new List<NeteasePlaylist>(),
            _ => new List<NeteasePlaylist>()
        );
    }

    #endregion

    public Task<ContainerBase> SearchProvidableItemsAsync(string keyword, string typeId,
                                                                 CancellationToken ctk = default)
    {
        return Task.FromResult(new NeteaseSearchContainer
        {
            Name = "搜索结果",
            ActualId = keyword,
            SearchTypeId = TypeIdToSearchIdMapper.MapToResourceId(typeId),
            SearchKeyword = keyword,
        } as ContainerBase);
    }

    public Task<ContainerBase?> GetStoredItemsAsync(string typeId, CancellationToken ctk = default)
    {
        switch (typeId)
        {
            case NeteaseTypeIds.Playlist:
                return Task.FromResult(LoginedUser as ContainerBase);
            default:
                throw new NotImplementedException();
        }
    }

}

#region

using FluentAssertions;
using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseProvider.Constants;
using HyPlayer.PlayCore.Abstraction.Models.Containers;

#endregion

namespace HyPlayer.NeteaseProvider.Tests;

[Retry(3)]
public class NeteaseApisTests
{
    private readonly NeteaseProvider _provider = new();

    //[Test]
    public async Task LoginEmail_Should_LoginWithInfo(string email, string password)
    {
        var result = await _provider.RequestAsync(NeteaseApis.LoginEmailApi,
            new LoginEmailRequest
            {
                Email = email,
                Password = password
            });
        result.Match(
            (resp) =>
            {
                resp.Should().NotBeNull();
                resp.Profile?.Gender.Should().BeOneOf(0, 1);
                return true;
            },
            (err) => throw err
        );
    }

    // [Test]
    public async Task LoginCellphone_Should_LoginWithInfo(string phone, string password)
    {
        var result =
            await _provider.RequestAsync(NeteaseApis.LoginCellphoneApi,
                new LoginCellphoneRequest()
                {
                    Cellphone = phone,
                    Password = password
                });
        result.Match(
            (resp) =>
            {
                resp.Should().NotBeNull();
                resp.Profile?.Gender.Should().BeOneOf(0, 1);
                return true;
            },
            (err) => throw err
        );
    }

    [Test]
    [Arguments("2034742057")]
    public async Task SongDetail_Single_Should_HasInfo(string id)
    {
        var result =
            await _provider.RequestAsync(
                NeteaseApis.SongDetailApi,
                new SongDetailRequest()
                {
                    Id = id
                });
        result.Match(
            success =>
            {
                success.Songs.Should().HaveCount(1);
                success.Songs?[0].Id.Should().Be(id);
                return true;
            },
            error => throw error
        );
    }

    [Test]
    [Arguments("2034742057", "1811209786", "1953828605")]
    public async Task SongDetail_Multiple_Should_HasInfo(params string[] ids)
    {
        var result =
            await _provider.RequestAsync(
                NeteaseApis.SongDetailApi,
                new SongDetailRequest()
                {
                    IdList = ids.ToList()
                });
        result.Match(
            success =>
            {
                success.Songs.Should().HaveCount(3);
                return true;
            },
            error => throw error
        );
    }

    [Test]
    [Arguments("2034742057")]
    public async Task SongUrl_Single_Should_ReturnUrl(string id)
    {
        var result =
            await _provider.RequestAsync(
                NeteaseApis.SongUrlApi,
                new SongUrlRequest()
                {
                    Id = id,
                    Level = "jymaster"
                });
        result.Match(
            success =>
            {
                success.SongUrls.Should().NotBeNull();
                success.SongUrls.Should().HaveCount(1);
                success.SongUrls![0].Url.Should().NotBeEmpty();
                return true;
            },
            error => throw error
        );
    }

    [Test]
    [Arguments("2034742057", "1811209786", "1953828605")]
    public async Task SongUrl_Multiple_Should_ReturnUrl(params string[] ids)
    {
        var result =
            await _provider.RequestAsync(
                NeteaseApis.SongUrlApi,
                new SongUrlRequest()
                {
                    IdList = ids.ToList(),
                    Level = "jymaster"
                });
        result.Match(
            _ => true,
            error => throw error
        );
    }

    [Test]
    [Arguments("2778408564")]
    [Arguments("897784673")]
    public async Task PlaylistDetail_Should_ReturnNormal(string playlistId)
    {
        var result = await _provider.RequestAsync(
            NeteaseApis.PlaylistDetailApi,
            new PlaylistDetailRequest()
            {
                Id = playlistId
            });
        result.Match(
            _ => true,
            error => throw error
        );
    }

    [Test]
    [Arguments("2778408564")]
    public async Task PlaylistTracksGet_Should_ReturnNormal(string playlistId)
    {
        var result = await _provider.RequestAsync(
            NeteaseApis.PlaylistTracksGetApi,
            new PlaylistTracksGetRequest()
            {
                Id = playlistId
            });
        result.Match(
            success => success.Playlist?.TrackIds.Should().NotBeEmpty(),
            error => throw error
        );
    }

    [Test]
    [Arguments("1880520974")]
    public async Task Lyric_Should_ReturnNormal(string songId)
    {
        var result = await _provider.RequestAsync(
            NeteaseApis.LyricApi,
            new LyricRequest()
            {
                Id = songId
            }
        );
        result.Match(
            success => success.Lyric?.Lyric.Should().NotBeEmpty(),
            error => throw error
        );
    }

    [Test]
    public async Task Toplist_Should_ReturnNormal()
    {
        var result = await _provider.RequestAsync(NeteaseApis.ToplistApi, new ToplistRequest());
        result.Match(
            success => success.List.Should().NotBeEmpty(),
            error => throw error
        );
    }

    [Test]
    public async Task RecommendedPlaylist_Should_ReturnNormal()
    {
        var result = await _provider.RequestAsync(NeteaseApis.RecommendSongsApi, new RecommendSongsRequest());
        result.Match(
            success => success.Data?.DailySongs.Should().NotBeEmpty(),
            error => throw error
        );

        (await (await _provider.GetRecommendationAsync(NeteaseTypeIds.Playlist))
            .Should().BeAssignableTo<LinerContainerBase>().Subject
            .GetAllItemsAsync()).Should().NotBeEmpty();
    }

    [Test]
    [Arguments("ヰ世界情緒", NeteaseTypeIds.SingleSong)]
    [Arguments("ヰ世界情緒", NeteaseTypeIds.Album)]
    [Arguments("ヰ世界情緒", NeteaseTypeIds.Artist)]
    [Arguments("ヰ世界情緒", NeteaseTypeIds.Playlist)]
    [Arguments("网易云音乐", NeteaseTypeIds.User)]
    [Arguments("ヰ世界情緒", NeteaseTypeIds.Mv)]
    [Arguments("ヰ世界情緒", NeteaseTypeIds.Lyric)]
    [Arguments("ヰ世界情緒", NeteaseTypeIds.RadioChannel)]
    [Arguments("ヰ世界情緒", NeteaseTypeIds.MBlog)]
    public async Task Search_Song_Should_ReturnNormal(string keyword, string type)
    {
        var result = await _provider.SearchProvidableItemsAsync(keyword, type);
        (await result.Should().BeAssignableTo<LinerContainerBase>().Subject.GetAllItemsAsync()).Should().NotBeEmpty();
    }

    [Test]
    public async Task PlaylistCategoryListApi_Should_BeNormal()
    {
        var result = await _provider.RequestAsync(NeteaseApis.PlaylistCategoryListApi, new PlaylistCategoryListRequest
        {
            Category = "ACG",
            Limit = 15
        });
        result.Match(s => s.Playlists.Should().NotBeEmpty(),
            e => throw e);
    }

    [Test]
    public async Task AiDjRcmdInfo_Should_ReturnNormal()
    {
        var result = await _provider.RequestAsync(NeteaseApis.AiDjContentRcmdInfoApi, new AiDjContentRcmdInfoRequest());
        result.Match(success =>
                success.Code.Should().Be(200),
            e => throw e);
    }

    [Test]
    [Arguments("1972641406")]
    public async Task ListenFirstInfo_Should_BeNormal(string songId)
    {
        var result = await _provider.RequestAsync(NeteaseApis.MusicFirstListenInfoApi, new MusicFirstListenInfoRequest()
        {
            SongId = songId
        });
        result.Match(s => s.Code.Should().Be(200),
            e => throw e);
    }

    [Test]
    [Arguments("DEFAULT", null)]
    [Arguments("SCENE_RCMD", "NIGHT_EMO")]
    public async Task PersonalFm_Should_BeNormal(string mode, string? subMode = null)
    {
        var result = await _provider.RequestAsync(NeteaseApis.PersonalFmApi, new PersonalFmRequest
        {
            Mode = mode,
            SubMode = subMode,
            Limit = 5
        });
        result.Match(s => s.Code.Should().Be(200),
            e => throw e);
    }

    // [Test]
    public async Task LoginQrCodeUnikey_Should_BeNormal()
    {
        var result = await _provider.RequestAsync(NeteaseApis.LoginQrCodeUnikeyApi, new LoginQrCodeUnikeyRequest());
        result.Match(s => s.Code.Should().Be(200),
            e => throw e);
        TestContext.Current!.ObjectBag["unikey"] = result.Value?.Unikey;
    }

    // [Test]
    // [DependsOn(nameof(LoginQrCodeUnikey_Should_BeNormal))]
    public async Task LoginQrCodeCheck_Should_BeNormal()
    {
        var unikey = TestContext.Current!.ObjectBag["unikey"] as string;
        var result = await _provider.RequestAsync(NeteaseApis.LoginQrCodeCheckApi, new LoginQrCodeCheckRequest
        {
            Unikey = unikey!
        });
        result.Match(s => s.Code.Should().Be(200),
            e => throw e);
    }

    [Test]
    public async Task AlbumSublist_Should_BeNormal()
    {
        var result = await _provider.RequestAsync(NeteaseApis.AlbumSublistApi, new AlbumSublistRequest
        {
            Limit = 5
        });
        result.Match(s =>
            {
                s.Code.Should().Be(200);
                s.Data.Should().NotBeEmpty();
                s.Data.Should().AllSatisfy(t => t.Id.Should().NotBeEmpty());
                return true;
            },
            e => throw e);
    }

    [Test]
    [Arguments("162549105")]
    public async Task AlbumDetailDynamic_Should_BeNormal(string id)
    {
        var result = await _provider.RequestAsync(NeteaseApis.AlbumDetailDynamicApi, new AlbumDetailDynamicRequest
        {
            Id = id
        });
        result.Match(s =>
            {
                s.Code.Should().Be(200);
                s.CommentCount.Should().BeGreaterThan(0);
                return true;
            },
            e => throw e);
    }

    [Test]
    [Arguments("8645419738")]
    public async Task UserRecordApi_Should_BeNormal(string id)
    {
        var result =
            await _provider
                .RequestAsync<UserRecordAllResponse, UserRecordRequest, UserRecordResponse, ErrorResultBase,
                    UserRecordActualRequest>(NeteaseApis.UserRecordApi, new UserRecordRequest
                {
                    UserId = id,
                    RecordType = UserRecordType.All
                });
        result.Match(s => s.Code.Should().Be(200),
            e => throw e);
    }

    //SearchSuggestionApi
    [Test]
    [Arguments("ヰ世界情緒")]
    public async Task SearchSuggestion_Should_BeNormal(string keyword)
    {
        var result = await _provider.RequestAsync(NeteaseApis.SearchSuggestionApi, new SearchSuggestionRequest
        {
            Keyword = keyword
        });
        result.Match(s => s.Code.Should().Be(200),
            e => throw e);
    }

    // ArtistDetailApi
    [Test]
    [Arguments("51020133")]
    public async Task ArtistDetail_Should_BeNormal(string id)
    {
        var result = await _provider.RequestAsync(NeteaseApis.ArtistDetailApi, new ArtistDetailRequest
        {
            ArtistId = id
        });
        result.Match(s => s.Code.Should().Be(200),
            e => throw e);
    }


    [Test]
    [Arguments("793914432")]
    public async Task DjChannelPrograms_Should_BeNormal(string id)
    {
        var result = await _provider.RequestAsync(NeteaseApis.DjChannelProgramsApi, new DjChannelProgramsRequest
        {
            RadioId = id
        });
        result.Match(s => s.Code.Should().Be(200),
            e => throw e);
    }

    // LoginStatusApi
    [Test]
    public async Task LoginStatus_Should_BeNormal()
    {
        var result = await _provider.RequestAsync(NeteaseApis.LoginStatusApi, new LoginStatusRequest());
        result.Match(s => s.Code.Should().Be(200),
            e => throw e);
    }

    // ArtistTopSong
    [Test]
    [Arguments("51020133")]
    public async Task ArtistTopSong_Should_BeNormal(string id)
    {
        var result = await _provider.RequestAsync(NeteaseApis.ArtistTopSongApi, new ArtistTopSongRequest
        {
            ArtistId = id
        });
        result.Match(s => s.Code.Should().Be(200),
            e => throw e);
    }

    // SongWikiSummaryApi
    [Test]
    [Arguments("2033878955")]
    public async Task SongWikiSummary_Should_BeNormal(string id)
    {
        var result = await _provider.RequestAsync(NeteaseApis.SongWikiSummaryApi, new SongWikiSummaryRequest
        {
            SongId = id
        });
        result.Match(s => s.Code.Should().Be(200),
            e => throw e);
    }

    // RecommendResourceApi
    [Test]
    public async Task RecommendResource_Should_BeNormal()
    {
        var result = await _provider.RequestAsync(NeteaseApis.RecommendResourceApi, new RecommendResourceRequest());
        result.Match(s => s.Code.Should().Be(200),
            e => throw e);
    }

    // ArtistVideoApi
    [Test]
    [Arguments("51020133")]
    public async Task ArtistVideo_Should_BeNormal(string id)
    {
        var result = await _provider.RequestAsync(NeteaseApis.ArtistVideoApi, new ArtistVideoRequest
        {
            ArtistId = id
        });
        result.Match(s => s.Code.Should().Be(200),
            e => throw e);
    }


    [Before(Test)]
    public async Task InitializeAsync()
    {
        _provider.Handler.Option.AdditionalParameters = Secrets.AdditionalParameters;
    }
}
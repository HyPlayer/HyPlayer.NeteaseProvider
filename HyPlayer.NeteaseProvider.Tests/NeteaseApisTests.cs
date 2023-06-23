using FluentAssertions;
using HyPlayer.NeteaseProvider.Requests;

namespace HyPlayer.NeteaseProvider.Tests;

public class NeteaseApisTests
{
    private readonly NeteaseProvider _provider;

    public NeteaseApisTests()
    {
        _provider = new NeteaseProvider();
    }

    //[Theory]
    public async void LoginEmail_Should_LoginWithInfo(string email, string password)
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
                resp.Profile.Gender.Should().BeOneOf(0, 1);
                return true;
            },
            (err) => throw err
        );
    }

    //[Theory]
    public async void LoginCellphone_Should_LoginWithInfo(string phone, string password)
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
                resp.Profile.Gender.Should().BeOneOf(0, 1);
                return true;
            },
            (err) => throw err
        );
    }

    [Theory]
    [InlineData("2034742057")]
    public async void SongDetail_Single_Should_HasInfo(string id)
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
                success.Songs[0].Id.Should().Be(id);
                return true;
            },
            error => throw error
        );
    }
    
    [Theory]
    [InlineData("2034742057","1811209786","1953828605")]
    public async void SongDetail_Multiple_Should_HasInfo(params string[] ids)
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
}
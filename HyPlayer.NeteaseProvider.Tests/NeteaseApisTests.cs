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

    [Theory]
    [InlineData("hyplayer123@163.com", "hyplayer123123")]
    public async void LoginEmail(string email, string password)
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
                resp.Profile.Gender.Should().Be(1);
                return true;
            },
            (err) => throw err
        );
    }
}
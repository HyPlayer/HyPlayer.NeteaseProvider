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
                resp.Profile.Gender.Should().BeOneOf(0, 1);
                return true;
            },
            (err) => throw err
        );
    }

    //[Theory]
    public async void LoginCellphone(string phone, string password)
    {
        var result = await _provider.RequestAsync(NeteaseApis.LoginCellphoneApi,
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
}
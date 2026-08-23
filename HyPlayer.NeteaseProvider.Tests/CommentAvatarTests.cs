using AwesomeAssertions;
using HyPlayer.NeteaseProvider.Models;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Resources;
using TUnit.Core;

namespace HyPlayer.NeteaseProvider.Tests;

public sealed class CommentAvatarTests
{
    [Test]
    public async Task Missing_fourth_comment_avatar_returns_a_failed_resource_instead_of_throwing()
    {
        NeteaseUser[] commentUsers =
        [
            CreateUser("https://example.com/1.jpg"),
            CreateUser("https://example.com/2.jpg"),
            CreateUser("https://example.com/3.jpg"),
            CreateUser(null)
        ];

        var results = await Task.WhenAll(commentUsers.Select(user => user.GetCoverAsync()));

        results.Should().HaveCount(4);
        results[3].ResourceStatus.Should().Be(ResourceStatus.Fail);
        var avatarResource = results[3].Should().BeAssignableTo<IResourceResultOf<Uri?>>().Subject;
        (await avatarResource.GetResourceAsync()).Should().BeNull();
    }

    private static NeteaseUser CreateUser(string? avatarUrl)
    {
        return new NeteaseUser
        {
            ActualId = Guid.NewGuid().ToString(),
            Name = "评论用户",
            AvatarUrl = avatarUrl
        };
    }
}

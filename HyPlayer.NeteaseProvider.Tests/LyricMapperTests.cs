using AwesomeAssertions;
using HyPlayer.NeteaseApi.ApiContracts.Song;
using HyPlayer.NeteaseProvider.Mappers;
using HyPlayer.PlayCore.Abstraction.Models.Lyric;

namespace HyPlayer.NeteaseProvider.Tests;

public class LyricMapperTests
{
    [Test]
    public void Map_Should_KeepNonEmptyLyrics()
    {
        var response = new LyricResponse
        {
            Lyric = new LyricResponse.LyricInfo { Lyric = "[00:01.000]line" },
            TranslationLyric = new LyricResponse.LyricInfo { Lyric = "[00:01.000]translation" },
            RomajiLyric = new LyricResponse.LyricInfo { Lyric = "[00:01.000]romaji" },
            YunLyric = new LyricResponse.LyricInfo { Lyric = "[1000,1000](1000,1000,0)word" },
            YunTranslationLyric = new LyricResponse.LyricInfo { Lyric = "[00:01.000]word translation" },
            YunRomajiLyric = new LyricResponse.LyricInfo { Lyric = "[00:01.000]word romaji" }
        };

        var result = response.Map();

        result.Should().HaveCount(6);
        result.Should().Contain(t => !t.IsWord && t.LyricType == LyricType.Original && t.LyricText.Contains("line"));
        result.Should().Contain(t => !t.IsWord && t.LyricType == LyricType.Translation && t.LyricText.Contains("translation"));
        result.Should().Contain(t => !t.IsWord && t.LyricType == LyricType.Romaji && t.LyricText.Contains("romaji"));
        result.Should().Contain(t => t.IsWord && t.LyricType == LyricType.Original && t.LyricText.Contains("word"));
        result.Should().Contain(t => t.IsWord && t.LyricType == LyricType.Translation && t.LyricText.Contains("word translation"));
        result.Should().Contain(t => t.IsWord && t.LyricType == LyricType.Romaji && t.LyricText.Contains("word romaji"));
        result.HasTranslation.Should().BeTrue();
        result.HasWordLyric.Should().BeTrue();
    }

    [Test]
    public void Map_Should_IgnoreEmptyLyrics()
    {
        var response = new LyricResponse
        {
            Lyric = new LyricResponse.LyricInfo { Lyric = string.Empty },
            TranslationLyric = new LyricResponse.LyricInfo { Lyric = string.Empty },
            RomajiLyric = new LyricResponse.LyricInfo { Lyric = string.Empty },
            YunLyric = new LyricResponse.LyricInfo { Lyric = string.Empty },
            YunTranslationLyric = new LyricResponse.LyricInfo { Lyric = string.Empty },
            YunRomajiLyric = new LyricResponse.LyricInfo { Lyric = string.Empty }
        };

        var result = response.Map();

        result.Should().BeEmpty();
        result.HasTranslation.Should().BeFalse();
        result.HasWordLyric.Should().BeFalse();
    }
}

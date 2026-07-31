using AwesomeAssertions;
using HyPlayer.NeteaseProvider.Models;
using TUnit.Core;

namespace HyPlayer.NeteaseProvider.Tests;

public sealed class AudioGainMetadataTests
{
    [Test]
    [Arguments("standard", true)]
    [Arguments("higher", true)]
    [Arguments("exhigh", true)]
    [Arguments("lossless", true)]
    [Arguments("hires", true)]
    [Arguments("jymaster", true)]
    [Arguments("jyeffect", false)]
    [Arguments("sky", false)]
    [Arguments("future-spatial-format", false)]
    public void Only_known_stereo_levels_support_audio_gain(string level, bool expected)
    {
        var resource = new NeteaseMusicResource { Level = level };

        resource.SupportsAudioGain.Should().Be(expected);
    }
}

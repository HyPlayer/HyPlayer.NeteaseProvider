using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.ApiContracts.Song;
using HyPlayer.NeteaseProvider.Models;
using HyPlayer.PlayCore.Abstraction.Models.Lyric;

namespace HyPlayer.NeteaseProvider.Mappers;

public static class ResponseToRawLyricInfosMapper
{
    public static NeteaseSongLyricInfos Map(this LyricResponse response)
    {
        var ret = new NeteaseSongLyricInfos();
        if (!string.IsNullOrEmpty(response.Lyric?.Lyric))
        {
            ret.Add(new NeteaseRawLyricInfo
            {
                LyricText = response.Lyric?.Lyric!,
                Source = "netease:lrc",
                LyricTypeActual = LyricType.Original,
                Author = MapAuthor(response.LyricUser)
            });
        }

        if (!string.IsNullOrEmpty(response.TranslationLyric?.Lyric))
        {
            ret.HasTranslation = true;
            ret.Add(new NeteaseRawLyricInfo
            {
                LyricText = response.TranslationLyric?.Lyric!,
                Source = "netease:lrc",
                LyricTypeActual = LyricType.Translation,
                Author = MapAuthor(response.TranslationUser)
            }
            );
        }

        if (!string.IsNullOrEmpty(response.RomajiLyric?.Lyric))
        {
            ret.Add(new NeteaseRawLyricInfo
            {
                LyricText = response.RomajiLyric?.Lyric!,
                Source = "netease:lrc",
                LyricTypeActual = LyricType.Romaji,
                Author = new()
                {
                    ActualId = "1",
                    Name = "网易云音乐 自动转换"
                }
            }
            );
        }

        if (!string.IsNullOrEmpty(response.YunLyric?.Lyric))
        {
            ret.HasWordLyric = true;
            ret.Add(new NeteaseRawLyricInfo
            {
                LyricText = response.YunLyric?.Lyric!,
                Source = "netease:yrc",
                IsWord = true,
                LyricTypeActual = LyricType.Original,
                Author = MapAuthor(response.LyricUser)
            });
        }

        if (!string.IsNullOrEmpty(response.YunTranslationLyric?.Lyric))
        {
            ret.HasTranslation = true;
            ret.Add(new NeteaseRawLyricInfo
            {
                LyricText = response.YunTranslationLyric?.Lyric!,
                Source = "netease:yrc",
                IsWord = true,
                LyricTypeActual = LyricType.Translation,
                Author = MapAuthor(response.TranslationUser)
            });
        }


        if (!string.IsNullOrEmpty(response.YunRomajiLyric?.Lyric))
        {
            ret.Add(new NeteaseRawLyricInfo
            {
                LyricText = response.YunRomajiLyric?.Lyric!,
                Source = "netease:yrc",
                IsWord = true,
                LyricTypeActual = LyricType.Romaji,
                Author = new()
                {
                    ActualId = "1",
                    Name = "网易云音乐 自动转换"
                }
            }
            );
        }

        return ret;
    }

    private static NeteaseRawLyricInfo.LyricAuthorInfo? MapAuthor(LyricResponse.LyricUserInfo? user)
    {
        if (string.IsNullOrWhiteSpace(user?.UserId) && string.IsNullOrWhiteSpace(user?.Nickname))
            return null;

        return new NeteaseRawLyricInfo.LyricAuthorInfo
        {
            ActualId = user?.UserId,
            Name = user?.Nickname ?? "未知贡献者"
        };
    }
}

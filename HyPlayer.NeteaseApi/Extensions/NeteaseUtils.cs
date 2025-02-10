using HyPlayer.NeteaseApi.Models;
using System.Security.Cryptography;
using System.Text;

namespace HyPlayer.NeteaseApi.Extensions;

public class NeteaseUtils
{
    public static string CommentTypeToThreadPrefix(NeteaseResourceType type)
    {
        switch (type)
        {
            case NeteaseResourceType.Song: return "R_SO_4_";
            case NeteaseResourceType.MV: return "R_MV_5_";
            case NeteaseResourceType.Playlist: return "A_PL_0_";
            case NeteaseResourceType.Album: return "R_AL_3_";
            case NeteaseResourceType.RadioChannel: return "A_DR_14_";
            case NeteaseResourceType.RadioProgram: return "A_DJ_1_";
            case NeteaseResourceType.Video: return "R_VI_62_";
            case NeteaseResourceType.Dynamic: return "A_EV_2_";
            case NeteaseResourceType.MLog: return "R_MLOG_1001_";
            default: throw new ArgumentOutOfRangeException(nameof(type));
        }
    }

    public static string GetCacheKey(string input)
    {
        // aes encrypt: ecb key:")(13daqP@ssw0rd~", output base64
        var key = ")(13daqP@ssw0rd~";
        var aes = Aes.Create();
        aes.BlockSize = 128;
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.Mode = CipherMode.ECB;
        var encryptor = aes.CreateEncryptor();
        var buffer = Encoding.UTF8.GetBytes(input);
        var result = encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
        return Convert.ToBase64String(result);
    }
}
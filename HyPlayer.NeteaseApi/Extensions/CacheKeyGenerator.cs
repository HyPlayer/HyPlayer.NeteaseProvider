using System.Security.Cryptography;
using System.Text;

namespace HyPlayer.NeteaseApi.Extensions;

public class CacheKeyGenerator
{
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
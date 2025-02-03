using System.Security.Cryptography;
using System.Text;

namespace HyPlayer.NeteaseApi.Extensions;

public class ClientSignGenerator
{
    public string GetNMCID()
    {
        var chars = "abcdefghijklmnopqrstuvwxyz";
        var result = new string(Enumerable.Range(0, 5).Select(_ => chars[Random.Shared.Next(chars.Length)]).ToArray());
        return $"{result}.{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}.01.4";
    }
    
    
}

public class ClientInfo
{

}
#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace HyPlayer.NeteaseProvider.LocalMusic;

public static class NCMFile
{
    private static readonly byte[] _flag = "CTENFDAM"u8.ToArray();

    private static readonly byte[] _coreBoxKey = "hzHRAmso5kInbaxW"u8.ToArray();

    private static readonly byte[] _modifyBoxKey = "#14ljk_!\\]&0U<'("u8.ToArray();

    private static byte[]? _keyBox;

    public static byte[] GetCoverByteArray(Stream stream)
    {
        stream.Seek(9, SeekOrigin.Current);
        return ReadChunk(stream) ?? [];
    }

    public static Stream GetEncryptedStream(Stream sourceStream)
    {
        var n = 0x8000;
        var stream = new MemoryStream();
        var keyBox = GetKeyBox();

        while (n > 1)
        {
            var chunk = new byte[n];
            n = sourceStream.Read(chunk, 0, n);

            for (var i = 0; i < n; i++)
            {
                var j = (i + 1) & 0xff;
                chunk[i] ^= keyBox[(keyBox[j] + keyBox[(keyBox[j] + j) & 0xff]) & 0xff];
            }

            stream.Write(chunk, 0, n);
        }

        return stream;
    }

    public static async Task CopyDecryptedContentToAsync(
        Stream sourceStream,
        Stream destinationStream,
        CancellationToken cancellationToken = default)
    {
        var buffer = new byte[0x8000];
        var keyBox = GetKeyBox();
        while (true)
        {
            var read = await sourceStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);
            if (read <= 0)
                break;

            for (var i = 0; i < read; i++)
            {
                var j = (i + 1) & 0xff;
                buffer[i] ^= keyBox[(keyBox[j] + keyBox[(keyBox[j] + j) & 0xff]) & 0xff];
            }

            await destinationStream.WriteAsync(buffer, 0, read, cancellationToken).ConfigureAwait(false);
        }
    }

    public static The163KeyClass GetNCMMusicInfo(Stream stream)
    {
        The163KeyClass? keys = null;
        stream.Seek(2, SeekOrigin.Current);
        var coreKeyChunk = ReadChunk(stream)
            ?? throw new InvalidDataException("The NCM file does not contain an encryption key.");
        for (var i = 0; i < coreKeyChunk.Length; i++) coreKeyChunk[i] ^= 0x64;

        coreKeyChunk = AesDecrypt(coreKeyChunk, _coreBoxKey);

        var finalKey = new byte[coreKeyChunk.Length - 17];
        Array.Copy(coreKeyChunk, 17, finalKey, 0, finalKey.Length);
        _keyBox = new byte[256];
        for (var i = 0; i < _keyBox.Length; i++) _keyBox[i] = (byte)i;
        byte last_byte = 0;
        byte key_offset = 0;

        for (var i = 0; i < _keyBox.Length; i++)
        {
            byte swap = _keyBox[i];
            byte c = (byte)((swap + last_byte + finalKey[key_offset++]) & 0xff);
            if (key_offset >= finalKey.Length) key_offset = 0;
            _keyBox[i] = _keyBox[c];
            _keyBox[c] = swap;
            last_byte = c;
        }

        var dontModifyChunk = ReadChunk(stream);

        if (dontModifyChunk != null)
        {
            var startIndex = 0;
            for (var i = 0; i < dontModifyChunk.Length; i++)
            {
                dontModifyChunk[i] ^= 0x63;
                if (dontModifyChunk[i] == 58 && startIndex == 0) startIndex = i + 1;
            }

            var dontModifyDecryptChunk = Convert.FromBase64String(Encoding.UTF8.GetString(dontModifyChunk,
                startIndex, dontModifyChunk.Length - startIndex));
            dontModifyDecryptChunk = AesDecrypt(dontModifyDecryptChunk, _modifyBoxKey);

            // skip `music:`
            using var reader = new MemoryStream(dontModifyDecryptChunk, 6, dontModifyDecryptChunk.Length - 6);
            var infoStr = Encoding.UTF8.GetString(reader.ToArray());
            keys = JsonSerializer.Deserialize<The163KeyClass>(infoStr);
        }

        return keys ?? throw new InvalidDataException("The NCM file does not contain valid music metadata.");
    }

    public static bool IsCorrectNCMFile(Stream stream)
    {
        var buffer = new byte[8];
        stream.Seek(0, SeekOrigin.Begin);
        ReadExactly(stream, buffer, buffer.Length);
        return buffer.SequenceEqual(_flag);
    }


    private static byte[] AesDecrypt(byte[] data, byte[] key)
    {
        var aes = Aes.Create();
        aes.Key = key;
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7;
        using var decryptor = aes.CreateDecryptor();
        return decryptor.TransformFinalBlock(data, 0, data.Length);
    }

    private static byte[]? ReadChunk(Stream fs)
    {
        var len = fs.ReadUInt32();
        if (len > 0)
        {
            var chunk = new byte[len];
            // unsafe
            ReadExactly(fs, chunk, (int)len);
            return chunk;
        }

        return null;
    }

    private static byte[] GetKeyBox()
    {
        return _keyBox is { Length: 256 } keyBox
            ? keyBox
            : throw new InvalidOperationException("NCM music information must be read before decrypting content.");
    }

    public static uint ReadUInt32(this Stream fs)
    {
        var raw = new byte[4];
        var ret = fs.Read(raw, 0, raw.Length);

        if (ret != raw.Length) throw new IOException("out of stream");

        return BitConverter.ToUInt32(raw, 0);
    }

    private static void ReadExactly(Stream stream, byte[] buffer, int count)
    {
        var offset = 0;
        while (offset < count)
        {
            var read = stream.Read(buffer, offset, count - offset);
            if (read == 0)
                throw new EndOfStreamException();

            offset += read;
        }
    }
}

public class The163KeyClass
{
    public ulong AlbumId { get; set; }
    public string Album { get; set; } = string.Empty;
    public long MusicId { get; set; }
    public string MusicName { get; set; } = string.Empty;
    public double Duration { get; set; }
    public int Bitrate { get; set; }
    public string AlbumPic { get; set; } = string.Empty;
    public List<List<object>> Artist { get; set; } = [];
    public string Format { get; set; } = string.Empty;
}

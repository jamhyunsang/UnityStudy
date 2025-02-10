using System.IO.Compression;
using System.IO;
using Newtonsoft.Json;
using System.Security.Cryptography;

public static class Util
{
    #region Json
    public static string ToJson(object Obj)
    {
        return JsonConvert.SerializeObject(Obj);
    }

    public static T ToObject<T>(string Str)
    {
        return JsonConvert.DeserializeObject<T>(Str);
    }
    #endregion Json

    #region DeCompress And Decrypt
    public static string DeCompress(byte[] bytes)
    {
        using (var inputStream = new MemoryStream(bytes))
        {
            using (var brotliStream = new BrotliStream(inputStream, CompressionMode.Decompress))
            {
                using (var streamReader = new StreamReader(brotliStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }

    public static byte[] Decrypt(byte[] bytes)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = Def.EncryptKey;
            aes.IV = Def.EncryptIV;
            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
                using (var memoryStream = new MemoryStream(bytes))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var outputStream = new MemoryStream())
                        {
                            cryptoStream.CopyTo(outputStream);
                            return outputStream.ToArray();
                        }
                    }
                }
            }
        }
    }
    #endregion
}

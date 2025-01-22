using System.IO.Compression;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Security.Cryptography;

public static class Util
{
    public static string ToJson(object Obj)
    {
        return JsonConvert.SerializeObject(Obj);
    }

    public static T ToObject<T>(string Str)
    {
        return JsonConvert.DeserializeObject<T>(Str);
    }

    public static string Decompress(string Str)
    {
        var Data = ToObject<byte[]>(Str);
        using (var InputStream = new MemoryStream(Data))
        {
            using (var BrotliStream = new BrotliStream(InputStream, CompressionMode.Decompress))
            {
                using (var OutputStream = new MemoryStream())
                {
                    BrotliStream.CopyTo(OutputStream);
                    return Encoding.UTF8.GetString(OutputStream.ToArray());
                }
            }
        }
    }

    public static string Decrypt(string encryptedData)
    {
        var ByteData = ToObject<byte[]>(encryptedData);

        using (var AES = Aes.Create())
        {
            AES.Key = Def.EncryptKey;
            AES.IV = Def.EncryptIV;
            using (var Decryptor = AES.CreateDecryptor())
            {
                using (var InputStream = new MemoryStream(ByteData))
                {
                    using (var CryptoStream = new CryptoStream(InputStream, Decryptor, CryptoStreamMode.Read))
                    {
                        using (var reader = new StreamReader(CryptoStream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}

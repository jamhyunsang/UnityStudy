using System.Security.Cryptography;
using System.Text;

public static class Def
{
    public static readonly byte[] EncryptKey = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("YouHyunsang"));
    public static readonly byte[] EncryptIV = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("JamHyunsang"));
}

public enum Scene
{
    None,

    Title,
    Lobby,
    Game,

    Max
}

public enum UI
{
    BackGround,
    Main,
    Popup,
    Fade
}
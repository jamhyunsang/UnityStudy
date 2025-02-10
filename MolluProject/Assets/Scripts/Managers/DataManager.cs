using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    #region Override Method
    protected override void Init()
    {
        m_GameTable = new Dictionary<TableType, object>();
    }
    #endregion

    #region Member Property
    private int TableVersion;
    private Dictionary<TableType, object> m_GameTable = null;
    #endregion

    #region Member Method
    public async UniTask Load()
    {
        JArray Arr = new JArray();
        var VersionBytes = await ResourceManager.Instance.LoadResourceAsync<TextAsset>($"Table/Version", false);
        var VersionDecrypt = Util.Decrypt(VersionBytes.bytes);
        var VersionDeCompress = Util.DeCompress(VersionDecrypt);
        TableVersion = int.Parse(VersionDeCompress);

        for (TableType Type = 0; Type < TableType.End; Type++)
        {
            TextAsset TableBytes = await ResourceManager.Instance.LoadResourceAsync<TextAsset>($"Table/{Type.ToString()}", false);
            var TableDecrypt = Util.Decrypt(TableBytes.bytes);
            var TableDeCompress = Util.DeCompress(TableDecrypt);

            JObject Obj = new JObject();
            Obj.Add("Key", Type.ToString());
            Obj.Add("Value", TableDeCompress);

            Arr.Add(Obj);
        }

        m_GameTable = GameTable.Parse(Arr);
    }

    public List<T> GetTable<T>(TableType gameTable) where T : GameTable
    {
        if (m_GameTable.ContainsKey(gameTable))
        {
            if (m_GameTable[gameTable] is Dictionary<int, T>)
            {
                return m_GameTable[gameTable] as List<T>;
            }
            else
            {
                Debug.LogError($"TableType {gameTable} is not {typeof(T).Name}");
                return null;
            }
        }
        else
        {
            Debug.LogError($"TableType {gameTable} is not exist");
            return null;
        }
    }

    public List<JObject> GetTable(string tableName)
    {
        TableType type = Enum.Parse<TableType>(tableName);

        if (m_GameTable.ContainsKey(type))
        {
            return Util.ToObject<List<JObject>>(Util.ToJson(m_GameTable[type]));
        }
        else
        {
            return new List<JObject>();
        }
    }
    #endregion
}

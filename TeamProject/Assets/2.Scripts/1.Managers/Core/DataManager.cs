using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class DataManager
{    
    public Dictionary<int, RequiredEXPByLevel> Dict_RequiredExp { get; private set; }

    const string DRE = "RequiredEXPByLevel";

    public void Init()
    {
        LoadData();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        string text = Managers._file.LoadJsonFile(path);
        if (!string.IsNullOrEmpty(text))
            return JsonUtility.FromJson<Loader>(text);
        else
        {
            TextAsset data = Resources.Load<TextAsset>($"Json/{path}");
            Loader datas = JsonUtility.FromJson<Loader>(data.ToString());
            Managers._file.SaveJsonFile(datas, path);
            return datas;
        }
    }

    void LoadData()
    {
        Dict_RequiredExp = new Dictionary<int, RequiredEXPByLevel>();
        Dict_RequiredExp = LoadJson<EXPData, int, RequiredEXPByLevel>(DRE).Make();
    }
}

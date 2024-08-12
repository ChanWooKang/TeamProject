using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _uniqueInstance;
    static Managers _inst { get { Init(); return _uniqueInstance; } }

    #region [ Core ] 
    InputManager Input = new InputManager();
    FileManager File = new FileManager();
    DataManager Data = new DataManager();
    public static InputManager _input { get { return _inst.Input; } }
    public static FileManager _file { get { return _inst.File; } }
    public static DataManager _data { get { return _inst.Data; } }
    #endregion [ Core ]

    #region [ Data ] 
    const string objName = "@Managers";
    #endregion [ Data ]

    static void Init()
    {
        if (_uniqueInstance == null)
        {
            GameObject go = GameObject.Find(objName);
            if (go == null)
            {
                go = new GameObject { name = objName };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            _uniqueInstance = go.GetComponent<Managers>();
            _uniqueInstance.File.Init();
            _uniqueInstance.Data.Init();
        }
    }

    public static void Clear()
    {
        _input.Clear();
    }

    void Update()
    {
        _input.OnUpdate();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DefineDatas;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public static string nextScene;

    public void Init()
    {

    }

    public void Clear()
    {
        CurrentScene.Clear();
    }

    
}

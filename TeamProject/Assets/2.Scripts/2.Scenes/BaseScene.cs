using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BaseScene : MonoBehaviour
{
    public eScene PrevScene { get; protected set; } = eScene.Unknown;
    public eScene CurrScene { get; protected set; } = eScene.Unknown;

    private void Awake()
    {
        Init();                
    }

    protected virtual void Init()
    {
        PoolingManager._inst.LoadObjectPool();        
    }

    public void SceneLoad(eScene scene)
    {
        StartCoroutine(Managers._scene.LoadCoroutine(scene));
    }

    public virtual void Clear()
    {
        PoolingManager._inst.Clear();
        PrevScene = CurrScene;
    }
}

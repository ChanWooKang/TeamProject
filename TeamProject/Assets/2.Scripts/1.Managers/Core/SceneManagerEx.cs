using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DefineDatas;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }   
    
    public void Init()
    {

    }

    public IEnumerator LoadCoroutine(eScene scene)
    {
        Managers.Clear();
        string sceneName = Utilitys.ConvertEnum(scene);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);        
        while (!operation.isDone)
        {
            yield return null;
        }        
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }

    
}

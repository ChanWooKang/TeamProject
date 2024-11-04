using DefineDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    [SerializeField] Image progress;
    private void Start()
    {
        StartCoroutine(LoadCoroutine());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene(Utilitys.ConvertEnum(eScene.LoadingScene));
    }

    public IEnumerator LoadCoroutine()
    {
        yield return null;
        Managers.Clear();        
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);
        operation.allowSceneActivation = false;
        float timer = 0.0f;
        while (!operation.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (operation.progress < 0.9f)
            {
                progress.fillAmount = Mathf.Lerp(progress.fillAmount, operation.progress, timer);
                if (progress.fillAmount >= operation.progress)
                    timer = 0f;
            }
            else
            {
                progress.fillAmount = Mathf.Lerp(progress.fillAmount, 1, timer);
                if(progress.fillAmount == 1.0f)
                {
                    operation.allowSceneActivation |= true;
                    yield break;
                }
            }
        }
    }
}

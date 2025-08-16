using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : SingletonMono<LoadSceneManager>
{
    private string sceneName;
    private int sceneIndex;
    private Action callback;
    public event Action<float> OnUpdateProgressEvent;
    public void LoadSceneById(int sceneIndex, Action callback)
    {
        sceneName = string.Empty;
        this.sceneIndex = sceneIndex;
        this.callback = callback;
        StopCoroutine(LoadSceneProgress());
        StartCoroutine(LoadSceneProgress());
    }

    public void LoadSceneByName(string sceneName, Action callback)
    {
        sceneIndex = -1;
        this.sceneName = sceneName;
        this.callback = callback;
        StopCoroutine(LoadSceneProgress());
        StartCoroutine(LoadSceneProgress());
    }
    private AsyncOperation asyncOperation;
    private float currentProgress;
    IEnumerator LoadSceneProgress()
    {
        currentProgress = 0f;
        yield return new WaitForEndOfFrame();
        asyncOperation = sceneIndex > 0 ? SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single) : SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        int count = 0; ;
        while(count<50)
        {
            yield return new WaitForSeconds(.05f);
            currentProgress += 1;
            count++;
            OnUpdateProgressEvent?.Invoke(currentProgress);
        }
        yield return new WaitForSeconds(2f);
        while(count<98)
        {
            yield return new WaitForSeconds(.05f);
            currentProgress += 1;
            count++;
            OnUpdateProgressEvent?.Invoke(currentProgress);
        }
        yield return new WaitForSeconds(1f);
        
        while(asyncOperation is { isDone: false }) // <=> asyncOperation != null && asyncOperation.isDone == false
        {
            yield return new WaitForSeconds(0.01f);
            currentProgress = asyncOperation.progress * 100f;
            OnUpdateProgressEvent?.Invoke(currentProgress);
        }
        currentProgress = 100f ;
        OnUpdateProgressEvent?.Invoke(currentProgress);
        callback();
        
    }
}

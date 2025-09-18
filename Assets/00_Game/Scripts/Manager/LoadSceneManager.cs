using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : SingletonMono<LoadSceneManager>
{
    private string sceneName;
    private Action callback;
    public event Action<float> OnUpdateProgressEvent;
    private float currentProgress;

    public void LoadSceneByName(string newSceneName, Action newCallback)
    {
        sceneName = newSceneName;
        callback = newCallback;
        StopCoroutine(nameof(LoadSceneProgress));
        StartCoroutine(nameof(LoadSceneProgress));
    }

    IEnumerator LoadSceneProgress()
    {
        UIManager.Instance.OpenUI<CanvasLoading>();
        
        currentProgress = 0f;
        yield return new WaitForEndOfFrame();

        // Khởi tạo AsyncOperation nhưng chưa cho phép active scene
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        if (asyncOperation != null)
        {
            asyncOperation.allowSceneActivation = false;

            // Giả lập progress đến ~90%
            int count = 0;
            while (count < 50)
            {
                yield return new WaitForSeconds(0.05f);
                currentProgress += 1;
                count++;
                OnUpdateProgressEvent?.Invoke(currentProgress);
            }

            // Lấy tiến trình thực sự từ asyncOperation (0 -> 0.9)
            while (asyncOperation.progress < 0.9f)
            {
                yield return new WaitForSeconds(0.01f);
                currentProgress = asyncOperation.progress * 100f;
                OnUpdateProgressEvent?.Invoke(currentProgress);
            }

            // Đưa progress lên 100% từ từ (UI mượt hơn)
            while (currentProgress < 100f)
            {
                yield return new WaitForSeconds(0.02f);
                currentProgress += 1f;
                OnUpdateProgressEvent?.Invoke(currentProgress);
            }
            UIManager.Instance.CloseUI<CanvasLoading>(0);
            yield return new WaitForSeconds(1f);
            
            // Khi UI đã đầy 100% thì mới cho phép active scene
            asyncOperation.allowSceneActivation = true;

            // Chờ scene thực sự được active
            while (!asyncOperation.isDone)
                yield return null;
            
            callback?.Invoke();
        }
    }
}

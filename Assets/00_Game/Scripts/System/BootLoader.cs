using System;
using System.Collections;
using UnityEngine;
using CONSTANT;
public class BootLoader : MonoBehaviour
{
    [SerializeField] private DataControl dataControl;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        ConfigManager.Instance.Init(() =>
        {
            // 1.Init Config Done
            
            // 2.Login
            
            // 3. Load Buffer Scene
            UIManager.Instance.OpenUI<CanvasLoading>();
            dataControl.Init(() =>
            {
                LoadSceneManager.Instance.LoadSceneByName(SceneName.BufferScene, () =>
                {
                    UIManager.Instance.CloseAll();
                    UIManager.Instance.OpenUI<CanvasHome>();
                });
            });
        });
    }
}

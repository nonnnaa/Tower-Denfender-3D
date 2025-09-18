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
            dataControl.Init(() =>
            {
                LoadSceneManager.Instance.LoadSceneByName("ThinhScene", () =>
                {
                    //UIManager.Instance.CloseUI<CanvasLoading>(0);
                    UIManager.Instance.OpenUI<CanvasHome>();
                });
            });
        });
    }
}

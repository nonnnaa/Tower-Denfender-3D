using UnityEngine;
using UnityEngine.UI;
using CONSTANT;

public class CanvasWin : UICanvas
{
    [SerializeField] private Button nextButton;

    protected override void Awake()
    {
        base.Awake();
        nextButton.onClick.AddListener(OnClickNextButton);
    }
    
    private void OnClickNextButton()
    {
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<CanvasLoadingInGame>();
        LoadSceneManager.Instance.LoadSceneByName(SceneName.BufferScene, () =>
        {
            UIManager.Instance.CloseUI<CanvasLoadingInGame>(0f);
            UIManager.Instance.OpenUI<CanvasHome>();
        });
    }
}

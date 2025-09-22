using UnityEngine;
using UnityEngine.UI;

public class CanvasLevelSelect : UICanvas
{
    [SerializeField] private Button levelSelectButton;
    protected override void Awake()
    {
        base.Awake();
        levelSelectButton.onClick.AddListener(OnClickLevelSelectButton);
    }

    private void OnClickLevelSelectButton()
    {
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<CanvasLoadingInGame>();
        LoadSceneManager.Instance.LoadSceneByName($"ThinhScene", () =>
        {
            UIManager.Instance.CloseUI<CanvasLoadingInGame>(0);
            UIManager.Instance.OpenUI<CanvasGamePlay>();
        });
    }
}

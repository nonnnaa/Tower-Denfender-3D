using UnityEngine;
using UnityEngine.UI;

public class CanvasHome : UICanvas
{
    [SerializeField] private Button startSelectLevel;

    protected override void Awake()
    {
        base.Awake();
        startSelectLevel.onClick.AddListener(OnClickStartSelectLevel);
    }

    private void OnClickStartSelectLevel()
    {
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<CanvasLevelSelect>();
    }
}

using UnityEngine;
using UnityEngine.UI;

public class CanvasPauseGame : UICanvas
{
    [SerializeField] private Button closeButton;

    protected override void Awake()
    {
        base.Awake();
        closeButton.onClick.AddListener(OnClickCloseButton);
    }

    private void OnClickCloseButton()
    {
        Close(0f);
    }
}

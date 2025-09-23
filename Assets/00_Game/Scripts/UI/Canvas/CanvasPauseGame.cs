using UnityEngine;
using UnityEngine.UI;

public class CanvasPauseGame : UICanvas
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;

    protected override void Awake()
    {
        base.Awake();
        closeButton.onClick.AddListener(OnClickCloseButton);
        quitButton.onClick.AddListener(OnClickQuitButton);
        resumeButton.onClick.AddListener(OnClickResumeButton);
    }

    private void OnClickCloseButton()
    {
        Close(0f);
    }

    private void OnClickQuitButton()
    {
        GameLevelManager.Instance.OnEndLevel();
    }

    private void OnClickResumeButton()
    {
        Close(0f);
    }
}

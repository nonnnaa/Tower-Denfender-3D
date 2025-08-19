using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CONSTANT;

public class CanvasLoading : UICanvas
{
    [SerializeField] private Slider loadingValueSlider;
    [SerializeField] private TextMeshProUGUI loadingValueText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Animator loadingAnimator;

    protected override void Awake()
    {
        base.Awake();
        if (LoadSceneManager.Instance != null)
        {
            LoadSceneManager.Instance.OnUpdateProgressEvent += UpdateLoadingUI;
        }
    }

    public override void Open()
    {
        base.Open();
        //Debug.Log("Open");
        if (canvasGroup == null) return;
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 1)
            .SetLink(canvasGroup.gameObject) // tween sẽ bị kill khi object bị destroy
            .OnComplete(() =>
            {
                canvasGroup.alpha = 1;
                if (LoadSceneManager.Instance != null)
                    LoadSceneManager.Instance.LoadSceneByName(SceneName.InGameScene, FadeUI);
            });
    }

    private void FadeUI()
    {
        if (canvasGroup == null) return;
        canvasGroup.DOFade(0, 1)
            .SetLink(canvasGroup.gameObject)
            .OnComplete(() => 
            {
                Close(0f);
            });
    }

    private void UpdateLoadingUI(float progressValue)
    {
        if (loadingValueSlider == null || loadingValueText == null) return;

        loadingValueSlider.value = progressValue;
        loadingValueText.text = $"Loading... {(int)progressValue}%";
    }

    public override void Close(float time)
    {
        base.Close(time);
        if (canvasGroup != null)
            DOTween.Kill(canvasGroup); 
    }
}
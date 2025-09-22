using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class CanvasLoading : UICanvas
{
    [SerializeField] private Slider loadingValueSlider;
    [SerializeField] private TextMeshProUGUI loadingValueText;
    [SerializeField] private CanvasGroup canvasGroup;

    protected override void Awake()
    {
        base.Awake();
        LoadSceneManager.Instance.OnUpdateProgressEvent += UpdateLoadingUI;
    }

    public override void Open()
    {
        base.Open();
        if (canvasGroup == null) return;
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 1).SetLink(gameObject).OnComplete(() =>
        {
            canvasGroup.alpha = 1;
        });
    }
    private void UpdateLoadingUI(float progressValue)
    {
        if (loadingValueSlider == null || loadingValueText == null) return;

        loadingValueSlider.value = progressValue;
        loadingValueText.text = $"Loading... {(int)progressValue}%";
    }
}
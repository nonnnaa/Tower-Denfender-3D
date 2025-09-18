using System;
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
        //Debug.Log("Open");
        if (canvasGroup == null) return;
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 1)
            .SetLink(canvasGroup.gameObject) // tween sẽ bị kill khi object bị destroy
            .OnComplete(() =>
            {
                canvasGroup.alpha = 1;
            });
    }

    private void FadeUI(Action callback)
    {
        if (canvasGroup == null) return;
        canvasGroup.DOFade(0, 1)
            .SetLink(canvasGroup.gameObject);
    }

    private void UpdateLoadingUI(float progressValue)
    {
        if (loadingValueSlider == null || loadingValueText == null) return;

        loadingValueSlider.value = progressValue;
        loadingValueText.text = $"Loading... {(int)progressValue}%";
    }

    public override void Close(float time)
    {
        FadeUI(() =>
        {
            base.Close(time);
            if (canvasGroup != null)
            {
                DOTween.Kill(canvasGroup); 
            }
        });
    }
}
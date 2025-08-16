using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Constant;
public class CanvasLoading : UICanvas
{
    [SerializeField] private Slider loadingValueSlider;
    [SerializeField] private TextMeshProUGUI  loadingValueText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Animator loadingAnimator;

    protected override void Awake()
    {
        base.Awake();
        LoadSceneManager.Instance.OnUpdateProgressEvent += UpdateLoadingUI;
    }
    public override void Open()
    {
        base.Open();
        Debug.Log("Open");
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 1).OnComplete(() =>
        {
            canvasGroup.alpha = 1;
            LoadSceneManager.Instance.LoadSceneByName(SceneName.InGameScene, null);
        });
    }
    
    void UpdateLoadingUI(float progressValue)
    {
        loadingValueSlider.value = (int)progressValue;
        loadingValueText.text = $"Loading... {(int)progressValue}%";
    }
}

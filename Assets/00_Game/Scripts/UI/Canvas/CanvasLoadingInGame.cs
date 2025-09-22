using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasLoadingInGame : UICanvas
{
    [SerializeField] private Slider loadingValueSlider;
    [SerializeField] private TextMeshProUGUI loadingValueText;

    protected override void Awake()
    {
        base.Awake();
        LoadSceneManager.Instance.OnUpdateProgressEvent += UpdateLoadingUI;
    }
    private void UpdateLoadingUI(float progressValue)
    {
        if (loadingValueSlider == null || loadingValueText == null) return;
        loadingValueSlider.value = progressValue;
        loadingValueText.text = $"Loading... {(int)progressValue}%";
    }
}
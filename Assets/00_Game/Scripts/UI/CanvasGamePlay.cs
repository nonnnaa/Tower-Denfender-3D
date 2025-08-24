using TMPro;
using UnityEngine;

public class CanvasGamePlay : UICanvas
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI text;

    public override void Setup()
    {
        base.Setup();
        coinText.text = "0";
    }
    public void UpdateCoinText(int coin)
    {
        coinText.text = coin.ToString();
    }
    public void SettingButton()
    {
        UIManager.Instance.OpenUI<CanvasSetting>().SetState(this);
    }

    public void PauseButton()
    {
        text.text = "Continue";
        Time.timeScale = 0;
    }
}

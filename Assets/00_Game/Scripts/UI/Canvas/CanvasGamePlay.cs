using UnityEngine;
using UnityEngine.UI;

public class CanvasGamePlay : UICanvas
{
    [SerializeField] private TurretIconControl turretIconControl;
    [SerializeField] private SellTurretControl sellTurretControl;
    [SerializeField] private Button settingButton;

    protected override void Awake()
    {
        base.Awake();
        settingButton.onClick.AddListener(OnClickSettingButton);
    }

    public override void Setup()
    {
        turretIconControl.Init(ConfigManager.Instance.GetFileConfigTurret().GetAllFileConfigTurretRecord());
        sellTurretControl.Init();
    }

    public void OnClickSettingButton()
    {
        UIManager.Instance.OpenUI<CanvasSetting>();
        Debug.Log("Setting Button Onclick");
    }
}

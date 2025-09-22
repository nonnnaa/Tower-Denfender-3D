using UnityEngine;
using UnityEngine.UI;

public class CanvasGamePlay : UICanvas
{
    [SerializeField] private TurretIconControl turretIconControl;
    [SerializeField] private SellTurretControl sellTurretControl;
    [SerializeField] private Button pauseButton;
    [SerializeField] private RectTransform hudHp;
    protected override void Awake()
    {
        base.Awake();
        pauseButton.onClick.AddListener(OnClickPauseButton);
    }

    public override void Setup()
    {
        turretIconControl.Init(ConfigManager.Instance.GetFileConfigTurret().GetAllFileConfigTurretRecord());
        sellTurretControl.Init();
    }

    public void OnClickPauseButton()
    {
        UIManager.Instance.OpenUI<CanvasPauseGame>();
        GameManager.Instance.ChangeGameState(GameManager.GameState.PAUSE);
        Debug.Log("Pause Button Onclick");
    }
}

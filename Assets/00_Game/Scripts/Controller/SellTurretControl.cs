using UnityEngine;
using UnityEngine.UI;

public class SellTurretControl : MonoBehaviour
{
    [SerializeField] private Button yesButton, noButton;

    private void Awake()
    {
        yesButton.onClick.AddListener(SellTurret);
        noButton.onClick.AddListener(DontSellTurret);
        EventManager.Instance.OnOpenSellTurretPopup += Open;
    }

    public void Init()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void SellTurret()
    {
        
    }

    public void DontSellTurret()
    {
        gameObject.SetActive(false);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Close();
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretIconControl : MonoBehaviour
{
    [SerializeField] private TurretIcon turretIconPrefab;
    [SerializeField] private List<TurretIcon> turretIcons = new List<TurretIcon>();
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(OnClose);
        EventManager.Instance.OnOpenTurretSelection += OnOpen;
    }

    public void Init(List<FileConfigTurretRecord> records)
    {
        foreach (FileConfigTurretRecord record in records)
        {
            TurretIcon turretIcon = Instantiate(turretIconPrefab, transform);
            turretIcon.Init(record);
            turretIcons.Add(turretIcon);
        }
        OnClose();
    }

    private void OnOpen()
    {
        gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
    }

    private void OnClose()
    {
        gameObject.SetActive(false);  
        closeButton.gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        EventManager.Instance.OnOpenTurretSelection -= OnOpen;
    }
}

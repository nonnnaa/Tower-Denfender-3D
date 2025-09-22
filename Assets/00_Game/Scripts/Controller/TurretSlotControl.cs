using UnityEngine;
using UnityEngine.EventSystems;

public class TurretSlotControl : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Transform spawnPoint;
    private bool isTaken;
    public static TurretSlotControl CurrentSelectedSlot { get; private set; }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isTaken)
        {
            EventManager.Instance.OnOpenSellTurretPopup.Invoke();
        }
        else
        {
            CurrentSelectedSlot = this;
            EventManager.Instance.OnOpenTurretSelection.Invoke(true);
        }
    }

    public void SpawnTurret(string turretName)
    {
        TurretControl go = Instantiate(Resources.Load("Turret/" + turretName, typeof(TurretControl)) as TurretControl, spawnPoint, true);
        go.transform.localPosition = Vector3.zero;
        CurrentSelectedSlot = null;
        isTaken = true;
    }
}
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
        CurrentSelectedSlot = this;
        EventManager.Instance.OnOpenTurretSelection.Invoke();
    }

    public void SpawnTurret(string turretName)
    {
        GameObject go = Instantiate(Resources.Load("Turret/" + turretName, typeof(GameObject)) as GameObject, spawnPoint, true);
        go.transform.localPosition = Vector3.zero;
        CurrentSelectedSlot = null;
        isTaken = true;
    }
}
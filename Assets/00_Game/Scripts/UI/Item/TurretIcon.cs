using UnityEngine;
using UnityEngine.UI;

public class TurretIcon : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    private FileConfigTurretRecord record;
    public void Init(FileConfigTurretRecord newRecord)
    {
        record = newRecord;
        iconImage.sprite = SpriteLibControl.Instance.GetImageByName(record.Name);
    }

    public void OnClick()
    {
        if (TurretSlotControl.CurrentSelectedSlot != null)
        {
            TurretSlotControl.CurrentSelectedSlot.SpawnTurret(record.Name);
        }
    }
}

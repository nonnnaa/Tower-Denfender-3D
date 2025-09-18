using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSelectControl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private string unitKey;
    private float coolDownTime = 3f;
    private bool isLocking;
    [SerializeField] private Image lockImage;
    

    [SerializeField] private GameObject dragUnitIcon;
    private void Awake()
    {
        isLocking = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }


    public void BeginCoolDown()
    {
        StartCoroutine(CoolDown());
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForEndOfFrame();
        float currentCoolDownTime = 0;
        while (currentCoolDownTime < coolDownTime)
        {
            currentCoolDownTime += Time.deltaTime;
            float t = currentCoolDownTime / coolDownTime;
            lockImage.fillAmount = Mathf.Lerp(0, 1, t);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        isLocking = false;
    }
    
    
    public void Init(string newUnitKey)
    {
        unitKey = newUnitKey;
    }
    
    
}

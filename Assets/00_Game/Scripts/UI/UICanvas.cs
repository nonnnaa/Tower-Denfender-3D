using System.Collections;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [SerializeField] bool isDestroyOnClose = false;
    protected virtual void Awake()
    {
        // xu ly tai tho
        RectTransform rect = GetComponent<RectTransform>();
        float ratio = (float)Screen.width / (float)Screen.height;
        if (ratio > 2.1f)
        {
            Vector2 leftBottom = rect.offsetMin;
            Vector2 rightTop = rect.offsetMax;
            
            leftBottom.y = 0;
            rightTop.y = -100;
            
            rect.offsetMin = leftBottom;
            rect.offsetMax = rightTop;
        }
    }

    // goi truoc khi canvas duoc active
    public virtual void Setup()
    {
        
    }
    // goi sau khi active
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }
    // tat canvas sau t (s)
    public virtual void Close(float time)
    {
        StartCoroutine(CloseE(time));
    }

    IEnumerator CloseE(float time)
    {
        yield return new WaitForSeconds(time);
        if (isDestroyOnClose)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
}

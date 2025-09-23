using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using CONSTANT;
public class HpHub : PoolableObject
{
    [SerializeField] private Image hpImage;
    [SerializeField] private CanvasGroup canvasGroup;

    private RectTransform rect;
    private RectTransform parent;
    private Transform anchorHub;

    private Tween twHp;
    private Tween twFade;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    
    public override void OnSpawn(Vector3 position, Transform newParent = null)
    {
        base.OnSpawn(position, newParent);
        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
    }
    
    public override void OnDespawn()
    {
        base.OnDespawn();
        twHp?.Kill();
        twFade?.Kill();
        anchorHub = null;
        parent = null;
        gameObject.SetActive(false);
    }
    
    public void SetupHub(Transform newAnchorHub, RectTransform newParent)
    {
        anchorHub = newAnchorHub;
        parent = newParent;
        transform.SetParent(newParent, false);
        hpImage.fillAmount = 1;
        canvasGroup.alpha = 0;
    }
    
    public void UpdateHp(int cur, int max)
    {
        canvasGroup.alpha = 1;
        float val = (float)cur / max;

        twHp?.Kill();
        twHp = hpImage.DOFillAmount(val, 0.5f).SetEase(Ease.OutCubic);
        twFade?.Kill();
        twFade = canvasGroup.DOFade(0, 0.5f).SetDelay(1f).OnComplete(() =>
        {
            PoolManager.Instance.ReleaseToPool(UIElementName.HpHub, this);
        });
    }

    private void Update()
    {
        if (anchorHub != null && parent != null)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(anchorHub.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, screenPoint, null, out Vector2 localPoint);
            rect.anchoredPosition = localPoint;
        }
    }
}

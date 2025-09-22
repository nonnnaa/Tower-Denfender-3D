using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using CONSTANT;
public class HpHub : PoolableObject
{
    [Header("UI Elements")]
    [SerializeField] private Image hpFG;
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
    public override void OnSpawn(Vector3 position, Transform parent = null)
    {
        base.OnSpawn(position, parent);
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
    
    public void SetupHub(Transform anchorHub, RectTransform parent)
    {
        this.anchorHub = anchorHub;
        this.parent = parent;
        transform.SetParent(parent, false);
        
        hpFG.fillAmount = 1;
        canvasGroup.alpha = 0;
    }
    
    public void UpdateHP(int cur, int max)
    {
        canvasGroup.alpha = 1;

        float val = (float)cur / (float)max;

        twHp?.Kill();
        twHp = hpFG.DOFillAmount(val, 0.5f).SetEase(Ease.OutCubic);

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
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, screenPoint, null, out localPoint);
            rect.anchoredPosition = localPoint;
        }
    }
}

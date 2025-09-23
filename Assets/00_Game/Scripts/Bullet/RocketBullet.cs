using UnityEngine;
using DG.Tweening;
using CONSTANT;
public class RocketBullet : PoolableObject, IBullet
{
    [Header("Rocket Settings")]
    [SerializeField] private float speed = 15f;
    [SerializeField] private float minCurveHeight = 5f;
    [SerializeField] private float maxCurveHeight = 50f;
    [SerializeField] private float minDistance = 5f; // follow turret setting
    [SerializeField] private float maxDistance = 100f; // follow turret setting
    [Tooltip("Điều chỉnh độ 'thẳng' của rocket khi bắt đầu bay. Giá trị càng lớn, rocket bay thẳng càng xa trước khi bẻ cong.")]
    [SerializeField] private float initialForwardInfluence = 0.4f;
    [SerializeField] private float delayAfterHit = 0.1f;

    [Header("Pool Keys")]
    [SerializeField] private string projectileName;
    [SerializeField] private string impactName;

    private Transform currentTarget;
    private Vector3 targetPosition;
    private Tween moveTween;
    private Vector3 lastPos;
    
    public void SetTarget(Transform newTarget)
    {
        currentTarget = newTarget;
        if (currentTarget != null)
            targetPosition = currentTarget.position;
        StartShooting();
    }

    public void Shoot(Vector3 direction)
    {
        transform.forward = direction.normalized;
        targetPosition = transform.position + direction.normalized * 10f;
        StartShooting();
    }

    private void StartShooting()
    {
        if (moveTween != null && moveTween.IsActive())
            moveTween.Kill();

        Vector3 startPoint = transform.position;
        Vector3 initialForward = transform.forward; // Lưu lại hướng bắn ban đầu

        // Khóa vị trí mục tiêu
        if (currentTarget != null)
            targetPosition = currentTarget.position;
        
        Vector3 endPoint = targetPosition;

        // --- Tính toán các điểm cho đường cong Bézier bậc ba ---
        float distance = Vector3.Distance(startPoint, endPoint);

        // Điểm kiểm soát 1: Nằm phía trước rocket để đảm bảo hướng bay ban đầu.
        // Khoảng cách của điểm này tỷ lệ với tổng quãng đường bay.
        Vector3 controlPoint1 = startPoint + initialForward * distance * initialForwardInfluence;

        // Điểm kiểm soát 2: Nằm phía trên mục tiêu để tạo ra đường vòng cung đi xuống.
        float t = Mathf.InverseLerp(minDistance, maxDistance, distance);
        float dynamicCurve = Mathf.Lerp(minCurveHeight, maxCurveHeight, t);
        Vector3 controlPoint2 = endPoint + Vector3.up * dynamicCurve;
        
        float duration = distance / speed;
        lastPos = startPoint;

        // --- Bay bằng DOTween.To để tính toán đường cong Bézier bậc ba ---
        moveTween = DOTween.To(
            setter: p =>
            {
                // Công thức tính toán vị trí trên đường cong Bézier bậc ba
                float oneMinusP = 1f - p;
                Vector3 newPos = Mathf.Pow(oneMinusP, 3) * startPoint +
                                 3f * Mathf.Pow(oneMinusP, 2) * p * controlPoint1 +
                                 3f * oneMinusP * Mathf.Pow(p, 2) * controlPoint2 +
                                 Mathf.Pow(p, 3) * endPoint;

                transform.position = newPos;

                // Cập nhật hướng bay của rocket để nó luôn hướng theo quỹ đạo
                Vector3 dir = transform.position - lastPos;
                if (dir.sqrMagnitude > 0.0001f)
                {
                    transform.forward = dir.normalized;
                }
                lastPos = transform.position;
            },
            startValue: 0f,
            endValue: 1f,
            duration: duration
        )
        .SetEase(Ease.Linear) // Giữ tốc độ di chuyển trên đường cong ổn định
        .OnComplete(() =>
         {
            HitTarget(currentTarget != null ? currentTarget.gameObject : null, targetPosition); 
         })
        .SetLink(gameObject);
    }
    
    private void ReleaseToPool()
    {
        if (moveTween != null && moveTween.IsActive())
            moveTween.Kill();
        PoolManager.Instance.Despawn(projectileName, this);
    }

    public void HitTarget(GameObject enemy, Vector3 point)
    {
        var impact = PoolManager.Instance.Spawn(impactName, point, null);
        if (impact != null)
        {
            var imp = impact.GetComponent<Impact>();
            if (imp != null) imp.SetScaleParticleSystem(3f);
        }

        if (enemy != null)
        {
            var enemyControl = enemy.GetComponent<EnemyControl>();
            if (enemyControl != null)
            {
                enemyControl.TakeDamage(1);
            }
        }

        Invoke(nameof(ReleaseToPool), delayAfterHit);
    }

    //--- IPoolable ---
    public override void OnSpawn(Vector3 position, Transform newParent = null)
    {
        base.OnSpawn(position, newParent);
        transform.position = position;
        if (newParent != null)
        {
            transform.SetParent(newParent);
        }
        gameObject.SetActive(true);
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        if (moveTween != null && moveTween.IsActive())
            moveTween.Kill();
        transform.SetParent(PoolManager.Instance.transform);
        PoolManager.Instance.ReleaseToPool(ProjectileName.ProjectileRocketTurret,this);
        gameObject.SetActive(false);
    }
}
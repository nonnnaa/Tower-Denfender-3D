using UnityEngine;
using System.Collections;

public class GatlingBullet : PoolableObject, IBullet
{
    [SerializeField] private float speed = 40f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private float delayAfterHit = 0.05f;
    [SerializeField] private string projectileName = "ProjectileGatlingBullet";
    [SerializeField] private string impactName = "ImpactGatling";

    private Vector3 moveDirection;
    private Coroutine moveCoroutine;
    private static RaycastHit[] hitBuffer = new RaycastHit[5];
    private const float MinMoveDistance = 0.01f;

    public void Shoot(Vector3 direction)
    {
        moveDirection = direction.normalized;
        transform.forward = moveDirection;
        StartShootingCoroutine();
    }

    private void StartShootingCoroutine()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveBulletCoroutine());
    }

    private IEnumerator MoveBulletCoroutine()
    {
        float timer = lifeTime;
        Vector3 previousPosition = transform.position;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            float distanceThisFrame = speed * Time.deltaTime;
            Vector3 newPosition = transform.position + moveDirection * distanceThisFrame;

            float travelDistance = Vector3.Distance(previousPosition, newPosition);

            if (travelDistance > MinMoveDistance)
            {
                int hitCount = Physics.RaycastNonAlloc(previousPosition, moveDirection, hitBuffer, travelDistance);

                if (hitCount > 0)
                {
                    for (int i = 0; i < hitCount; i++)
                    {
                        if (hitBuffer[i].collider.CompareTag("Enemy"))
                        {
                            HitTarget(hitBuffer[i].collider.gameObject, hitBuffer[i].point);
                            yield break;
                        }
                    }
                }
            }

            transform.position = newPosition;
            previousPosition = transform.position;
            yield return null;
        }

        Despawn();
    }

    public void HitTarget(GameObject enemy, Vector3 point)
    {
        PoolManager.Instance.Spawn(impactName, point, null);

        Invoke(nameof(Despawn), delayAfterHit);
    }


    private void Despawn()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
        PoolManager.Instance.Despawn(projectileName, this);
    }

    // PoolableObject overrides
    public override void OnSpawn(Vector3 position, Transform parent = null)
    {
        base.OnSpawn(position, parent);
        transform.position = position;
        transform.SetParent(parent != null ? parent : PoolManager.Instance.transform);
        gameObject.SetActive(true);
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        transform.SetParent(PoolManager.Instance.transform);
        gameObject.SetActive(false);
    }
}

using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    public float speed = 15f;
    public float lifeTime = 3f;

    private float lifeTimer;
    private Vector3 moveDirection;

    private static RaycastHit[] hitBuffer = new RaycastHit[3];

    public void Shoot(Vector3 direction)
    {
        moveDirection = direction.normalized;
        transform.forward = moveDirection;
        lifeTimer = lifeTime;
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            ReleaseToPool();
            return;
        }

        float distanceThisFrame = speed * Time.deltaTime;
        int hitCount = Physics.RaycastNonAlloc(transform.position, moveDirection, hitBuffer, distanceThisFrame);

        if (hitCount > 0)
        {
            for (int i = 0; i < hitCount; i++)
            {
                if (hitBuffer[i].collider.CompareTag("Enemy"))
                {
                    HitTarget(hitBuffer[i].collider.gameObject);
                    return;
                }
            }
        }

        transform.position += moveDirection * distanceThisFrame;
    }

    private void HitTarget(GameObject enemy)
    {
        // TODO: apply damage nếu cần
        ReleaseToPool();
    }

    private void ReleaseToPool()
    {
        PoolManager.Instance.Despawn("BulletPool", transform);
    }

    // IPoolable
    public void OnSpawned(Vector3 position)
    {
        transform.position = position;
        transform.SetParent(PoolManager.Instance.transform);
        gameObject.SetActive(true);
    }

    public void OnDespawned()
    {
        // Reset trạng thái nếu cần (particle, trail renderer...)
    }
}
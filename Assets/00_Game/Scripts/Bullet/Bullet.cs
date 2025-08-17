using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    public float speed = 15f;
    private float lifeTimer;
    private Vector3 moveDirection;

    private static RaycastHit[] hitBuffer = new RaycastHit[10];

    public void Shoot(Vector3 direction)
    {
        moveDirection = direction.normalized;
        transform.forward = moveDirection;
    }

    private void Update()
    {
        float distanceThisFrame = speed * Time.deltaTime;
        int hitCount = Physics.RaycastNonAlloc(transform.position, moveDirection, hitBuffer, distanceThisFrame);

        if (hitCount > 0)
        {
            for (int i = 0; i < hitCount; i++)
            {
                if (hitBuffer[i].collider.CompareTag("Enemy"))
                {
                    HitTarget(hitBuffer[i].collider.gameObject, hitBuffer[i].point);
                    return;
                }
            }
        }

        transform.position += moveDirection * distanceThisFrame;
    }

    private void HitTarget(GameObject enemy, Vector3 point)
    {
        PoolManager.Instance.Spawn("Impact", point);
        ReleaseToPool();
        OnDespawned();
    }

    private void ReleaseToPool()
    {
        PoolManager.Instance.Despawn("Bullet", transform);
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
        gameObject.SetActive(false);
    }
}
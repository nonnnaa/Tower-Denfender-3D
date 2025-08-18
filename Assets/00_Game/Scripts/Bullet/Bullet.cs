using System;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    public float speed = 15f;
    [SerializeField] private float lifeTime = 3f;
    private float timer;
    private Vector3 moveDirection;

    private static RaycastHit[] hitBuffer = new RaycastHit[5];

    private void Start()
    {
        timer = lifeTime;
    }

    public void Shoot(Vector3 direction)
    {
        moveDirection = direction.normalized;
        transform.forward = moveDirection;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = lifeTime;
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
                    HitTarget(hitBuffer[i].collider.gameObject, hitBuffer[i].point);
                    return;
                }
            }
        }

        transform.position += moveDirection * distanceThisFrame;
    }

    private void HitTarget(GameObject enemy, Vector3 point)
    {
        PoolManager.Instance.Spawn("Impact", point, enemy.transform);
        ReleaseToPool();
        OnDespawned();
    }

    private void ReleaseToPool()
    {
        PoolManager.Instance.Despawn("Bullet", transform);
    }

    // IPoolable
    public void OnSpawned(Vector3 position, Transform parent)
    {
        transform.position = position;
        transform.SetParent(parent);
        gameObject.SetActive(true);
    }

    public void OnDespawned()
    {
        gameObject.SetActive(false);
    }
}
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public interface IBullet
{
    public void Shoot(Vector3 direction);
    public void HitTarget(GameObject enemy, Vector3 point);
}
public class ShotGunBullet : MonoBehaviour, IPoolable, IBullet
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private float delayAfterHit;
    [SerializeField] private string projectileName, impactName;
    private Vector3 moveDirection;
    private Coroutine moveCoroutine; 
    private static RaycastHit[] hitBuffer = new RaycastHit[5];
    private const float MinMoveDistance = 0.01f;
    protected Transform currentParent;


    public void SetCurrentParent(Transform parent)
    {
        currentParent = parent;
    }
    protected virtual void HandlePreShooting(float timeDelay = 0f)
    {
        // Handle
        Invoke(nameof(StartShootingCoroutine), timeDelay);
    }
    public void Shoot(Vector3 direction)
    {
        moveDirection = direction.normalized;
        transform.forward = moveDirection;
        HandlePreShooting();
    }

    private void StartShootingCoroutine()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
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
        ReleaseToPool();
    }
    private void ReleaseToPool()
    {
        PoolManager.Instance.Despawn(projectileName, transform);
    }
    
    public void HitTarget(GameObject enemy, Vector3 point)
    {
        PoolManager.Instance.Spawn(impactName, point, enemy.transform);
        PoolManager.Instance.RemoveFromPool(transform, projectileName);
        Invoke(nameof(ReleaseToPool), delayAfterHit);
    }
    // IPoolable
    public void OnSpawned(Vector3 position, Transform newParent)
    {
        transform.position = position;
        if (newParent != null)
        {
             transform.SetParent(newParent);
             currentParent = newParent;
        }
        gameObject.SetActive(true);
    }

    public void OnDespawned()
    {
        PoolManager.Instance.ReturnToPool(transform, projectileName);
        transform.SetParent(PoolManager.Instance.transform);
        gameObject.SetActive(false);
    }
}
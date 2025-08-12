using UnityEngine;

public class Bullet : ObjectPool
{
    public float speed = 15f;
    public float lifeTime = 3f; // Tự hủy nếu không trúng

    private Transform target;
    private float lifeTimer;

    public void SetTarget(Transform enemy)
    {
        target = enemy;
        lifeTimer = lifeTime; // Reset lại thời gian sống mỗi khi bắn
    }

    private void Update()
    {
        // Giảm thời gian sống
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            ReleaseToPool();
            return;
        }

        // Nếu mất target
        if (target == null)
        {
            ReleaseToPool();
            return;
        }

        // Tính hướng bay
        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // Nếu đủ gần để trúng
        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        // Bay về hướng enemy
        transform.position += direction.normalized * distanceThisFrame;
        transform.LookAt(target);
    }

    void HitTarget()
    {
        // TODO: Gây sát thương
        ReleaseToPool();
    }

    private void ReleaseToPool()
    {
        // Trả lại object pool thay vì Destroy
        gameObject.SetActive(false);
    }
}
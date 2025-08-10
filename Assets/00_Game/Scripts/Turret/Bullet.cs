using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float lifeTime = 3f; // Tự hủy sau 3s nếu không trúng

    private Transform target;

    public void SetTarget(Transform enemy)
    {
        target = enemy;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // Nếu đạn đủ gần để coi như trúng mục tiêu
        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        // Bay về hướng enemy
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    void HitTarget()
    {
        // TODO: Thêm xử lý gây sát thương
        Destroy(gameObject);
    }
}
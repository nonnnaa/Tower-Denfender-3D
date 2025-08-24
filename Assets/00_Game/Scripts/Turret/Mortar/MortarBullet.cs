using UnityEngine;

public class MortarBullet : ObjectPool
{
    [Header("Shell Settings")]
    public float lifeTime = 5f;        // tự hủy sau 5 giây
    public float explosionRadius = 3f; // bán kính nổ
    public int damage = 50;

    [Header("References")]
    public GameObject explosionEffect;

    private Rigidbody rb;
    private float lifeTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Bắn đạn đi với vận tốc/lực cho trước
    /// </summary>
    public void Launch(Vector3 force)
    {
        lifeTimer = lifeTime;

        gameObject.SetActive(true);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(force, ForceMode.Impulse);
    }

    private void Update()
    {
        // countdown tự hủy
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Explode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        // hiệu ứng nổ
        if (explosionEffect != null)
        {
            GameObject fx = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(fx, 2f);
        }

        // gây damage AOE
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hits)
        {
            // Enemy enemy = hit.GetComponent<Enemy>();
            // if (enemy != null)
            // {
            //     enemy.TakeDamage(damage);
            // }
        }

        ReleaseToPool();
    }

    /// <summary>
    /// Trả lại object pool
    /// </summary>
    private void ReleaseToPool()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}

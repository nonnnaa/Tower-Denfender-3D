using UnityEngine;

public class MuzzleFlare : PoolableObject
{
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private string muzzleFlareName;
    public float lifeTime = 1.5f;
    private void Awake()
    {
        if (particle == null)
        {
            particle = GetComponent<ParticleSystem>();
        }
    }
    private void Update()
    {
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                PoolManager.Instance.Despawn(muzzleFlareName, this);
            }
        }
    }
    public override void OnSpawn(Vector3 position, Transform newParent = null)
    {
        if (newParent != null)
        {
            Quaternion combinedRotation = newParent.rotation * Quaternion.LookRotation((transform.position - newParent.transform.position).normalized);
            transform.rotation = combinedRotation;
        }
        gameObject.SetActive(true);
        particle.Play();
        lifeTime = 1.5f;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        transform.SetParent(PoolManager.Instance.transform);
        PoolManager.Instance.ReleaseToPool(muzzleFlareName, this);
        gameObject.SetActive(false);
    }
}
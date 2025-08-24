using UnityEngine;

public class MuzzleFlare : PoolableObject
{
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private string muzzleFlareName;
    [SerializeField] private float lifeTime = 1.5f;
    private float currentLifeTime;
    private void Awake()
    {
        if (particle == null)
        {
            particle = GetComponent<ParticleSystem>();
        }
    }
    private void Update()
    {
        if (currentLifeTime > 0)
        {
            currentLifeTime -= Time.deltaTime;
            if (currentLifeTime <= 0)
            {
                PoolManager.Instance.Despawn(muzzleFlareName, this);
            }
        }
    }
    public override void OnSpawn(Vector3 position, Transform newParent = null)
    {
        if (newParent != null)
        {
            transform.SetParent(newParent);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        gameObject.SetActive(true);
        transform.position = position;
        particle.Play();
        currentLifeTime = lifeTime;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        transform.SetParent(PoolManager.Instance.transform);
        PoolManager.Instance.ReleaseToPool(muzzleFlareName, this);
        gameObject.SetActive(false);
    }
}
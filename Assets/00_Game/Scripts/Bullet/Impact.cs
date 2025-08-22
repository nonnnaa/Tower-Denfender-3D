using UnityEngine;
public class Impact : PoolableObject
{
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private string impactName;
    public float lifeTime = 1.5f;
    public float currentLifeTime;
    private void Awake()
    {
        if (particle == null)
        {
            particle = GetComponent<ParticleSystem>();
        }
    }
    public void SetScaleParticleSystem(float scale)
    {
        particle.transform.localScale = new Vector3(scale, scale, scale);
    }
    private void Update()
    {
        if (currentLifeTime > 0)
        {
            currentLifeTime -= Time.deltaTime;
            if (currentLifeTime <= 0)
            {
                PoolManager.Instance.Despawn(impactName, this);
            }
        }
    }
    public override void OnSpawn(Vector3 position, Transform newParent = null)
    {
        base.OnSpawn(position, newParent);
        gameObject.SetActive(true);
        transform.position = position;
        currentLifeTime = lifeTime;
        if (newParent != null)
        {
            transform.SetParent(newParent);
            Quaternion combinedRotation = newParent.rotation * Quaternion.LookRotation((transform.position - newParent.transform.position).normalized);
            transform.rotation = combinedRotation;
        }
        particle.Play();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        particle.Stop();
        currentLifeTime = lifeTime;
        gameObject.transform.SetParent(PoolManager.Instance.transform);
        PoolManager.Instance.ReleaseToPool(impactName, this);
        gameObject.SetActive(false);
    }
}

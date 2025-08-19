using UnityEngine;

public class MuzzleFlare : MonoBehaviour, IPoolable
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
                PoolManager.Instance.Despawn(muzzleFlareName, transform);
            }
        }
    }
    public void OnSpawned(Vector3 position, Transform newParent)
    {
        if (newParent != null)
        {
            Quaternion combinedRotation = newParent.rotation * Quaternion.LookRotation((transform.position - newParent.transform.position).normalized);
            transform.rotation = combinedRotation;
        }
        particle.Play();
        lifeTime = 1.5f;
    }

    public void OnDespawned()
    {
        transform.SetParent(PoolManager.Instance.transform);
        gameObject.SetActive(false);
    }
}
using UnityEngine;
public class Impact : MonoBehaviour, IPoolable
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
                PoolManager.Instance.Despawn(impactName, transform);
            }
        }
    }
    public void OnSpawned(Vector3 position, Transform newParent)
    {
        currentLifeTime = lifeTime;
        if (newParent != null)
        {
            transform.SetParent(newParent);
            Quaternion combinedRotation = newParent.rotation * Quaternion.LookRotation((transform.position - newParent.transform.position).normalized);
            transform.rotation = combinedRotation;
        }
        particle.Play();
    }

    public void OnDespawned()
    {
        currentLifeTime = lifeTime;
        gameObject.transform.SetParent(PoolManager.Instance.transform);
        gameObject.SetActive(false);
    }
}

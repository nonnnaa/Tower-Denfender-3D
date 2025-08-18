using UnityEngine;

public class MuzzleFlare : MonoBehaviour, IPoolable
{
    [SerializeField] private ParticleSystem particle;
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
                PoolManager.Instance.Despawn("MuzzleFlare", transform);
            }
        }
    }
    public void OnSpawned(Vector3 position, Transform parent)
    {
        transform.SetParent(parent);
        particle.Play();
        lifeTime = 1.5f;
    }

    public void OnDespawned()
    {
        gameObject.SetActive(false);
    }
}
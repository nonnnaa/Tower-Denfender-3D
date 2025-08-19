using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonMono<PoolManager>
{
    public List<ObjectPool> pools;
    private Dictionary<string, ObjectPool> dictionaryPool = new Dictionary<string, ObjectPool>();

    protected override void Awake()
    {
        base.Awake();
        foreach (var pool in pools)
        {
            pool.Initialize(transform);
            dictionaryPool[pool.poolName] = pool;
        }
    }

    public void RemoveFromPool(Transform element, string poolName)
    {
        dictionaryPool[poolName].RemoveFromPool(element);
    }

    public void ReturnToPool(Transform element, string poolName)
    {
        dictionaryPool[poolName].ReturnToPool(element);
    }
    public Transform Spawn(string poolName, Vector3 position, Transform parent)
    {
        if (!dictionaryPool.TryGetValue(poolName, out var pool))
        {
            return null;
        }
        return pool.GetElement(position, parent);
    }

    public void Despawn(string poolName, Transform t)
    {
        if (!dictionaryPool.TryGetValue(poolName, out var pool))
        {
            return;
        }
        pool.Despawn(t);
    }

    private void OnDestroy()
    {
        dictionaryPool.Clear();
    }
}
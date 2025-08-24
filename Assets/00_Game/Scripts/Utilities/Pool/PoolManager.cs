using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonMono<PoolManager>
{
    [SerializeField] private List<PoolControl> poolInfors = new List<PoolControl>();
    private Dictionary<string, PoolControl> poolMapping = new Dictionary<string, PoolControl>();

    protected override void Awake()
    {
        base.Awake();
        foreach (var pool in poolInfors)
        {
            pool.Initialize(transform);                
            poolMapping[pool.PoolName] = pool;         
        }
    }
    
    public PoolableObject Spawn(string poolName, Vector3 pos, Transform parent = null)
    {
        if (poolMapping.TryGetValue(poolName, out var pool))
        {
            return pool.Spawn(pos, parent);
        }
        return null;
    }

    public void Despawn(string poolName, PoolableObject obj)
    {
        if (poolMapping.TryGetValue(poolName, out var pool))
        {
            pool.Despawn(obj);
        }
    }

    public void ReleaseToPool(string poolName, PoolableObject obj)
    {
        if (poolMapping.TryGetValue(poolName, out var poolControl))
        {
            poolControl.ReleaseToPool(obj);
        }
    }
    
}
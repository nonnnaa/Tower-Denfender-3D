using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonMono<PoolManager>
{
    public List<ObjectPool> pools;
    public static Dictionary<string, ObjectPool> dic_pool = new Dictionary<string, ObjectPool>();

    void Start()
    {
        foreach (ObjectPool pool in pools)
        {
            CreatePoolObjects(pool);
            dic_pool[pool.poolName] = pool;
        }
    }
    
    public void AddNewPool(ObjectPool pool)
    {
        if (!dic_pool.ContainsKey(pool.poolName))
        {
            CreatePoolObjects(pool);
            dic_pool[pool.poolName] = pool;
        }
    }
    private void CreatePoolObjects(ObjectPool pool)
    {
        for (int i = 0; i < pool.total; i++)
        {
            Transform trans = Instantiate(pool.prefab, Vector3.zero, Quaternion.identity);
            trans.gameObject.SetActive(false);
            pool.elements.Add(trans);
            pool.poolableCache.Add(trans.GetComponent<IPoolable>());
        }
    }
    private void OnDestroy()
    {
        dic_pool.Clear();
    }
}
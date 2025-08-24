using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class PoolControl
{
    [SerializeField] private string poolName;           
    [SerializeField] private PoolableObject prefab;     
    [SerializeField] private int total = 10;            

    private Queue<PoolableObject> poolCollection = new Queue<PoolableObject>();
    private Transform poolParent;

    public string PoolName => poolName;
    
    public void Initialize(Transform parent)
    {
        poolParent = parent;
        poolCollection.Clear();

        for (int i = 0; i < total; i++)
        {
            PoolableObject obj = Object.Instantiate(prefab, poolParent);
            obj.gameObject.SetActive(false);
            poolCollection.Enqueue(obj);
        }
    }
    public PoolableObject Spawn(Vector3 position, Transform parent = null)
    {
        PoolableObject obj;
        obj = poolCollection.Count > 0 ? poolCollection.Dequeue() : Object.Instantiate(prefab, poolParent);
        obj.OnSpawn(position, parent);
        return obj;
    }

    public void Despawn(PoolableObject obj)
    {
        if (obj == null) return;
        obj.OnDespawn();
    }

    public void ReleaseToPool(PoolableObject obj)
    {
        if (obj == null) return;
        poolCollection.Enqueue(obj);
    }
}
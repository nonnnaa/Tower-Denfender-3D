using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public interface IPoolable
{
    void OnSpawned(Vector3 position);
    void OnDespawned();
}

[Serializable]
public class ObjectPool
{
    public string poolName;
    public GameObject prefab;
    public int total;

    [NonSerialized] private List<Transform> elements = new List<Transform>();
    [NonSerialized] private List<IPoolable> poolableCache = new List<IPoolable>();
    private int index = -1;

    // Khởi tạo pool
    public void Initialize(Transform poolParent)
    {
        elements.Clear();
        poolableCache.Clear();

        for (int i = 0; i < total; i++)
        {
            GameObject obj = Object.Instantiate(prefab, poolParent);
            obj.SetActive(false);
            elements.Add(obj.transform);

            if (obj.TryGetComponent<IPoolable>(out var poolable))
                poolableCache.Add(poolable);
            else
                poolableCache.Add(null);
        }
    }

    // Lấy object từ pool
    public Transform GetElement(Vector3 position)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            index = (index + 1) % elements.Count;
            Transform t = elements[index];
            if (!t.gameObject.activeSelf)
            {
                t.position = position;
                t.gameObject.SetActive(true);

                poolableCache[index]?.OnSpawned(position);
                return t;
            }
        }

        // Nếu hết object inactive → tạo mới
        GameObject newObj = Object.Instantiate(prefab, PoolManager.Instance.transform);
        newObj.transform.position = position;
        elements.Add(newObj.transform);

        if (newObj.TryGetComponent<IPoolable>(out var newPoolable))
            poolableCache.Add(newPoolable);
        else
            poolableCache.Add(null);

        poolableCache[^1]?.OnSpawned(position); // ^1 = count - 1 => last element
        return newObj.transform;
    }

    // Trả object về pool
    public void Despawn(Transform targetTransform)
    {
        int idx = elements.IndexOf(targetTransform);
        if (idx >= 0)
        {
            targetTransform.gameObject.SetActive(false);
            poolableCache[idx]?.OnDespawned();
        }
        else
        {
            Object.Destroy(targetTransform.gameObject);
        }
    }
}

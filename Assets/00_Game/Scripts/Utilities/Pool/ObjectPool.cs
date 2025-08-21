using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

public interface IPoolable
{
    void OnSpawned(Vector3 position, [CanBeNull] Transform newParent);
    void OnDespawned();
}

[Serializable]
public class ObjectPool
{
    public string poolName;
    public GameObject prefab;
    public int total;
    private List<Transform> elements = new List<Transform>();

    // Khởi tạo pool
    public void Initialize(Transform poolParent)
    {
        elements.Clear();
        for (int i = 0; i < total; i++)
        {
            GameObject obj = Object.Instantiate(prefab, poolParent);
            obj.SetActive(false);
            elements.Add(obj.transform);
        }
    }

    public void RemoveFromPool(Transform element)
    {
        elements.Remove(element);
    }

    public void ReturnToPool(Transform element)
    {
        elements.Add(element);
    }

    // Lấy object từ pool
    public Transform GetElement(Vector3 position,  [CanBeNull] Transform parent)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            if (!elements[i].gameObject.activeInHierarchy)
            {
                elements[i].position = position;
                elements[i].gameObject.SetActive(true);
                elements[i].TryGetComponent<IPoolable>(out var poolable);
                poolable?.OnSpawned(position, parent);
                return elements[i];
            }
        }

        // Nếu hết object inactive → spawn thêm mới
        GameObject newObj = Object.Instantiate(prefab, PoolManager.Instance.transform);
        newObj.transform.position = position;
        elements.Add(newObj.transform);
        newObj.TryGetComponent<IPoolable>(out var newPoolable);
        newPoolable?.OnSpawned(position, null);
        return newObj.transform;
    }

    // Trả object về pool
    public void Despawn(Transform targetTransform)
    {
        if (targetTransform == null) return;
        targetTransform.gameObject.SetActive(false);
        targetTransform.TryGetComponent<IPoolable>(out var poolable);
        poolable?.OnDespawned();
    }
}

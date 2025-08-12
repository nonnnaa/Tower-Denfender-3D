using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    void OnSpawned();
    void OnDespawned();
}
[Serializable]
public class ObjectPool : MonoBehaviour
{
    public int total;
    public string poolName;
    public Transform prefab;
    [NonSerialized] public List<Transform> elements = new List<Transform>();
    [NonSerialized] public List<IPoolable> poolableCache = new List<IPoolable>();
    private int index;
    public ObjectPool() { }
    public ObjectPool(string poolName, int total, Transform prefab)
    {
        this.poolName = poolName;
        this.total = total;
        this.prefab = prefab;
    }
    public Transform OnSpawned()
    {
        if (elements.Count == 0) return null;

        index++;
        if (index >= elements.Count) index = 0;

        Transform trans = elements[index];
        trans.gameObject.SetActive(true);

        poolableCache[index]?.OnSpawned();
        return trans;
    }
    public void OnDespawned(Transform trans)
    {
        int idx = elements.IndexOf(trans);
        if (idx >= 0)
        {
            poolableCache[idx]?.OnDespawned();
            trans.gameObject.SetActive(false);
        }
    }
}
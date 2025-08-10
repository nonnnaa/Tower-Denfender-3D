using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolInfor
{
    public int maxCount;
    public float timeToDeactive;
    public GameObject prefab;
    public Stack<GameObject> deActiveGO;
}
public class ObjectPool : MonoBehaviour
{
    protected float timeToDeactive;
    protected string key;
    protected Transform spawnPoint;
    
    public virtual void OnInit(float timeLife, string nameKey)
    {
        timeToDeactive = timeLife;
    }

    public virtual void OnInit(float timeLife)
    {
        timeToDeactive = timeLife;
    }
    
    public virtual void OnSpawn(Vector3 newPosition, Transform parent)
    {

    }

    public virtual void OnDespawn()
    {
        
    }
}
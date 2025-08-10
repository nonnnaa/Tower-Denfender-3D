using System.Collections.Generic;
using UnityEngine;
public class ObjectPoolManager : SingletonMono<ObjectPoolManager>
{
    public List<PoolInfor> poolInfors = new List<PoolInfor>();
    private Dictionary<string, PoolInfor> poolMapping = new Dictionary<string, PoolInfor>();
    private void Start()
    {
        foreach (PoolInfor poolInfor in poolInfors)
        {
            poolMapping[poolInfor.prefab.name] = poolInfor;
            poolInfor.deActiveGO = new Stack<GameObject>();
            // spawn object when maxCount > 0 => fixed number of object
            
            if (poolInfor.maxCount > 0)
            {
                for (int i = 0; i < poolInfor.maxCount; i++)
                {
                    var go = Instantiate(poolInfor.prefab);
                    go.SetActive(false);
                    poolInfor.deActiveGO.Push(go);
                }
            }
        }
    }
    public PoolInfor GetPoolInforByName(string name)
    {
        return poolMapping[name];
    }

    public GameObject GetObjectPool(PoolInfor p)
    {
        if (p.deActiveGO.Count > 0)
        {
            var go = p.deActiveGO.Pop(); ;
            go.SetActive(true);
            return go;
        }
        return null;
    }
    public GameObject GetObjectPooled(string prefabName)
    {
        PoolInfor infor = GetPoolInforByName(prefabName);
        if (infor != null)
        {
            // handle maxCount != 0 case => unlimited number of gameObject.
            if (infor.maxCount <= 0)
            {
                // size stack > 0
                var objPool1 = GetObjectPool(infor);
                if (objPool1 != null)
                {
                    return objPool1;
                }
                else
                {
                    // size stack <= 0
                    var prefab = Instantiate(infor.prefab);
                    return prefab;
                }
            }
            else
            {
                // handle maxCount = 0 => limit number of gameObject active <= maxCount.
                var objPool2 = GetObjectPool(infor);
                if(objPool2 != null)
                    return objPool2;
            }
        }
        Debug.Log("Null Infor");
        return null;
    }
}

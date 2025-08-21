using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
    public virtual void OnSpawn(Vector3 position, Transform parent = null)
    {
        
    }
    public virtual void OnDespawn()
    {
        
    }
}
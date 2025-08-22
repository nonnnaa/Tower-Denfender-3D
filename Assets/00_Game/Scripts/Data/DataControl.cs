using System;
using UnityEngine;

public class DataControl : MonoBehaviour
{
    public static DataControl Instance { get; private set; }

    private DataModelLocal dataModel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        dataModel = new DataModelLocal();
    }
    
    public void Init(Action callback)
    {
        Instance = this;
        dataModel.CreateData(callback);
        callback?.Invoke();
    }
   
    public T Read<T>(string path)
    {
        return dataModel.Read<T>(path);
    }

    
    public T ReadDataKey<T>(string path, string key)
    {
        return dataModel.ReadDataKey<T>(path, key);
    }

    
    public void UpdateData<T>(string path, T value, Action callback = null)
    {
        dataModel.UpdateData(path, value, callback);
    }

    
    public void UpdateKey<T>(string path, T value, string key, Action callback = null)
    {
        dataModel.UpdateDataKey(path, value, key, callback);
    }
}
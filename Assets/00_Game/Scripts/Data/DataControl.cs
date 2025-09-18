using System;
using UnityEngine;



[CreateAssetMenu(menuName = "Data/DataController")]
public class DataControl : ScriptableObject
{
    public static DataControl Instance;

    [SerializeField] private DataModelLocal dataModel;
    
    public void Init(Action callback)
    {
        Instance = this;
        dataModel.CreateData(callback);
        callback?.Invoke();
    }
}
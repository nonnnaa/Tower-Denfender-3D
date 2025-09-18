using System;
using UnityEngine;
public enum UnitType
{
    NORMAL = 1,
    EPIC = 2,
    LEGENDARY = 3
}

[Serializable]
public class FileConfigUnitRecord
{
    [SerializeField] private int id;
    public int Id => id;
    
    [SerializeField] private string name;
    public string Name => name;
    
    [SerializeField] private string description;
    public string Description => description;
    
    [SerializeField] private string prefabName;
    public string PrefabName => prefabName;
    
    [SerializeField] private UnitType type;
    public UnitType Type => type;
    
    [SerializeField] private int stamina;
    public int Stamina => stamina;
    
    [SerializeField] private float coolDown;
    public float CoolDown => coolDown;
}

using System;
using UnityEngine;

public enum TurretType
{
    Lightning = 0,
    Shield = 1,
    Rocket = 2,
    Plasma = 3,
    Shotgun = 4
}
[Serializable]
public class FileConfigTurretRecord
{
    [SerializeField] private TurretType type;
    public TurretType Type => type;
    
    [SerializeField] private string name;
    public string Name => name;
    
    [SerializeField] private string description;
    public string Description => description;
    
    [SerializeField] private string prefabName;
    public string PrefabName => prefabName;
    
    [SerializeField] private float hp;
    public float Hp => hp;
    
    [SerializeField] private float buyPrice;
    public float BuyPrice => buyPrice;
    
    [SerializeField] private float sellPrice;
    public float SellPrice => sellPrice;
    
}

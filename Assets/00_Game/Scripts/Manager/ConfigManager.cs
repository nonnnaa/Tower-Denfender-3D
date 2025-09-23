using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : SingletonMono<ConfigManager>
{
    [SerializeField] private FileConfigTurret fileConfigTurret;
    [SerializeField] private FileConfigEnemy fileConfigEnemy;
    [SerializeField] private FileConfigGameLevel fileConfigGameLevel;
    
    
    private Dictionary<string, TurretControl> turretDictionary = new Dictionary<string, TurretControl>();
    
    public void Init(Action callback)
    {
        StartCoroutine(OnStart(callback));
    }
    public FileConfigEnemy GetFileConfigEnemy()
    {
        return fileConfigEnemy;
    }

    public FileConfigTurret GetFileConfigTurret()
    {
        return fileConfigTurret;
    }
    public FileConfigGameLevel GetFileConfigGameLevel() => fileConfigGameLevel;
    
    IEnumerator OnStart(Action callback)
    {
        fileConfigTurret = Resources.Load<FileConfigTurret>("Config/FileConfigTurret");
        yield return new WaitUntil(() => fileConfigTurret != null);
        
        fileConfigEnemy = Resources.Load<FileConfigEnemy>("Config/FileConfigEnemy");
        yield return new WaitUntil(() => fileConfigTurret != null);
        
        fileConfigGameLevel =  Resources.Load<FileConfigGameLevel>("Config/FileConfigGameLevel");
        yield return new WaitUntil(() => fileConfigGameLevel != null);
        
        TurretControl[] turretControls = Resources.LoadAll<TurretControl>($"Turret");
        foreach (TurretControl turretControl in turretControls)
        {
            turretDictionary[turretControl.name] = turretControl;
        }
        
        callback?.Invoke();
    }

    public TurretControl GetTurretControl(string newName)
    {
        return turretDictionary.GetValueOrDefault(newName);
    }
}

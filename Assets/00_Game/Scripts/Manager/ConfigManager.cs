using System;
using System.Collections;
using UnityEngine;

public class ConfigManager : SingletonMono<ConfigManager>
{
    [SerializeField] private FileConfigTurret fileConfigTurret;
    [SerializeField] private FileConfigEnemy fileConfigEnemy;
    [SerializeField] private FileConfigGameLevel fileConfigGameLevel;
    
    public void Init(Action callback)
    {
        StartCoroutine(OnStart(callback));
    }
    public FileConfigEnemy GetFileConfigEnemy()
    {
        return fileConfigEnemy;
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
        callback?.Invoke();
    }
}

using System;
using System.Collections;
using UnityEngine;

public class ConfigManager : SingletonMono<ConfigManager>
{
    private FileConfigTurret fileConfigTurret;
    
    public void Init(Action callback)
    {
        StartCoroutine(OnStart(callback));
    }
    
    IEnumerator OnStart(Action callback)
    {
        fileConfigTurret = Resources.Load<FileConfigTurret>("Config/FileConfigTurret");
        yield return new WaitUntil(() => fileConfigTurret != null);
        
        
        if (callback != null)
        {
            callback();
        }
    }


}

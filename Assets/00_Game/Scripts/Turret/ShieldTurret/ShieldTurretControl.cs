using System.Collections;
using UnityEngine;
using CONSTANT;
public class ShieldTurretControl : TurretControl
{
    [Header("Turret Data")]
    [SerializeField] private float timeToBuffHp;
    [SerializeField] private float timeToBuffDef; // should be plus time of vfx play
    [SerializeField] private float hpBuff;
    [SerializeField] private float defBuff;


    #region Temp
    private Coroutine hpBuffCoroutine, defBuffCoroutine;
    private bool isHpCoroutineRunning, isDefCoroutineRunning;

    #endregion
    
    
    private void Start()
    {
        isHpCoroutineRunning = true;
        isDefCoroutineRunning = true;
        OnActivateEffect();
    }

    private void OnActivateEffect() // run each time when turret active in scene
    {
        if (hpBuffCoroutine != null)
        {
            StopCoroutine(hpBuffCoroutine);
        }
        hpBuffCoroutine = StartCoroutine(HpBuffCoroutine());

        if (defBuffCoroutine != null)
        {
            StopCoroutine(defBuffCoroutine);
        }
        defBuffCoroutine = StartCoroutine(DefBuffCoroutine());
    }

    IEnumerator HpBuffCoroutine()
    {
        while (isHpCoroutineRunning)
        {
            // Handle buff
            yield return new WaitForSeconds(timeToBuffHp);
        }
    }

    private void BuffDef()
    {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag($"Turret");
        foreach (GameObject turret in turrets)
        {
            PoolManager.Instance.Spawn(BuffVfxName.TurretBuffDef, turret.transform.position 
                                                                  + new Vector3(0, 1.5f, 0f), PoolManager.Instance.transform);
        }
    }
    
    IEnumerator DefBuffCoroutine()
    {
        while (isDefCoroutineRunning)
        {
            // handle buff
            BuffDef();
            yield return new WaitForSeconds(timeToBuffDef);
        }
    }
}
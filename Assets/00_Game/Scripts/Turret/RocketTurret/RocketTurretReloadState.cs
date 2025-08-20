using System.Collections;
using UnityEngine;

public class RocketTurretReloadState : FSMState
{
    private RocketTurretControl turretControl;
    private Coroutine reloadRoutine;
    private int bulletCount;
    public RocketTurretReloadState(RocketTurretControl turretControl)
    {
        this.turretControl = turretControl;
    }
    public override void EnterState()
    {
        bulletCount = 0;
        if (reloadRoutine != null)
        {
            turretControl.StopCoroutine(FillRockets());
        }
        reloadRoutine = turretControl.StartCoroutine(FillRockets());
    }
    IEnumerator FillRockets()
    {
        while (bulletCount <= 8)
        {
            yield return new WaitForSeconds(turretControl.timeToReload);
            turretControl.FillRocket();
            bulletCount++;
        }
        turretControl.ChangeState(turretControl.idleState);
        bulletCount = 0;
    }
    
    public override void ExitState()
    {
        if (reloadRoutine != null)
        {
            turretControl.StopCoroutine(FillRockets());
        }
    }
}

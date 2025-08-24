using UnityEngine;

public class ShotGunTurretIdleState : FSMState
{
    private readonly ShotGunTurretControl turretControl;

    public ShotGunTurretIdleState(ShotGunTurretControl turretControl)
    {
        this.turretControl = turretControl;
    }

    public override void EnterState()
    {
        turretControl.SetTarget(null);
    }

    public override void UpdateState()
    {
        CheckEnemy();
    }

    private void CheckEnemy()
    {
        Transform enemy = FindNearestEnemy();
        if (enemy != null)
        {
            float dist = Vector3.Distance(turretControl.gameObject.transform.position, enemy.position);
            Debug.Log(dist);
            if (dist >= turretControl.GetMinAttackRange() && dist <= turretControl.GetMaxAttackRange())
            {
                turretControl.SetTarget(enemy);
                turretControl.ChangeState(turretControl.attackState);
                return;
            }
        }
        turretControl.ResetRotation();
    }

    private Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject e in enemies)
        {
            float dist = Vector3.Distance(turretControl.GetFirePoint().position, e.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = e.transform;
            }
        }
        return nearest;
    }
}
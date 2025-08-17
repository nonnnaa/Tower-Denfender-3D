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
        turretControl.target = null;
    }

    public override void UpdateState()
    {
        Transform enemy = FindNearestEnemy();
        if (enemy != null)
        {
            float dist = Vector3.Distance(turretControl.gameObject.transform.position, enemy.position);

            // trong khoảng min - max attack range thì mới chuyển sang Attack
            if (dist >= turretControl.minAttackRange && dist <= turretControl.attackRange)
            {
                turretControl.target = enemy;
                turretControl.attackState.SetTarget(enemy);
                turretControl.ChangeState(turretControl.attackState);
                return;
            }
        }
        // không có enemy → xoay về mặc định
        turretControl.ResetRotation();
    }

    private Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject e in enemies)
        {
            float dist = Vector3.Distance(turretControl.firePoint.position, e.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = e.transform;
            }
        }

        return nearest;
    }
}
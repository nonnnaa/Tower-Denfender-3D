using UnityEngine;

public class DisruptorTurretIdleState : FSMState
{
    private readonly DisruptorTurretControl turretControl;

    public DisruptorTurretIdleState(DisruptorTurretControl turretControl)
    {
        this.turretControl = turretControl;
    }

    public override void EnterState()
    {
        turretControl.SetTarget(null);
        //turretControl.(true);
    }

    public override void UpdateState()
    {
        Transform enemy = FindNearestEnemy();
        if (enemy != null)
        {
            float dist = Vector3.Distance(turretControl.transform.position, enemy.position);

            if (dist >= turretControl.GetMinAttackRange() && dist <= turretControl.GetMaxAttackRange())
            {
                turretControl.SetTarget(enemy);
                turretControl.ChangeState(turretControl.attackState);
                return;
            }
        }

        //turretControl.ResetRotation();
    }

    private Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject e in enemies)
        {
            float dist = Vector3.Distance(turretControl.transform.position, e.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = e.transform;
            }
        }

        return nearest;
    }
}

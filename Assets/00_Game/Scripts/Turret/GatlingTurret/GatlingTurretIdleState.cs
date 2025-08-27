using UnityEngine;

public class GatlingTurretIdleState : FSMState
{
    private readonly GatlingTurretControl turretControl;

    public GatlingTurretIdleState(GatlingTurretControl turretControl)
    {
        this.turretControl = turretControl;
    }

    public override void EnterState()
    {
        turretControl.SetTarget(null);
        turretControl.StopEffects(); // đổi lại StopEffects()
    }


    public override void UpdateState()
    {
        Transform enemy = FindNearestEnemy();
        if (enemy != null)
        {
            float dist = Vector3.Distance(turretControl.transform.position, enemy.position);

            // Nếu enemy trong khoảng bắn thì chuyển Attack
            if (dist >= turretControl.GetMinAttackRange() && dist <= turretControl.GetMaxAttackRange())
            {
                turretControl.SetTarget(enemy);
                turretControl.ChangeState(turretControl.attackState);
                return;
            }
        }

        // Không có enemy → reset về góc mặc định
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

using UnityEngine;

public class FlamerTurretAttackState : FSMState
{
    private readonly FlamerTurretControl turretControl;

    public FlamerTurretAttackState(FlamerTurretControl turretControl)
    {
        this.turretControl = turretControl;
    }

    public override void EnterState()
    {
        turretControl.StartFlame();
    }

    public override void UpdateState()
    {
        Transform target = turretControl.GetTarget();
        if (target == null)
        {
            turretControl.ChangeState(turretControl.idleState);
            return;
        }

        RotateTurret(target);
        UpdateFlameDirection(target);

        turretControl.PlaySmoke(target);

        float dist = Vector3.Distance(turretControl.transform.position, target.position);
        if (dist < turretControl.GetMinAttackRange() || dist > turretControl.GetMaxAttackRange())
        {
            turretControl.StopFlame();
            turretControl.StopSmoke(target);
            turretControl.ChangeState(turretControl.idleState);
        }
    }

    public override void ExitState()
    {
        turretControl.StopFlame();
        turretControl.StopSmoke(turretControl.GetTarget());
    }

    private void RotateTurret(Transform target)
    {
        Transform baseY = turretControl.GetTurretBaseY();
        Transform headX = turretControl.GetTurretHeadX();

        Vector3 dir = (target.position - baseY.position).normalized;
        dir.y = 0; // chỉ xoay ngang
        if (dir != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            baseY.rotation = Quaternion.Lerp(baseY.rotation, lookRot, Time.deltaTime * turretControl.GetRotationSpeed());
        }

        Vector3 dirHead = (target.position - headX.position).normalized;
        if (dirHead != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(dirHead);
            headX.rotation = Quaternion.Lerp(headX.rotation, lookRot, Time.deltaTime * turretControl.GetRotationSpeed());
        }
    }

    private void UpdateFlameDirection(Transform target)
    {
        Transform firePoint = turretControl.GetFlameFirePoint();
        if (firePoint == null) return;

        Vector3 dir = (target.position - firePoint.position).normalized;
        if (dir != Vector3.zero)
            firePoint.rotation = Quaternion.LookRotation(dir);
    }
}

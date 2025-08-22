using UnityEngine;
using System.Collections;
public class LightningTurretAttackState : FSMState
{
    private readonly LightningTurretControl turretControl;
    private Transform target;
    private Coroutine attackCoroutine;
    public LightningTurretAttackState(LightningTurretControl turretControl)
    {
        this.turretControl = turretControl;
    }
    public override void EnterState()
    {
        target = turretControl.GetTarget();
        if (attackCoroutine != null)
        {
            turretControl.StopCoroutine(attackCoroutine);
        }
        attackCoroutine = turretControl.StartCoroutine(AttackRoutine());
    }
    public override void UpdateState()
    {
        if (target != null)
        {
            base.UpdateState();
            RotateUntilAimed(target);
        }
    }

    public override void ExitState()
    {
        if (attackCoroutine != null)
        {
            turretControl.StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
        turretControl.GetLightningControl().enabled = false;
        turretControl.StopParticleSystem(true);
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (target == null)
            {
                turretControl.ChangeState(turretControl.idleState);
                turretControl.GetLightningControl().enabled = false;
                turretControl.StopParticleSystem(true);
                yield break;
            }

            float dist = Vector3.Distance(turretControl.GetFirePoint().position, target.position);
            if (dist <= turretControl.GetMinAttackRange() || dist >= turretControl.GetMaxAttackRange())
            {
                turretControl.ChangeState(turretControl.idleState);
                turretControl.GetLightningControl().enabled = false;
                turretControl.StopParticleSystem(true);
                yield break;
            }
            Fire();
            yield return new WaitForSeconds(turretControl.GetTimeAttack());
            
            turretControl.GetLightningControl().enabled = false;
            turretControl.StopParticleSystem(true);
            
            yield return new WaitForSeconds(turretControl.GetFireInterval());
        }
    }

    private void RotateUntilAimed(Transform newTarget)
    {
            Vector3 dir = newTarget.position - turretControl.GetFirePoint().position;
            // Rotate Y
            Vector3 flatDir = new Vector3(dir.x, 0f, dir.z);
            if (flatDir.sqrMagnitude > 0.01f)
            {
                Quaternion yRot = Quaternion.LookRotation(flatDir);
                turretControl.GetTurretBaseY().rotation = Quaternion.Lerp(
                    turretControl.GetTurretBaseY().rotation,
                    yRot,
                    Time.deltaTime * turretControl.GetRotationSpeed()
                );
            }
            // Rotate X
            Vector3 localDir = turretControl.GetTurretBaseY().InverseTransformDirection(dir);
            float angleX = Mathf.Atan2(localDir.y, localDir.z) * Mathf.Rad2Deg;

            Quaternion xRot = Quaternion.Euler(-angleX, 0f, 0f);
            turretControl.GetTurretHeadX().localRotation = Quaternion.Lerp(
                turretControl.GetTurretHeadX().localRotation,
                xRot,
                Time.deltaTime * turretControl.GetRotationSpeed()
            );
    }
    private void Fire()
    {
        turretControl.GetLightningControl().enabled = true;
        turretControl.GetLightningControl().SetTarget(target);
        turretControl.StopParticleSystem(false);
    }
}

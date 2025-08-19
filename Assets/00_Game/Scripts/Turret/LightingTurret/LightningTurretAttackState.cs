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
        attackCoroutine = turretControl.StartCoroutine(AttackRoutine());
    }


    public override void UpdateState()
    {
        if (target != null)
        {
            base.UpdateState();
            // --- xoay turret cho tới khi thẳng hướng ---
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

            float dist = Vector3.Distance(turretControl.firePoint.position, target.position);

            if (dist <= turretControl.minAttackRange || dist >= turretControl.attackRange)
            {
                turretControl.ChangeState(turretControl.idleState);
                turretControl.GetLightningControl().enabled = false;
                turretControl.StopParticleSystem(true);
                yield break;
            }

            // --- bắn ---
            Fire();
            yield return new WaitForSeconds(turretControl.timeAttack);
            
            // Tắt tia lightning trong timeAttack
            turretControl.GetLightningControl().enabled = false;
            turretControl.StopParticleSystem(true);
            // chờ interval trước khi lặp lại
            yield return new WaitForSeconds(turretControl.fireInterval);
        }
    }

    private void RotateUntilAimed(Transform newTarget)
    {
            Vector3 dir = newTarget.position - turretControl.firePoint.position;
            // xoay theo Y
            Vector3 flatDir = new Vector3(dir.x, 0f, dir.z);
            if (flatDir.sqrMagnitude > 0.01f)
            {
                Quaternion yRot = Quaternion.LookRotation(flatDir);
                turretControl.turretBaseY.rotation = Quaternion.Lerp(
                    turretControl.turretBaseY.rotation,
                    yRot,
                    Time.deltaTime * turretControl.rotationSpeed
                );
            }
            // xoay theo X
            Vector3 localDir = turretControl.turretBaseY.InverseTransformDirection(dir);
            float angleX = Mathf.Atan2(localDir.y, localDir.z) * Mathf.Rad2Deg;

            Quaternion xRot = Quaternion.Euler(-angleX, 0f, 0f);
            turretControl.turretHeadX.localRotation = Quaternion.Lerp(
                turretControl.turretHeadX.localRotation,
                xRot,
                Time.deltaTime * turretControl.rotationSpeed
            );
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    private void Fire()
    {
        turretControl.GetLightningControl().enabled = true;
        turretControl.GetLightningControl().SetTarget(target);
        turretControl.StopParticleSystem(false);
    }
}

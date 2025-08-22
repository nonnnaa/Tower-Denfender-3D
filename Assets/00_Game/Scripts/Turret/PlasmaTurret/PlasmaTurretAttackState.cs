using UnityEngine;
using System.Collections;
using CONSTANT;
public class PlasmaTurretAttackState : FSMState
{
    private readonly PlasmaTurretControl turretControl;
    private Transform target;
    private Coroutine attackCoroutine;
    public PlasmaTurretAttackState(PlasmaTurretControl turretControl)
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

    public override void ExitState()
    {
        if (attackCoroutine != null)
        {
            turretControl.StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (target == null)
            {
                turretControl.ChangeState(turretControl.idleState);
                yield break;
            }

            float dist = Vector3.Distance(turretControl.GetFirePoint().position, target.position);

            if (dist <= turretControl.GetMinAttackRange() || dist >= turretControl.GetMaxAttackRange())
            {
                turretControl.ChangeState(turretControl.idleState);
                yield break;
            }
            yield return RotateUntilAimed(target);
            Fire();
            yield return new WaitForSeconds(turretControl.GetFireInterval());
        }
    }

    private IEnumerator RotateUntilAimed(Transform newTarget)
    {
        while (true)
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
            float angleToTarget = Vector3.Angle(turretControl.GetFirePoint().forward, dir);
            if (angleToTarget < 2f) 
                break;
            yield return null; 
        }
    }

    private void Fire()
    {
        if (target == null) return;
        PoolableObject bulletObj = PoolManager.Instance.Spawn(
            ProjectileName.ProjectilePlasmaTurret,
            turretControl.GetFirePoint().position,
            PoolManager.Instance.transform
        );
        bulletObj.GetComponent<PlasmaBullet>().SetCurrentParent(turretControl.GetFirePoint());
        if (bulletObj == null) return;
        IBullet bullet = bulletObj.GetComponent<IBullet>();
        if (bullet != null)
        {
            Vector3 direction = (target.position - turretControl.GetFirePoint().position).normalized;
            bullet.Shoot(direction);
        }
    }
}

using UnityEngine;
using System.Collections;

public class ShotGunTurretAttackState : FSMState
{
    private readonly ShotGunTurretControl turretControl;
    private Transform target;
    private Coroutine attackCoroutine;

    public ShotGunTurretAttackState(ShotGunTurretControl turretControl)
    {
        this.turretControl = turretControl;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public override void EnterState()
    {
        // start attack coroutine
        attackCoroutine = turretControl.StartCoroutine(AttackRoutine());
    }

    public override void ExitState()
    {
        // stop when leaving state
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

            float dist = Vector3.Distance(turretControl.firePoint.position, target.position);

            if (dist <= turretControl.minAttackRange || dist >= turretControl.attackRange)
            {
                turretControl.ChangeState(turretControl.idleState);
                yield break;
            }

            // --- xoay turret cho tới khi thẳng hướng ---
            yield return RotateUntilAimed(target);

            // --- bắn ---
            Fire();

            // chờ interval trước khi lặp lại
            yield return new WaitForSeconds(turretControl.fireInterval);
        }
    }

    private IEnumerator RotateUntilAimed(Transform target)
    {
        while (true)
        {
            Vector3 dir = target.position - turretControl.firePoint.position;

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

            // check nếu đã aim thẳng (góc lệch nhỏ hơn ngưỡng)
            float angleToTarget = Vector3.Angle(turretControl.firePoint.forward, dir);
            if (angleToTarget < 2f) // bạn có thể chỉnh ngưỡng 2°
                break;
            yield return null; // chờ frame sau
        }
    }

    private void Fire()
    {
        if (target == null) return;

        // Spawn bullet từ pool (position mặc định là firePoint)
        Transform bulletObj = PoolManager.Instance.Spawn(
            "BulletPool",
            turretControl.firePoint.position
        );

        if (bulletObj == null) return;

        // Lấy component Bullet và thiết lập hướng bay
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            Vector3 direction = (target.position - turretControl.firePoint.position).normalized;
            bullet.Shoot(direction);
        }
    }
}

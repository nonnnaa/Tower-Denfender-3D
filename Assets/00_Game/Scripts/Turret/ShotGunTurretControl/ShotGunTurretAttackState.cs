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
        if (!PoolManager.dic_pool.TryGetValue("Bullet", out var pool))
        {
            Debug.LogWarning("Pool 'Bullet' not found!");
            return;
        }
        Transform bulletObj = null;
        foreach (var item in pool.elements)
        {
            if (!item.gameObject.activeInHierarchy)
            {
                bulletObj = item;
                break;
            }
        }

        if (bulletObj == null)
        {
            bulletObj = Object.Instantiate(pool.prefab, turretControl.firePoint.position, turretControl.firePoint.rotation);
            pool.elements.Add(bulletObj);
            pool.poolableCache.Add(bulletObj.GetComponent<IPoolable>());
        }
        bulletObj.position = turretControl.firePoint.position;
        bulletObj.rotation = turretControl.firePoint.rotation;
        bulletObj.gameObject.SetActive(true);

        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetTarget(target);
        }
    }
}

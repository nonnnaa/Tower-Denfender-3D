using UnityEngine;
using System.Collections;
using CONSTANT;

public class RocketTurretAttackState : FSMState
{
    private readonly RocketTurretControl turretControl;
    private Transform target;
    private Coroutine attackCoroutine;

    public RocketTurretAttackState(RocketTurretControl turretControl)
    {
        this.turretControl = turretControl;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public override void EnterState()
    {
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

private IEnumerator RotateUntilAimed(Transform newTarget)
{
    while (true)
    {
        if (newTarget == null) yield break; 

        Vector3 targetPos = newTarget.position;
        Vector3 startPos = turretControl.firePoint.position;

        Vector3 dir = targetPos - startPos;
        float dist = Vector3.Distance(startPos, targetPos);

        // --- 1. Xoay theo Y (ngang)
        Vector3 flatDir = new Vector3(dir.x, 0f, dir.z);
        Quaternion targetYRotation = turretControl.turretBaseY.rotation; // Mặc định là rotation hiện tại
        if (flatDir.sqrMagnitude > 0.01f)
        {
            targetYRotation = Quaternion.LookRotation(flatDir);
            turretControl.turretBaseY.rotation = Quaternion.Slerp( // Slerp cho chuyển động mượt hơn
                turretControl.turretBaseY.rotation,
                targetYRotation,
                Time.deltaTime * turretControl.rotationSpeed
            );
        }

        // --- 2. Tính toán góc bắn theo X (dọc) với độ cong ---
        // a. Tính góc bắn thẳng tới mục tiêu (base pitch)
        Vector3 localDir = turretControl.turretBaseY.InverseTransformDirection(dir);
        float basePitch = Mathf.Atan2(localDir.y, localDir.z) * Mathf.Rad2Deg;

        // b. Tính góc cộng thêm (bonus pitch) dựa vào khoảng cách
        // Nội suy tuyến tính để tìm ra góc bonus phù hợp
        float t = Mathf.InverseLerp(turretControl.minAttackRange, turretControl.attackRange, dist);
        float bonusPitch = Mathf.Lerp(turretControl.arcAngleAtMinRange, turretControl.arcAngleAtMaxRange, t);

        // c. Góc cuối cùng = góc cơ bản + góc bonus
        float finalPitch = basePitch + bonusPitch;
        
        // d. Giới hạn góc bắn trong khoảng cho phép
        finalPitch = Mathf.Clamp(finalPitch, turretControl.minPitch, turretControl.maxPitch);
        
        // e. Áp dụng góc quay
        Quaternion targetXRotation = Quaternion.Euler(-finalPitch, 0f, 0f);
        turretControl.turretHeadX.localRotation = Quaternion.Slerp( 
            turretControl.turretHeadX.localRotation,
            targetXRotation,
            Time.deltaTime * turretControl.rotationSpeed
        );

        // --- 3. Kiểm tra đã ngắm thẳng chưa (dựa trên góc thay vì hướng vector) ---
        // Chúng ta so sánh góc hiện tại và góc mục tiêu của cả 2 trục
        float angleYDiff = Quaternion.Angle(turretControl.turretBaseY.rotation, targetYRotation);
        float angleXDiff = Quaternion.Angle(turretControl.turretHeadX.localRotation, targetXRotation);
        
        if (angleYDiff < 1f && angleXDiff < 1f) // Ngưỡng chấp nhận là 1 độ
        {
            break;
        }
        yield return null;
    }
}



    private void Fire()
    {
        if (target == null) return;
        Transform[] transforms = turretControl.GetRocketTransforms();
        for (int i = 0; i < transforms.Length; i++)
        {
            if (transforms[i] != null)
            {
                var bullet = transforms[i].GetComponent<RocketBullet>();
                if (bullet != null)
                {
                    bullet.SetTarget(target);
                    PoolManager.Instance.Spawn(MuzzleFlareName.MuzzleFlareShortGunTurret, transforms[i].position, transforms[i]);
                    transforms[i] = null;
                    return;
                }
            }
        }
        turretControl.ChangeState(turretControl.reloadState);
    }
}

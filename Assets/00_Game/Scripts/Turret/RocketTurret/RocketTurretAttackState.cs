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
            if (newTarget == null) yield break; 

            Vector3 targetPos = newTarget.position;
            Vector3 startPos = turretControl.GetFirePoint().position;

            Vector3 dir = targetPos - startPos;
            float dist = Vector3.Distance(startPos, targetPos);

            // --- 1. Xoay theo Y (ngang)
            Vector3 flatDir = new Vector3(dir.x, 0f, dir.z);
            Quaternion targetYRotation = turretControl.GetTurretBaseY().rotation; // Mặc định là rotation hiện tại
            if (flatDir.sqrMagnitude > 0.01f)
            {
                targetYRotation = Quaternion.LookRotation(flatDir);
                turretControl.GetTurretBaseY().rotation = Quaternion.Slerp( // Slerp cho chuyển động mượt hơn
                    turretControl.GetTurretBaseY().rotation,
                    targetYRotation,
                    Time.deltaTime * turretControl.GetRotationSpeed()
                );
            }

            // --- 2. Tính toán góc bắn theo X (dọc) với độ cong ---
            // a. Tính góc bắn thẳng tới mục tiêu (base pitch)
            Vector3 localDir = turretControl.GetTurretBaseY().InverseTransformDirection(dir);
            float basePitch = Mathf.Atan2(localDir.y, localDir.z) * Mathf.Rad2Deg;

            // b. Tính góc cộng thêm (bonus pitch) dựa vào khoảng cách
            // Nội suy tuyến tính để tìm ra góc bonus phù hợp
            float t = Mathf.InverseLerp(turretControl.GetMinAttackRange(), turretControl.GetMaxAttackRange(), dist);
            float bonusPitch = Mathf.Lerp(turretControl.GetArcAngleAtMinRange(), turretControl.GetArcAngleAtMaxRange(), t);

            // c. Góc cuối cùng = góc cơ bản + góc bonus
            float finalPitch = basePitch + bonusPitch;
            
            // d. Giới hạn góc bắn trong khoảng cho phép
            finalPitch = Mathf.Clamp(finalPitch, turretControl.GetMinPitch(), turretControl.GetMaxPitch());
            
            // e. Áp dụng góc quay
            Quaternion targetXRotation = Quaternion.Euler(-finalPitch, 0f, 0f);
            turretControl.GetTurretHeadX().localRotation = Quaternion.Slerp( 
                turretControl.GetTurretHeadX().localRotation,
                targetXRotation,
                Time.deltaTime * turretControl.GetRotationSpeed()
            );

            // --- 3. Kiểm tra đã ngắm thẳng chưa (dựa trên góc thay vì hướng vector) ---
            // Chúng ta so sánh góc hiện tại và góc mục tiêu của cả 2 trục
            float angleYDiff = Quaternion.Angle(turretControl.GetTurretBaseY().rotation, targetYRotation);
            float angleXDiff = Quaternion.Angle(turretControl.GetTurretHeadX().localRotation, targetXRotation);
            
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
        PoolableObject[] rockets = turretControl.GetRockets();
        for (int i = 0; i < rockets.Length; i++)
        {
            if (rockets[i] != null)
            {
                var bullet = rockets[i].GetComponent<RocketBullet>();
                if (bullet != null)
                {
                    bullet.SetTarget(target);
                    PoolManager.Instance.Spawn(MuzzleFlareName.MuzzleFlareShortGunTurret, rockets[i].gameObject.transform.position, rockets[i].gameObject.transform);
                    rockets[i] = null;
                    return;
                }
            }
        }
        turretControl.ChangeState(turretControl.reloadState);
    }
}

using UnityEngine;

public class GatlingTurretAttackState : FSMState
{
    private readonly GatlingTurretControl turretControl;
    private float fireTimer;

    public GatlingTurretAttackState(GatlingTurretControl turretControl)
    {
        this.turretControl = turretControl;
    }

    public override void EnterState()
    {
        fireTimer = 0f;
        turretControl.PlayEffects(); 
    }

    public override void UpdateState()
    {
        Transform target = turretControl.GetTarget();

        if (target == null)
        {
            turretControl.ChangeState(turretControl.idleState);
            return;
        }

        // Kiểm tra khoảng cách
        float dist = Vector3.Distance(turretControl.transform.position, target.position);
        if (dist < turretControl.GetMinAttackRange() || dist > turretControl.GetMaxAttackRange())
        {
            turretControl.SetTarget(null);
            turretControl.ChangeState(turretControl.idleState);
            return;
        }

        // Xoay turret theo target
        Vector3 dir = target.position - turretControl.GetFirePoint().position;
        Quaternion lookRot = Quaternion.LookRotation(dir);

        // Quay base Y
        turretControl.GetTurretBaseY().rotation = Quaternion.Lerp(
            turretControl.GetTurretBaseY().rotation,
            Quaternion.Euler(0, lookRot.eulerAngles.y, 0),
            Time.deltaTime * turretControl.GetRotationSpeed()
        );

        // Quay head X
        turretControl.GetTurretHeadX().localRotation = Quaternion.Lerp(
            turretControl.GetTurretHeadX().localRotation,
            Quaternion.Euler(lookRot.eulerAngles.x, 0, 0),
            Time.deltaTime * turretControl.GetRotationSpeed()
        );

        // Timer bắn đạn
        fireTimer += Time.deltaTime;
        if (fireTimer >= turretControl.GetFireInterval())
        {
            fireTimer = 0f;
            Fire();
        }
    }

    public override void ExitState()
    {
        turretControl.StopEffects(); 
    }

    private void Fire()
    {
        Transform firePoint = turretControl.GetFirePoint();

        PoolableObject bulletObj = PoolManager.Instance.Spawn(
            "ProjectileGatlingBullet",
            firePoint.position,
            null
        );

        if (bulletObj != null)
        {
            // Dùng Shoot thay cho Rigidbody velocity
            GatlingBullet bullet = bulletObj.GetComponent<GatlingBullet>();
            if (bullet != null)
            {
                bullet.Shoot(firePoint.forward);
            }
        }
        turretControl.PlayMuzzleFlash();
        turretControl.RotateGatlingBarrel();
    }
}

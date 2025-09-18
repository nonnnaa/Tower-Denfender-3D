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
        if (dist > turretControl.GetMaxAttackRange())
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

        // Spawn bullet từ PoolManager
        PoolableObject bulletObj = PoolManager.Instance.Spawn(
            CONSTANT.ProjectileName.ProjectileGatlingBullet,
            firePoint.position,
            null // parent, để mặc định PoolManager transform
        );

        if (bulletObj != null)
        {
            // Bảo đảm bullet active
            bulletObj.gameObject.SetActive(true);

            // Lấy component GatlingBullet
            GatlingBullet bullet = bulletObj.GetComponent<GatlingBullet>();
            if (bullet != null)
            {
                // Bắn theo hướng firePoint.forward
                bullet.Shoot(firePoint.forward);
            }
        }

        // Hiệu ứng muzzle flash
        turretControl.PlayMuzzleFlash();

        // Xoay Gatling barrel
        turretControl.RotateGatlingBarrel();
    }

}

using UnityEngine;

public class ShotGunTurretControl : FSMSystem
{
    [Header("Turret Settings")]
    public Transform turretBaseY;        // trục xoay Y
    public Transform turretHeadX;        // trục xoay X
    public Transform firePoint;          // điểm bắn
    public float rotationSpeed = 5f;
    public float fireInterval = 0.5f;
    public float attackRange = 15f;
    public float minAttackRange = 3f;

    [HideInInspector] public Transform target;

    private Quaternion defaultBaseYRot;
    private Quaternion defaultHeadXRot;

    // giữ reference các state
    public ShotGunTurretIdleState idleState;
    public ShotGunTurretAttackState attackState;

    private void Awake()
    {
        // lưu góc mặc định
        defaultBaseYRot = turretBaseY.rotation;
        defaultHeadXRot = turretHeadX.localRotation;

        // tạo state 1 lần
        idleState = new ShotGunTurretIdleState(this);
        attackState = new ShotGunTurretAttackState(this);

        
    }

    private void Start()
    {
        // set state ban đầu
        ChangeState(idleState);
    }

    public void ResetRotation()
    {
        turretBaseY.rotation = Quaternion.Lerp(
            turretBaseY.rotation, defaultBaseYRot, Time.deltaTime * rotationSpeed);

        turretHeadX.localRotation = Quaternion.Lerp(
            turretHeadX.localRotation, defaultHeadXRot, Time.deltaTime * rotationSpeed);
    }
}
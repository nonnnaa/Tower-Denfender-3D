using UnityEngine;

public class LightningTurretControl : FSMSystem
{
    [SerializeField] private LightningControl lightningControl;
    [SerializeField] private ParticleSystem impactParticleSystem, muzzleFlareParticleSystem;
    public LightningControl GetLightningControl() => lightningControl;

    public void StopParticleSystem(bool isStop)
    {
        if (!isStop)
        {
            impactParticleSystem.Play();
            muzzleFlareParticleSystem.Play();
        }
        else
        {
            impactParticleSystem.Stop();
            muzzleFlareParticleSystem.Stop();
        }
    } 
    [Header("Turret Settings")]
    public Transform turretBaseY;        // trục xoay Y
    public Transform turretHeadX;        // trục xoay X
    public Transform firePoint;          // điểm bắn
    public float rotationSpeed = 5f;
    public float fireInterval = 0.5f;
    public float attackRange = 15f;
    public float minAttackRange = 3f;
    public float timeAttack;

    [HideInInspector] public Transform target;

    private Quaternion defaultBaseYRot;
    private Quaternion defaultHeadXRot;

    // giữ reference các state
    public LightningTurretIdleState idleState;
    public LightningTurretAttackState attackState;

    private void Awake()
    {
        // lưu góc mặc định
        defaultBaseYRot = turretBaseY.rotation;
        defaultHeadXRot = turretHeadX.localRotation;
        // tạo state 1 lần
        idleState = new LightningTurretIdleState(this);
        attackState = new LightningTurretAttackState(this);
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
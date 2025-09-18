using UnityEngine;

public class GatlingTurretControl : TurretControl
{
    [Header("Turret Data")]
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float fireInterval = 0.1f;   // Gatling bắn nhanh
    [SerializeField] private float maxAttackRange = 40f;
    [SerializeField] private float minAttackRange = 2f;

    [Header("Turret Settings")]
    [SerializeField] private Transform turretBaseY;
    [SerializeField] private Transform turretHeadX;
    [SerializeField] private Transform firePoint;

    [Header("Effects")]
    [SerializeField] private ParticleSystem muzzleFlashPS;
    [SerializeField] private Transform GatlingBarrel;
    #region Temp
    private Transform target;
    private Quaternion defaultBaseYRot;
    private Quaternion defaultHeadXRot;
    #endregion

    #region Turret State
    public GatlingTurretIdleState idleState;
    public GatlingTurretAttackState attackState;
    #endregion

    #region Get Set
    public Transform GetTurretBaseY() => turretBaseY;
    public Transform GetTurretHeadX() => turretHeadX;
    public Transform GetFirePoint() => firePoint;
    public float GetRotationSpeed() => rotationSpeed;
    public float GetFireInterval() => fireInterval;
    public float GetMaxAttackRange() => maxAttackRange;

    public Transform GetTarget() => target;
    public void SetTarget(Transform newTarget) => target = newTarget;
    #endregion

    #region Unity Functions
    private void Awake()
    {
        defaultBaseYRot = turretBaseY.rotation;
        defaultHeadXRot = turretHeadX.localRotation;

        idleState = new GatlingTurretIdleState(this);
        attackState = new GatlingTurretAttackState(this);
    }

    private void Start()
    {
        ChangeState(idleState);
    }
    #endregion

    #region Turret Logic
    public void ResetRotation()
    {
        turretBaseY.rotation = Quaternion.Lerp(
            turretBaseY.rotation, defaultBaseYRot, Time.deltaTime * rotationSpeed);
        turretHeadX.localRotation = Quaternion.Lerp(
            turretHeadX.localRotation, defaultHeadXRot, Time.deltaTime * rotationSpeed);
    }

    public void PlayEffects()
    {
        if (muzzleFlashPS != null && !muzzleFlashPS.isPlaying)
            muzzleFlashPS.Play();


    }

    public void StopEffects()
    {
        if (muzzleFlashPS != null && muzzleFlashPS.isPlaying)
            muzzleFlashPS.Stop();


    }
    public void PlayMuzzleFlash()
    {
        if (muzzleFlashPS != null)
        {
            muzzleFlashPS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            muzzleFlashPS.Play();
        }
    }

    public void RotateGatlingBarrel()
    {
        if (GatlingBarrel != null)
        {
            GatlingBarrel.Rotate(Vector3.forward, 5000f * Time.deltaTime);
        }
    }
    #endregion
}

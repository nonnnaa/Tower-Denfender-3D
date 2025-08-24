using UnityEngine;

public class ShotGunTurretControl : TurretControl
{
    [Header("Turret Data")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float fireInterval = 1f;
    [SerializeField] private float maxAttackRange = 15f;
    [SerializeField] private float minAttackRange = 4f;
    
    [Header("Turret Setting")]
    [SerializeField] private Transform turretBaseY;        
    [SerializeField] private Transform turretHeadX;       
    [SerializeField] private Transform firePoint;

    #region Turret State
    public ShotGunTurretIdleState idleState;
    public ShotGunTurretAttackState attackState;
    
    #endregion


    #region Temp
    private Transform target;
    private Quaternion defaultBaseYRot;
    private Quaternion defaultHeadXRot;
    
    #endregion
    
    #region Get Set
    public void SetTarget(Transform newtTarget)
    {
        target = newtTarget;
    }
    public Transform GetTurretBaseY() => turretBaseY;
    public Transform GetTurretHeadX() => turretHeadX;
    public Transform GetFirePoint() => firePoint;
    public float GetRotationSpeed() => rotationSpeed;
    public float GetFireInterval() => fireInterval;
    public float GetMaxAttackRange() => maxAttackRange;
    public float GetMinAttackRange() => minAttackRange;
    
    public Transform GetTarget() => target;
    #endregion




    #region Unity Functions
    private void Awake()
    {
        defaultBaseYRot = turretBaseY.localRotation;
        defaultHeadXRot = turretHeadX.localRotation;
        idleState = new ShotGunTurretIdleState(this);
        attackState = new ShotGunTurretAttackState(this);
    }

    private void Start()
    {
        ChangeState(idleState);
    }
    
    
    #endregion
    

    #region Functions
    public void ResetRotation()
    {
        turretBaseY.localRotation = Quaternion.Lerp(
            turretBaseY.rotation, defaultBaseYRot, Time.deltaTime * rotationSpeed);

        turretHeadX.localRotation = Quaternion.Lerp(
            turretHeadX.localRotation, defaultHeadXRot, Time.deltaTime * rotationSpeed);
    }
    #endregion
    
}
using UnityEngine;
using CONSTANT;
public class RocketTurretControl : TurretControl
{
    [Header("Turret Data")]
    [SerializeField] private Transform turretBaseY;        
    [SerializeField] private Transform turretHeadX;        
    [SerializeField] private Transform firePoint;          
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float fireInterval = 2f;
    [SerializeField] private float maxAttackRange = 100f;
    [SerializeField] private float minAttackRange = 5f;
    [SerializeField] private float timeToReload = 1f;
    [SerializeField] private float minPitch = 5f; 
    [SerializeField] private float maxPitch = 50f; 
    [Tooltip("Góc cộng thêm (độ) khi mục tiêu ở khoảng cách TỐI THIỂU. Giúp tạo đường cong cao cho mục tiêu ở gần.")]
    [SerializeField] private float arcAngleAtMinRange = 20f;

    [Tooltip("Góc cộng thêm (độ) khi mục tiêu ở khoảng cách TỐI ĐA. Giúp tạo đường cong phẳng hơn cho mục tiêu ở xa.")]
    [SerializeField] private float arcAngleAtMaxRange = 5f;
    
    
    [Header("Turret Settings")]
    [SerializeField] private Transform[] rocketSlotTransforms = new Transform[8];


    #region Temp
    private PoolableObject[] rockets = new PoolableObject[8];
    private Transform target;
    private Quaternion defaultBaseYRot;
    private Quaternion defaultHeadXRot;
    #endregion

    #region Turret State
    public RocketTurretIdleState idleState;
    public RocketTurretAttackState attackState;
    public RocketTurretReloadState reloadState;

    #endregion
    
    #region Get Set
    public PoolableObject[] GetRockets() => rockets;
    public Transform GetTurretBaseY() => turretBaseY;
    public Transform GetTurretHeadX() => turretHeadX;
    public Transform GetFirePoint() => firePoint;
    public float GetRotationSpeed() => rotationSpeed;
    public float GetFireInterval() => fireInterval;
    public float GetMaxAttackRange() => maxAttackRange;
    public float GetMinAttackRange() => minAttackRange;
    public float GetTimeToReload() => timeToReload;
    public float GetMinPitch() => minPitch;
    public float GetMaxPitch() => maxPitch;
    public float GetArcAngleAtMinRange() => arcAngleAtMinRange;
    public float GetArcAngleAtMaxRange() => arcAngleAtMaxRange;
    
    public Transform GetTarget() => target;
    public void SetTarget(Transform newTarget)  => target = newTarget;
    #endregion
    
    #region Unity Functions
    private void Awake()
    {
        defaultBaseYRot = turretBaseY.rotation;
        defaultHeadXRot = turretHeadX.localRotation;
        idleState = new RocketTurretIdleState(this);
        attackState = new RocketTurretAttackState(this);
        reloadState = new RocketTurretReloadState(this);
    }

    private void Start()
    {
        ChangeState(reloadState);
    }

    #endregion

    #region Functions

    public void ResetRotation()
    {
        turretBaseY.rotation = Quaternion.Lerp(
            turretBaseY.rotation, defaultBaseYRot, Time.deltaTime * rotationSpeed);

        turretHeadX.localRotation = Quaternion.Lerp(
            turretHeadX.localRotation, defaultHeadXRot, Time.deltaTime * rotationSpeed);
    }
    public void FillRocket()
    {
        for (int i = 0 ; i < 8; i++)
        {
            if (rockets[i] == null)
            {
                rockets[i] = PoolManager.Instance.Spawn(ProjectileName.ProjectileRocketTurret, rocketSlotTransforms[i].position, rocketSlotTransforms[i]);
                rockets[i].gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                return;
            }
        }
    }
    
    #endregion
}
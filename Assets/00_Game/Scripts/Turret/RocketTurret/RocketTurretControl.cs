using UnityEngine;
using CONSTANT;
public class RocketTurretControl : FSMSystem
{
    [Header("Turret Settings")]
    public Transform turretBaseY;        // trục xoay Y
    public Transform turretHeadX;        // trục xoay X
    public Transform firePoint;          // điểm bắn
    public float rotationSpeed = 5f;
    public float fireInterval = 2f;
    public float attackRange = 100f;
    public float minAttackRange = 5f;
    public float timeToReload = 1f;
    public float minPitch = 5f; // góc thấp (địch gần)
    public float maxPitch = 50f; // góc cao (địch xa)
    [SerializeField] private Transform[] rocketSlotTransforms = new Transform[8];
    private Transform[] rocketTransforms = new Transform[8];
    public Transform[] GetRocketTransforms() => rocketTransforms;
    [HideInInspector] public Transform target;

    private Quaternion defaultBaseYRot;
    private Quaternion defaultHeadXRot;

    [Header("Arc Trajectory Settings")]
    [Tooltip("Góc cộng thêm (độ) khi mục tiêu ở khoảng cách TỐI THIỂU. Giúp tạo đường cong cao cho mục tiêu ở gần.")]
    public float arcAngleAtMinRange = 20f;

    [Tooltip("Góc cộng thêm (độ) khi mục tiêu ở khoảng cách TỐI ĐA. Giúp tạo đường cong phẳng hơn cho mục tiêu ở xa.")]
    public float arcAngleAtMaxRange = 5f;
    
    public RocketTurretIdleState idleState;
    public RocketTurretAttackState attackState;
    public RocketTurretReloadState reloadState;
    private void Awake()
    {
        // save default rotation
        defaultBaseYRot = turretBaseY.rotation;
        defaultHeadXRot = turretHeadX.localRotation;
        
        // init state
        idleState = new RocketTurretIdleState(this);
        attackState = new RocketTurretAttackState(this);
        reloadState = new RocketTurretReloadState(this);
    }

    private void Start()
    {
        // set init state 
        ChangeState(reloadState);
    }

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
            if (rocketTransforms[i] == null)
            {
                rocketTransforms[i] = PoolManager.Instance.Spawn(ProjectileName.ProjectileRocketTurret, rocketSlotTransforms[i].position, rocketSlotTransforms[i]);
                rocketTransforms[i].localRotation = Quaternion.Euler(0, 0, 0);
                Debug.Log(rocketTransforms[i].name);
                return;
            }
        }
    }
}
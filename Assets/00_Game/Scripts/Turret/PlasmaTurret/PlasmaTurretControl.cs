using UnityEngine;

public class PlasmaTurretControl : TurretControl
{
    [Header("Turret Data")]
    [SerializeField] private Transform turretBaseY;        
    [SerializeField] private Transform turretHeadX;        
    [SerializeField] private Transform firePoint;          
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float fireInterval = 3f;
    [SerializeField] private float maxAttackRange = 15f;
    [SerializeField] private float minAttackRange = 5f;

    #region Temp
    private Transform target;
    private Quaternion defaultBaseYRot;
    private Quaternion defaultHeadXRot;

    #endregion

    #region Turret State
    public PlasmaTurretIdleState idleState;
    public PlasmaTurretAttackState attackState;
    
    #endregion


    #region Get Set

    public Transform GetTurretBaseY() => turretBaseY;
    public Transform GetTurretHeadX() => turretHeadX;
    public Transform GetFirePoint() => firePoint;
    public float GetRotationSpeed() => rotationSpeed;
    public float GetFireInterval() => fireInterval;
    public float GetMaxAttackRange() => maxAttackRange;
    public float GetMinAttackRange() => minAttackRange;
    
    public Transform GetTarget() => target;
    public void SetTarget(Transform newTarget) => target = newTarget;

    #endregion
    
    #region Unity Functions
    private void Awake()
    {
        defaultBaseYRot = turretBaseY.rotation;
        defaultHeadXRot = turretHeadX.localRotation;
        idleState = new PlasmaTurretIdleState(this);
        attackState = new PlasmaTurretAttackState(this);
    }
    private void Start()
    {
        ChangeState(idleState);
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
    #endregion
    
}
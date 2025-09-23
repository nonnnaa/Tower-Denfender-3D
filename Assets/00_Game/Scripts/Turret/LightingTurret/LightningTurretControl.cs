using System;
using UnityEngine;

public class LightningTurretControl : TurretControl
{
    [Header("Turret Data")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float fireInterval = 2f;
    [SerializeField] private float maxAttackRange = 100f;
    [SerializeField] private float minAttackRange = 3f;
    [SerializeField] private float timeAttack = 10f;
    
    
    [Header("Turret Settings")]
    [SerializeField] private Transform turretBaseY;        
    [SerializeField] private Transform turretHeadX;        
    [SerializeField] private Transform firePoint;  
    [SerializeField] private LightningControl lightningControl;
    
    
    
    #region Temp
    private Transform target;
    private Quaternion defaultBaseYRot;
    private Quaternion defaultHeadXRot;

    
    #endregion

    #region Turret State
    public LightningTurretIdleState idleState;
    public LightningTurretAttackState attackState;
    
    
    #endregion


    #region Get Set
    public LightningControl GetLightningControl() => lightningControl;
    public Transform GetTurretBaseY() => turretBaseY;
    public Transform GetTurretHeadX() => turretHeadX;
    public Transform GetFirePoint() => firePoint;
    public float GetRotationSpeed() => rotationSpeed;
    public float GetFireInterval() => fireInterval;

    public float GetMinAttackRange() => minAttackRange;

    public float GetMaxAttackRange() => maxAttackRange;

    public float GetTimeAttack() => timeAttack;
    
    public Transform GetTarget() => target;
    public void SetTarget(Transform newTarget) => target = newTarget;
    #endregion

    #region Unity Functions

    private void Awake()
    {
        defaultBaseYRot = turretBaseY.rotation;
        defaultHeadXRot = turretHeadX.localRotation;
        
        idleState = new LightningTurretIdleState(this);
        attackState = new LightningTurretAttackState(this);
        
    }

    private void Start()
    {
        ChangeState(idleState);
    }

    #endregion
    

    public void ResetRotation()
    {
        turretBaseY.rotation = Quaternion.Lerp(
            turretBaseY.rotation, defaultBaseYRot, Time.deltaTime * rotationSpeed);
        turretHeadX.localRotation = Quaternion.Lerp(
            turretHeadX.localRotation, defaultHeadXRot, Time.deltaTime * rotationSpeed);
    }
}
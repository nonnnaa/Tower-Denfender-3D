using System;
using UnityEngine;

public class ShotGunTurretControl : FSMSystem
{
    [Header("Turret Settings")]
    public Transform turretBaseY;        
    public Transform turretHeadX;       
    public Transform firePoint;          
    public float rotationSpeed = 5f;
    public float fireInterval = 0.5f;
    public float attackRange = 15f;
    public float minAttackRange = 3f;

    [NonSerialized] public Transform target;

    private Quaternion defaultBaseYRot;
    private Quaternion defaultHeadXRot;
    
    public ShotGunTurretIdleState idleState;
    public ShotGunTurretAttackState attackState;

    private void Awake()
    {
        defaultBaseYRot = turretBaseY.rotation;
        defaultHeadXRot = turretHeadX.localRotation;
        idleState = new ShotGunTurretIdleState(this);
        attackState = new ShotGunTurretAttackState(this);
    }

    private void Start()
    {
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
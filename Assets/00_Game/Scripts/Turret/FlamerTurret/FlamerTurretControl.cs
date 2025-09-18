using UnityEngine;

public class FlamerTurretControl : TurretControl
{
    [Header("Turret Data")]
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private float maxAttackRange = 12f;
    [SerializeField] private float minAttackRange = 1.5f;

    [Header("Turret Settings")]
    [SerializeField] private Transform turretBaseY;
    [SerializeField] private Transform turretHeadX;

    [Header("Effects")]
    [SerializeField] private ParticleSystem flamePS;
    [SerializeField] private AudioSource flameSFX;
    [SerializeField] private Transform flameFirePoint;

    [Header("Hit Effects")]
    [SerializeField] private ParticleSystem smokePrefab;

    #region Temp
    private Transform target;
    private Quaternion defaultBaseYRot;
    private Quaternion defaultHeadXRot;
    #endregion

    #region Turret State
    public FlamerTurretIdleState idleState;
    public FlamerTurretAttackState attackState;
    #endregion

    #region Get Set
    public Transform GetTurretBaseY() => turretBaseY;
    public Transform GetTurretHeadX() => turretHeadX;
    public float GetRotationSpeed() => rotationSpeed;
    public float GetMaxAttackRange() => maxAttackRange;

    public Transform GetTarget() => target;
    public void SetTarget(Transform newTarget) => target = newTarget;

    public Transform GetFlameFirePoint() => flameFirePoint;
    #endregion

    #region Unity Functions
    private void Awake()
    {
        defaultBaseYRot = turretBaseY.rotation;
        defaultHeadXRot = turretHeadX.localRotation;

        idleState = new FlamerTurretIdleState(this);
        attackState = new FlamerTurretAttackState(this);
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

    public void StartFlame()
    {
        if (flamePS != null && !flamePS.isPlaying)
            flamePS.Play();

        if (flameSFX != null && !flameSFX.isPlaying)
            flameSFX.Play();
    }

    public void StopFlame()
    {
        if (flamePS != null && flamePS.isPlaying)
            flamePS.Stop();

        if (flameSFX != null && flameSFX.isPlaying)
            flameSFX.Stop();
    }

    // ✅ Smoke Effect Control
    public void PlaySmoke(Transform enemy)
    {
        if (enemy == null || smokePrefab == null) return;

        // tìm smoke child đã có
        ParticleSystem smoke = enemy.GetComponentInChildren<ParticleSystem>();
        if (smoke == null || smoke.name != smokePrefab.name)
        {
            smoke = Instantiate(smokePrefab, enemy.position, Quaternion.identity, enemy);
            smoke.name = smokePrefab.name;
        }
        if (!smoke.isPlaying) smoke.Play();
    }

    public void StopSmoke(Transform enemy)
    {
        if (enemy == null) return;

        ParticleSystem smoke = enemy.GetComponentInChildren<ParticleSystem>();
        if (smoke != null && smoke.isPlaying) smoke.Stop();
    }
    #endregion
}

using UnityEngine;

public class DisruptorTurretControl : TurretControl
{
    [Header("Turret Data")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float fireInterval = 2f;
    [SerializeField] private float maxAttackRange = 20f;
    [SerializeField] private float minAttackRange = 3f;
    [SerializeField] private float timeAttack = 0.2f;

    [Header("Attack Settings")]
    [SerializeField] private int maxChainTargets = 3;
    [SerializeField] private float chainRange = 6f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float stunDuration = 1.5f;

    [Header("Turret Settings")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private DisruptorLightningControl lightningControl;

    [Header("Effects")]
    [SerializeField] private ParticleSystem muzzleFlarePS;   // tia lóe ở nòng
    [SerializeField] private ParticleSystem impactPrefab;    // prefab effect trúng mục tiêu
    [SerializeField] private ParticleSystem rangeCircle;     // vòng tròn phạm vi

    #region Temp
    private Transform target;
    private Quaternion defaultBaseYRot;
    private Quaternion defaultHeadXRot;
    #endregion

    #region Turret State
    public DisruptorTurretIdleState idleState;
    public DisruptorTurretAttackState attackState;
    #endregion

    #region Get Set
    public Transform GetTarget() => target;
    public void SetTarget(Transform newTarget) => target = newTarget;
    public float GetRotationSpeed() => rotationSpeed;
    public float GetFireInterval() => fireInterval;
    public float GetMinAttackRange() => minAttackRange;
    public float GetMaxAttackRange() => maxAttackRange;
    public float GetTimeAttack() => timeAttack;
    public Transform GetFirePoint() => firePoint;
    public DisruptorLightningControl GetLightningControl() => lightningControl;

    public int GetMaxChainTargets() => maxChainTargets;
    public float GetChainRange() => chainRange;
    public float GetDamage() => damage;
    public float GetStunDuration() => stunDuration;
    #endregion

    private void Awake()
    {
        idleState = new DisruptorTurretIdleState(this);
        attackState = new DisruptorTurretAttackState(this);
    }

    private void Start()
    {
        ChangeState(idleState);
        if (rangeCircle != null) rangeCircle.Play();
    }

    // Hiệu ứng đầu nòng
    public void PlayMuzzleEffect()
    {
        if (muzzleFlarePS != null)
        {
            muzzleFlarePS.transform.position = firePoint.position;
            muzzleFlarePS.transform.rotation = firePoint.rotation;
            muzzleFlarePS.Play();
        }
    }

    // Hiệu ứng trúng enemy (gắn 1 lần duy nhất, sau đó bật/tắt)
    public void PlayImpactEffect(Transform enemy)
    {
        if (enemy == null || impactPrefab == null)
        {
            Debug.LogWarning("Impact effect không có prefab hoặc enemy null!");
            return;
        }

        ParticleSystem impact = FindOrCreateImpact(enemy);

        if (impact != null && !impact.isPlaying)
        {
            impact.Play();
        }
    }

    public void StopImpactEffect(Transform enemy)
    {
        if (enemy == null) return;

        ParticleSystem impact = FindImpact(enemy);
        if (impact != null && impact.isPlaying)
        {
            impact.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    private ParticleSystem FindImpact(Transform enemy)
    {
        foreach (var ps in enemy.GetComponentsInChildren<ParticleSystem>(true))
        {
            if (ps.name == impactPrefab.name) return ps;
        }
        return null;
    }

    private ParticleSystem FindOrCreateImpact(Transform enemy)
    {
        ParticleSystem impact = FindImpact(enemy);

        if (impact == null)
        {
            impact = Instantiate(impactPrefab, enemy.position, Quaternion.identity, enemy);
            impact.name = impactPrefab.name;

            // ⚡ Quan trọng: giữ effect không tự hủy
            var main = impact.main;
            main.stopAction = ParticleSystemStopAction.None;
            main.loop = true; // để nó chỉ tắt khi mình gọi StopImpactEffect
        }

        return impact;
    }
}

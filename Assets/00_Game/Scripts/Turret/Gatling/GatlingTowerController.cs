using System.Collections;
using UnityEngine;

public class GatlingTowerController : TowerBase
{
    [Header("Turret Settings")]
    public Transform turretHead;
    public float rotationSpeed = 10f;
    public Transform firePoint;
    public float fireInterval = 0.1f;   // bắn rất nhanh
    public float attackRange = 8f;

    [Header("FX & Audio")]
    public ParticleSystem muzzleFlash;  // hiệu ứng chớp nòng
    public ParticleSystem shellEject;   // hiệu ứng vỏ đạn văng ra
    public AudioSource gatlingSound;    // âm thanh quay nòng

    private Transform target;
    private Coroutine shootingCoroutine;

    private void Update()
    {
        target = FindClosestEnemy();
        if (target != null)
        {
            // Xoay turret
            Vector3 direction = target.position - turretHead.position;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                turretHead.rotation = Quaternion.Lerp(
                    turretHead.rotation,
                    lookRotation,
                    Time.deltaTime * rotationSpeed
                );
            }

            // Bắt đầu bắn
            if (shootingCoroutine == null)
            {
                shootingCoroutine = StartCoroutine(ShootRoutine());
                if (gatlingSound != null && !gatlingSound.isPlaying)
                    gatlingSound.Play();
            }
        }
        else
        {
            // Dừng bắn
            if (shootingCoroutine != null)
            {
                StopCoroutine(shootingCoroutine);
                shootingCoroutine = null;
            }

            if (gatlingSound != null && gatlingSound.isPlaying)
                gatlingSound.Stop();
        }
    }

    private Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < minDist && dist <= attackRange)
            {
                minDist = dist;
                closest = enemy.transform;
            }
        }
        return closest;
    }

    private IEnumerator ShootRoutine()
    {
        while (target != null)
        {
            Shoot();
            yield return new WaitForSeconds(fireInterval);
        }
    }

    private void Shoot()
    {
        if (firePoint && target != null)
        {
            // Spawn bullet từ PoolManager
            if (PoolManager.dic_pool.TryGetValue("Bullet", out ObjectPool bulletPool))
            {
                Transform bulletTrans = bulletPool.OnSpawned();
                if (bulletTrans != null)
                {
                    bulletTrans.position = firePoint.position;
                    bulletTrans.rotation = firePoint.rotation;

                    Bullet bullet = bulletTrans.GetComponent<Bullet>();
                    if (bullet != null)
                    {
                        bullet.SetTarget(target);
                    }
                }
            }

            // Hiệu ứng nòng súng + vỏ đạn
            if (muzzleFlash != null) muzzleFlash.Play();
            if (shellEject != null) shellEject.Play();
        }
    }
}

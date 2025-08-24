using System.Collections;
using UnityEngine;

public class MortarTowerController : TowerBase
{
    [Header("Turret Settings")]
    public Transform turretHead;
    public float rotationSpeed = 3f;
    public Transform firePoint;
    public float fireInterval = 3f;  // bắn chậm hơn SonicTower
    public float attackRange = 12f;

    [Header("Projectile Settings")]
    public float launchForce = 15f;   // lực bắn ngang
    public float upwardForce = 8f;    // lực bắn hướng lên

    private Transform target;
    private Coroutine shootingCoroutine;

    private void Update()
    {
        target = FindClosestEnemy();
        if (target != null)
        {
            // Xoay turret theo hướng mục tiêu (chỉ quay trục Y)
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
                shootingCoroutine = StartCoroutine(ShootRoutine());
        }
        else
        {
            // Dừng bắn
            if (shootingCoroutine != null)
            {
                StopCoroutine(shootingCoroutine);
                shootingCoroutine = null;
            }
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
            // Lấy MortarShell từ PoolManager
            if (PoolManager.dic_pool.TryGetValue("MortarBullet", out ObjectPool shellPool))
            {
                Transform shellTrans = shellPool.OnSpawned();
                if (shellTrans != null)
                {
                    shellTrans.position = firePoint.position;
                    shellTrans.rotation = Quaternion.identity;

                    MortarBullet shell = shellTrans.GetComponent<MortarBullet>();
                    if (shell != null)
                    {
                        // Tính hướng bay
                        Vector3 dir = (target.position - firePoint.position).normalized;
                        Vector3 force = new Vector3(dir.x * launchForce, upwardForce, dir.z * launchForce);

                        shell.Launch(force);
                    }
                }
            }
            else
            {
                Debug.LogWarning("Pool 'MortarShell' chưa được tạo trong PoolManager!");
            }
        }
    }
}

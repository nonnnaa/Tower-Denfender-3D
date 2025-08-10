using System.Collections;
using UnityEngine;

public class SonicTurretController : TurretBase
{
    [Header("Turret Settings")]
    public Transform turretHead;       // Phần xoay của turret
    public float rotationSpeed = 5f;   // Tốc độ xoay
    public GameObject bulletPrefab;    // Prefab viên đạn
    public Transform firePoint;        // Vị trí bắn đạn
    public float fireInterval = 0.5f;  // Thời gian giữa các lần bắn

    private Transform target;          // Enemy đang bị khóa
    private Coroutine shootingCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            target = other.transform;
            // Bắt đầu bắn
            if (shootingCoroutine == null)
                shootingCoroutine = StartCoroutine(ShootRoutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && other.transform == target)
        {
            target = null;
            // Ngừng bắn
            if (shootingCoroutine != null)
            {
                StopCoroutine(shootingCoroutine);
                shootingCoroutine = null;
            }
        }
    }

    private void Update()
    {
        if (target != null)
        {
            // Xoay turret hướng về enemy
            Vector3 direction = target.position - turretHead.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            turretHead.rotation = Quaternion.Lerp(turretHead.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
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
        if (bulletPrefab && firePoint)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}
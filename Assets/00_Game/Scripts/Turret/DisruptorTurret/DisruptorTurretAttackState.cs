using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisruptorTurretAttackState : FSMState
{
    private readonly DisruptorTurretControl turretControl;
    private Coroutine attackRoutine;

    public DisruptorTurretAttackState(DisruptorTurretControl turretControl)
    {
        this.turretControl = turretControl;
    }

    public override void EnterState()
    {
        attackRoutine = turretControl.StartCoroutine(AttackRoutine());
    }

    public override void UpdateState()
    {
        Transform target = turretControl.GetTarget();
        if (target == null)
        {
            turretControl.ChangeState(turretControl.idleState);
            return;
        }

        float dist = Vector3.Distance(turretControl.transform.position, target.position);
        if (dist > turretControl.GetMaxAttackRange() || dist < turretControl.GetMinAttackRange())
        {
            turretControl.ChangeState(turretControl.idleState);
            return;
        }
    }

    public override void ExitState()
    {
        if (attackRoutine != null)
            turretControl.StopCoroutine(attackRoutine);

        // tắt toàn bộ impact đang chạy
        turretControl.StopAllImpactEffects();
    }



    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(turretControl.GetFireInterval());
            Fire();
        }
    }

    private void Fire()
    {
        Transform target = turretControl.GetTarget();
        if (target == null) return;

        // Hiệu ứng đầu nòng
        turretControl.PlayMuzzleEffect();

        // Tìm các mục tiêu trong chain
        List<Transform> chainTargets = new List<Transform> { target };
        FindChainTargets(target, chainTargets);

        // Spawn đạn/điện
        PoolableObject pooled = PoolManager.Instance.Spawn(
            CONSTANT.ProjectileName.ProjectileDisruptorBullet,
            turretControl.GetFirePoint().position
        );

        if (pooled != null)
        {
            DisruptorLightningControl lightning = pooled.GetComponent<DisruptorLightningControl>();
            lightning.SetTargets(chainTargets, pooled);

            // Bật hiệu ứng impact trên tất cả enemy trúng
            foreach (Transform enemy in chainTargets)
            {
                turretControl.PlayImpactEffect(enemy);
            }
        }
    }

    private void FindChainTargets(Transform startTarget, List<Transform> chainTargets)
    {
        int maxTargets = turretControl.GetMaxChainTargets();
        float chainRange = turretControl.GetChainRange();

        Transform current = startTarget;
        while (chainTargets.Count < maxTargets)
        {
            Transform next = FindNearestEnemy(current.position, chainTargets, chainRange);
            if (next == null) break;

            chainTargets.Add(next);
            current = next;
        }
    }

    private Transform FindNearestEnemy(Vector3 fromPos, List<Transform> excludeList, float range)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject e in enemies)
        {
            Transform et = e.transform;
            if (excludeList.Contains(et)) continue;

            float dist = Vector3.Distance(fromPos, et.position);
            if (dist < minDist && dist <= range)
            {
                minDist = dist;
                nearest = et;
            }
        }

        return nearest;
    }
}

using DG.Tweening;
using UnityEngine;
using CONSTANT;
public class PlasmaBullet : ShotGunBullet
{
    [SerializeField] private float timeToScale;
    protected override void HandlePreShooting(float timeDelay = 0)
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1, timeToScale)
            .OnComplete(() =>
            {
                base.HandlePreShooting();
                PoolManager.Instance.Spawn(MuzzleFlareName.MuzzleFlarePlasmaTurret, transform.position, currentParent);
            });
    }
}

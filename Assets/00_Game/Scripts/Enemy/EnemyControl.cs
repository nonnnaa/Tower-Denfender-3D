using UnityEngine;
public enum TeamType
{
    TeamA,
    TeamB
}

public class EnemyControl : FSMSystem
{
    public const TeamType Team = TeamType.TeamB;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected Transform hpHubPoint;
    public Transform AttackPoint => attackPoint;

    public virtual void Init(string enemyKey)
    {
        
    }

    public virtual void TakeDamage(int damage)
    {
        
    }
    protected virtual void OnDead()
    {
        Destroy(gameObject);
    }
}

using UnityEngine;
public enum TeamType
{
    TeamA,
    TeamB
}

public class EnemyControl : FSMSystem
{
    public const TeamType Team = TeamType.TeamB;
    [SerializeField] private Transform attackPoint;
    public Transform AttackPoint => attackPoint;

    public virtual void Init(string enemyKey)
    {
        
    }
}

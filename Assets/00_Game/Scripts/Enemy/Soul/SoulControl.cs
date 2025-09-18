using System;
using UnityEngine;

[Serializable]
public class SoulDataInGame
{
    private FileConfigEnemyRecord data;
}
public class SoulControl : EnemyControl
{
    [SerializeField] private FileConfigEnemyRecord dataRecord;
    [SerializeField] private SoulDataBinding soulDataBinding;
    public SoulDataBinding SoulDataBinding => soulDataBinding;
    
    public SoulMoveState moveState;
    public SoulAttackState attackState;
    public SoulHitState hitState;
    public SoulDeadState deadState;

    
    public float GetSpeedMove() => dataRecord.Speed;
    public float GetAttackRange() => dataRecord.AttackRange;
    public void Awake()
    {
        dataRecord = ConfigManager.Instance.GetFileConfigEnemy().GetEnemyRecordByName("Soul3");
        moveState = new SoulMoveState(this);
        attackState = new SoulAttackState(this);
        hitState = new SoulHitState(this);
        deadState = new SoulDeadState(this);
    }

    public void Init()
    {
        
    }
    
    
    private void Start()
    {
        ChangeState(moveState);
    }
}

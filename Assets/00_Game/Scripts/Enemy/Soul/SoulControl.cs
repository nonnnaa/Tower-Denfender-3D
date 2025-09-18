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
        Init("Soul3");
        moveState = new SoulMoveState(this);
        attackState = new SoulAttackState(this);
        hitState = new SoulHitState(this);
        deadState = new SoulDeadState(this);
    }

    public void Init(string enemyKey)
    {
        dataRecord = ConfigManager.Instance.GetFileConfigEnemy().GetEnemyRecordByName(enemyKey);
    }
    
    private void Start()
    {
        ChangeState(moveState);
    }

    public override void OnMidlleAnim()
    {
        base.OnMidlleAnim();
    }
}

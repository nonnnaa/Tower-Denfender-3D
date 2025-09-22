using System;
using UnityEngine;
using CONSTANT;
[Serializable]
public class EnemyDataInGame
{
    private FileConfigEnemyRecord data;
    private int hp;
    
    public EnemyDataInGame(FileConfigEnemyRecord newData, int newHp)
    {
        data = newData;
        hp = newHp;
    }
    
    public FileConfigEnemyRecord DataRecord => data;
    public int Hp => hp;
    public void OnDamage(int damage, Action callback)
    {
        hp -= damage;
        if (hp <= 0)
        {
            callback?.Invoke();;
        }
    }
}
public class SoulControl : EnemyControl
{
    private EnemyDataInGame dataInGame;
    [SerializeField] private FileConfigEnemyRecord dataRecord; // Thừa
    [SerializeField] private SoulDataBinding soulDataBinding;
    public SoulDataBinding SoulDataBinding => soulDataBinding;
    private HpHub hpHub;
    
    public SoulMoveState moveState;
    public SoulAttackState attackState;
    public SoulHitState hitState;
    public SoulDeadState deadState;

    
    public float GetSpeedMove() => dataRecord.Speed;
    public float GetAttackRange() => dataRecord.AttackRange;
    public void Awake()
    {
        moveState = new SoulMoveState(this);
        attackState = new SoulAttackState(this);
        hitState = new SoulHitState(this);
        deadState = new SoulDeadState(this);
    }

    public override void Init(string enemyKey)
    {
        base.Init(enemyKey);
        dataRecord = ConfigManager.Instance.GetFileConfigEnemy().GetEnemyRecordByName(enemyKey);
        dataInGame = new EnemyDataInGame(dataRecord, dataRecord.Hp);
        CanvasGamePlay canvasGamePlay = FindAnyObjectByType<CanvasGamePlay>();
        hpHub = (HpHub)PoolManager.Instance.Spawn(UIElementName.HpHub, hpHubPoint.position, canvasGamePlay.GetHudHp());
        hpHub.SetupHub(hpHubPoint, canvasGamePlay.GetHudHp());
    }
    
    private void Start()
    {
        ChangeState(moveState);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        dataInGame.OnDamage(damage, OnDead);
        hpHub.UpdateHp(dataInGame.Hp, dataRecord.Hp);
        Debug.Log(dataInGame.Hp + " - " + damage);
    }

    private void OnDead()
    {
        Destroy(gameObject);
    }
}

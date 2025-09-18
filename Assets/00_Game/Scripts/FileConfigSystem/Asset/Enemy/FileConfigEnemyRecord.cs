using System;
using UnityEngine;

[Serializable]
public class FileConfigEnemyRecord
{
    [SerializeField] private int id;
    public int Id => id;

    [SerializeField] private string name;
    public string Name => name;

    [SerializeField] private string prefabName;
    public string PrefabName => prefabName;

    [SerializeField] private int level;
    public int Level => level;

    [SerializeField] private int hp;
    public int Hp => hp;

    [SerializeField] private int atk;
    public int Atk => atk;

    [SerializeField] private int def;
    public int Def => def;

    [SerializeField] private float speed;
    public float Speed => speed;

    [SerializeField] private float attackRange;
    public float AttackRange => attackRange;

    public EnemyControl Prefab => Resources.Load<EnemyControl>("Enemy/" + prefabName);
}
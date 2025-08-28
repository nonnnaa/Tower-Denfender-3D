using System;
using UnityEngine;

public enum AnimationType
{
    Melee,
    Projectile,
    Cast,
    Summon,
    AOE
}

[Serializable]
public class FileConfigEnemySkillRecord
{
    [SerializeField] private int id;
    public int Id => id;

    [SerializeField] private string enemyId;
    public string EnemyId => enemyId;

    [SerializeField] private string skillName;
    public string SkillName => skillName;

    [SerializeField] private AnimationType animType;
    public AnimationType AnimType => animType;

    [SerializeField] private float radius;
    public float Radius => radius;

    [SerializeField] private float damage;
    public float Damage => damage;

    [SerializeField] private float duration;
    public float Duration => duration;

    [SerializeField] private string prefabName;
    public string PrefabName => prefabName;
}
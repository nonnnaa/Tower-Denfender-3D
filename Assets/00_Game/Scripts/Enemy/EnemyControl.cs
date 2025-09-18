using UnityEngine;
public enum TeamType
{
    TeamA,
    TeamB
}

public class EnemyControl : FSMSystem
{
    public const TeamType Team = TeamType.TeamB;
}

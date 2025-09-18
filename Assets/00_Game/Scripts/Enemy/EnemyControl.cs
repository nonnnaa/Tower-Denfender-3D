using UnityEngine;
public enum TeamType
{
    TeamA,
    TeamB
}

public class EnemyControl : FSMSystem
{
    protected const TeamType Team = TeamType.TeamB;
}

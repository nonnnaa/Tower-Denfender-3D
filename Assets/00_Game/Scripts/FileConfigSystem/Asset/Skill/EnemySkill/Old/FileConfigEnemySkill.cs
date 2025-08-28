using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileConfigEnemySkill : FileConfig<FileConfigEnemySkillRecord>
{
    public override void DefineConfigCompare()
    {
        records = new List<FileConfigEnemySkillRecord>();
    }
}

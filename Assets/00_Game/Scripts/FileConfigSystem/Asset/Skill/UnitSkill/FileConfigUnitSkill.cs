using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileConfigUnitSkill : FileConfig<FileConfigUnitSkillRecord>
{
    public override void DefineConfigCompare()
    {
        records = new List<FileConfigUnitSkillRecord>();
    }
}

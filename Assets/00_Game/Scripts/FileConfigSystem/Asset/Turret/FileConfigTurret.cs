using System.Collections.Generic;
public class FileConfigTurret : FileConfig<FileConfigTurretRecord>
{
    public override void DefineConfigCompare()
    {
        records = new List<FileConfigTurretRecord>();
    }
}

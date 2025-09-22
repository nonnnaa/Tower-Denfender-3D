using System.Collections.Generic;
public class FileConfigTurret : FileConfig<FileConfigTurretRecord>
{
    public override void DefineConfigCompare()
    {
        configCompare = new ConfigCompare<FileConfigTurretRecord>("id");
    }

    public List<FileConfigTurretRecord> GetAllFileConfigTurretRecord()
    {
        return records;
    }
}

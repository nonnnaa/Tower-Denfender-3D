using System;

public class FileConfigEnemy : FileConfig<FileConfigEnemyRecord>
{
    public override void DefineConfigCompare()
    {
        configCompare = new ConfigCompare<FileConfigEnemyRecord>("id", "name");
    }

    public FileConfigEnemyRecord GetEnemyRecordByName(string nameEnemy)
    {
        foreach (var record in records)
        {
            if (String.Compare(record.Name, nameEnemy, StringComparison.Ordinal) == 0)
            {
                return record;
            }
        }
        return null;
    }

    public FileConfigEnemyRecord GetEnemyRecordById(int id)
    {
        foreach (var record in records)
        {
            if (id == record.Id)
            {
                return record;
            }
        }
        return null;
    }
}

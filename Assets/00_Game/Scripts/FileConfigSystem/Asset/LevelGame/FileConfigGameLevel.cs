public class FileConfigGameLevel : FileConfig<FileConfigGameLevelRecord>
{
    public override void DefineConfigCompare()
    {
        configCompare = new ConfigCompare<FileConfigGameLevelRecord>("id");
    }

    public FileConfigGameLevelRecord GetFileConfigGameLevelRecordById(int id)
    {
        foreach (var record in records)
        {
            if (record.Id == id)
            {
                return record;
            }
        }
        return null;
    }

    public int GetEnemyCount(int id)
    {
        int count = 0;
        foreach (var record in records)
        {
            if (record.Id == id)
            {
                foreach (var wave in record.EnemyWaves)
                {
                    count += wave.Count;
                }
            }
        }
        return count;
    }
}

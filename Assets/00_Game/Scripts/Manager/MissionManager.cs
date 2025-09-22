using System.Collections.Generic;

public class MissionManager : SingletonMono<MissionManager>
{
    private FileConfigGameLevelRecord gameLevelRecord;
    private List<List<EnemyInWave>> enemiesInWaves;

    protected override void Awake()
    {
        base.Awake();
        gameLevelRecord = ConfigManager.Instance.GetFileConfigGameLevel().GetFileConfigGameLevelRecordById(0);
        enemiesInWaves = ConfigManager.Instance.GetFileConfigGameLevel().GetFileConfigGameLevelRecordById(0).EnemyWaves;
    }

    
}

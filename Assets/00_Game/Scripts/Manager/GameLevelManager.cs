using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelManager : SingletonMono<GameLevelManager>
{
    private FileConfigGameLevelRecord gameLevelRecord;
    private List<List<EnemyInWave>> enemiesInWaves;
    [SerializeField] private List<Transform> spawnPositions;
    protected override void Awake()
    {
        base.Awake();
        gameLevelRecord = ConfigManager.Instance.GetFileConfigGameLevel().GetFileConfigGameLevelRecordById(1);
        enemiesInWaves = ConfigManager.Instance.GetFileConfigGameLevel().GetFileConfigGameLevelRecordById(1).EnemyWaves;
    }

    private void Start()
    {
        StartCoroutine(StartNewWave(enemiesInWaves[0]));
    }

    IEnumerator StartNewWave(List<EnemyInWave> enemyInWave)
    {
        foreach (var enemy in enemyInWave)
        {
            yield return new WaitForSeconds(enemy.GetCoolDown());
            int enemyId = enemy.GetId();
            FileConfigEnemyRecord enemyRecord = ConfigManager.Instance.GetFileConfigEnemy().GetEnemyRecordById(enemyId);
            EnemyControl enemyControl = Instantiate(enemyRecord.Prefab, spawnPositions[0].position, spawnPositions[0].rotation);
        }
    }
}

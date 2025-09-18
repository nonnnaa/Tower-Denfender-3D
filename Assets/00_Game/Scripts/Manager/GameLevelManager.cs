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
        StartCoroutine(StartWaves());
    }

    IEnumerator StartWaves()
    {
        foreach (var wave in enemiesInWaves)
        {
            yield return StartCoroutine(StartNewWave(wave));
        }
    }

    IEnumerator StartNewWave(List<EnemyInWave> enemyInWave)
    {
        for (int i = 0; i < enemyInWave.Count; i++)
        {
            EnemyInWave enemy = enemyInWave[i];
            yield return new WaitForSeconds(enemy.GetCoolDown());

            int enemyId = enemy.GetId();
            FileConfigEnemyRecord enemyRecord = ConfigManager.Instance.GetFileConfigEnemy().GetEnemyRecordById(enemyId);
            
            Transform spawnPos = spawnPositions[i % spawnPositions.Count];
            Instantiate(enemyRecord.Prefab, spawnPos.position, spawnPos.rotation);
        }
    }
    
    
}

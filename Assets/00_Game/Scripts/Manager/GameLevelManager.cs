using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelManager : SingletonMono<GameLevelManager>
{
    private FileConfigGameLevelRecord gameLevelRecord;
    private List<List<EnemyInWave>> enemiesInWaves;
    [SerializeField] private Transform spawnA;
    [SerializeField] private Transform spawnB;
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
        for(int  i = 0; i < enemiesInWaves.Count; i++)
        {
            yield return new WaitForSeconds(gameLevelRecord.TimeStartWaves[i]);
            StartCoroutine(StartNewWave(enemiesInWaves[i]));
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
            
            EnemyControl enemyControl = Instantiate(enemyRecord.GetPrefab(), spawnB.position, spawnB.rotation);
            enemyControl.Init(enemyRecord.Name);
        }
    }
    
    
}

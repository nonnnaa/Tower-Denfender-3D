using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CONSTANT;
public class GameLevelManager : SingletonMono<GameLevelManager>
{
    private FileConfigGameLevelRecord gameLevelRecord;
    private List<List<EnemyInWave>> enemiesInWaves;
    [SerializeField] private Transform spawnA;
    [SerializeField] private Transform spawnB;
    
    private int enemyCount;
    protected override void Awake()
    {
        base.Awake();
        gameLevelRecord = ConfigManager.Instance.GetFileConfigGameLevel().GetFileConfigGameLevelRecordById(1);
        enemiesInWaves = ConfigManager.Instance.GetFileConfigGameLevel().GetFileConfigGameLevelRecordById(1).EnemyWaves;
        EventManager.Instance.OnEnemyDestroy += UpdateEnemyCount;
        EventManager.Instance.OnLoseLevel += OnLoseLevel;
    }

    private void Start()
    {
        StartCoroutine(StartWaves());
        enemyCount = gameLevelRecord.GetEnemyCount();
        Debug.Log(enemyCount);
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

    private void UpdateEnemyCount()
    {
        enemyCount--;
        if (enemyCount <= 0)
        {
            OnWinLevel();
        }
        Debug.Log(enemyCount);
    }
    public void OnEndLevel()
    {
        EventManager.Instance.OnEnemyDestroy -= UpdateEnemyCount;
        EventManager.Instance.OnLoseLevel -= OnLoseLevel;
        UIManager.Instance.CloseAll();
    }
    public void OnLoseLevel()
    {
        OnEndLevel();
        UIManager.Instance.OpenUI<CanvasLose>();
    }

    private void OnWinLevel()
    {
        OnEndLevel();
        UIManager.Instance.OpenUI<CanvasWin>();
    }
}

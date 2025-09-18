using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class FileConfigGameLevelRecord 
{
    [SerializeField] private int id; 
    public int Id => id;
    [SerializeField] private int rewardGold;    
    public int RewardGold => rewardGold;
    [SerializeField] private int rewardExp;  
    public int RewardExp => rewardExp;
    [SerializeField] private float timeLimit;
    public float TimeLimit => timeLimit;
    // Format 1:1,1:2,1:1|1:1,2:2,2:2|2:2,2:3 -> mỗi wave cách nhau '|', mỗi enemy cách nhau ',', "idEnemy" : "coolDownTimeSpawn
    [SerializeField] private  string enemyWaves;

    public List<List<EnemyInWave>> EnemyWaves
    {
        get
        {
            List<int> timeToStarts = TimeStartWaves;
            List<List<EnemyInWave>> result = new List<List<EnemyInWave>>();
            string[] waves = enemyWaves.Trim().Split('|');
            for (int i = 0; i < waves.Length; i++)
            {
                string[] enemys = waves[i].Split('-');
                List<EnemyInWave> coolDown = new List<EnemyInWave>();
                foreach (var enemy in enemys)
                {
                    string[] infor = enemy.Split(':');
                    coolDown.Add(new EnemyInWave(int.Parse(infor[0]), int.Parse(infor[1]), timeToStarts[i]));
                }

                result.Add(coolDown);
            }

            return result;
        }
    }

    // 1|2|3 thời gian bắt đầu wave trong level ->  thứ tự matching với enemyWaves
    [SerializeField] private string timeStartWaves;
    public List<int> TimeStartWaves
    {
        get
        {
            List<int> result1 = new List<int>();
            //Debug.Log(timeStartWaves);
            string[] times = timeStartWaves.Trim().Split('|');
            foreach (string time in times)
            {
                //Debug.Log(time);
                result1.Add(int.Parse(time));
            }
            return result1;
        }
    }
}
// idEnemy - coolDown : idEnemy xem trong excel FileConfigEnemy id này chứa thông tin về level lẫn monster type r
public class EnemyInWave
{
    public EnemyInWave(int id, float coolDown, float timeStart)
    {
        idEnemy = id;
        coolDownToSpawn = coolDown;
        timeStartWave = timeStart;
    }
    private float timeStartWave;
    private int idEnemy;
    private float coolDownToSpawn;
    public float GetTimeStartWave() => timeStartWave;
    public int GetId() => idEnemy;
    public float GetCoolDown() => coolDownToSpawn;
}


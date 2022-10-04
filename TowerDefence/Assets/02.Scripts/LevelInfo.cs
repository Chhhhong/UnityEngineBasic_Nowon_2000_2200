using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo", menuName = "TowerDefence/LevelInfo")]
public class LevelInfo : ScriptableObject
{
    public int level;
    public int lifeInit;
    public int moneyInit;
    public List<StageInfo> stagesInfo;
}

[System.Serializable]
public class StageInfo
{
    public int id;
    public List<EnemySpawnData> enemySpawnDataList = new List<EnemySpawnData>();
}

[System.Serializable]
public class EnemySpawnData
{
    public PoolElement poolElement;
    public int spawnPointIndex;
    public int goalPointIndex;
    public float term;
    public float delay;
}

[System.Serializable]
public class PoolElement
{
    public GameObject prefab;
    public int num;
}
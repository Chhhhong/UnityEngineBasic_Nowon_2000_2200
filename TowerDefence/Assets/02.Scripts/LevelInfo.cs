using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo", menuName = "TowerDefence/LevelInfo")]
public class LevelInfo : ScriptableObject
{
    public int lifeInit;
    public int moneyInit;
    public List<StageInfo> stagesInfo;
}

[System.Serializable]
public class StageInfo
{
    public List<EnemySpawnData> enemyspawnDataList = new List<EnemySpawnData>();
}

[System.Serializable]
public class EnemySpawnData
{
    public GameObject prefab;
    public int num;
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private List<StageInfo> stageList = new List<StageInfo>();
    private List<float[]> timersList = new List<float[]>();
    private List<float[]> delayTimersList = new List<float[]>();
    private List<int[]> spawnCountersList = new List<int[]>();
    private List<List<GameObject>> enemiesSpawnedList = new List<List<GameObject>>();
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] goalPoints;
    public event Action<int> OnStageFinished;


    public void StartSpawn(StageInfo stageInfo)
    {
        stageList.Add(stageInfo);
        float[] tmpTimerList = new float[stageInfo.enemySpawnDataList.Count];
        float[] tmpDelayTimersList = new float[stageInfo.enemySpawnDataList.Count];
        int[] tmpSpawnCountersList = new int[stageInfo.enemySpawnDataList.Count];

        for (int i = 0; i < stageInfo.enemySpawnDataList.Count; i++)
        {
            tmpTimerList[i] = stageInfo.enemySpawnDataList[i].term;
            tmpDelayTimersList[i] = stageInfo.enemySpawnDataList[i].delay;
            tmpSpawnCountersList[i] = stageInfo.enemySpawnDataList[i].poolElement.num;
        }

        timersList.Add(tmpTimerList);
        delayTimersList.Add(tmpDelayTimersList);
        spawnCountersList.Add(tmpSpawnCountersList);
        enemiesSpawnedList.Add(new List<GameObject>());
    }

    private void Update()
    {
        for (int i = stageList.Count - 1; i >=0; i--)
        {
            bool isSpawnFinished = true;
            for (int j = 0; j < stageList[i].enemySpawnDataList.Count; j++)
            {
                if (delayTimersList[i][j] < 0)
                {
                    if (timersList[i][j] < 0)
                    {
                        if (spawnCountersList[i][j] > 0)
                        {
                            GameObject go = Instantiate(original: stageList[i].enemySpawnDataList[j].poolElement.prefab,
                                                    position: spawnPoints[i].position,
                                                    rotation: Quaternion.identity);
                            enemiesSpawnedList[i].Add(go);
                            int tmpId = stageList[i].id;
                            go.GetComponent<Enemy>().OnDie += () => 
                            {
                                int tmpIdx = stageList.FindIndex(stageInfo => stageInfo.id == tmpId);

                                enemiesSpawnedList[tmpIdx].Remove(go); 
                                if (enemiesSpawnedList[tmpIdx].Count == 0)
                                {
                                    OnStageFinished(tmpId);
                                    stageList.RemoveAt(tmpIdx);
                                    timersList.RemoveAt(tmpIdx);
                                    delayTimersList.RemoveAt(tmpIdx);
                                    spawnCountersList.RemoveAt(tmpIdx);
                                }
                            };

                            go.GetComponent<EnemyMove>().SetStartEnd(start: spawnPoints[stageList[i].enemySpawnDataList[j].spawnPointIndex],
                                                                     end: goalPoints[stageList[i].enemySpawnDataList[j].goalPointIndex]);

                            timersList[i][j] = stageList[i].enemySpawnDataList[j].term;
                            spawnCountersList[i][j]--;
                            isSpawnFinished = false;
                        }
                    }
                    else
                    {
                        timersList[i][j] -= Time.deltaTime;
                    }
                }
                else
                {
                    delayTimersList[i][j] -= Time.deltaTime;
                }
            }

            if (isSpawnFinished)
            {
                
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private List<StageInfo> stageList = new List<StageInfo>();
    private List<float[]> timersList = new List<float[]>();
    private List<float[]> delayTimersList = new List<float[]>();
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] goalPoints;


    public void StartSpawn(StageInfo stageInfo)
    {
        stageList.Add(stageInfo);
        float[] tmpTimerList = new float[stageInfo.enemySpawnDataList.Count];
        float[] tmpDelayTimersList = new float[stageInfo.enemySpawnDataList.Count];

        for (int i = 0; i < tmpTimerList.Length; i++)
            tmpTimerList[i] = stageInfo.enemySpawnDataList[i].term;

        for (int i = 0; i < tmpDelayTimersList.Length; i++)
            tmpDelayTimersList[i] = stageInfo.enemySpawnDataList[i].delay;

        timersList.Add(tmpTimerList);
        delayTimersList.Add(tmpDelayTimersList);
    }

    private void Update()
    {
        for (int i = 0; i < stageList.Count; i++)
        {
            for (int j = 0; j < stageList[i].enemySpawnDataList.Count; j++)
            {
                if (delayTimersList[i][j] < 0)
                {
                    if (timersList[i][j] < 0)
                    {
                        GameObject go = Instantiate(original: stageList[i].enemySpawnDataList[j].poolElement.prefab, 
                                                    position: spawnPoints[i].position,
                                                    rotation: Quaternion.identity);

                        go.GetComponent<EnemyMove>().SetStartEnd(start: spawnPoints[stageList[i].enemySpawnDataList[j].spawnPointIndex],
                                                                 end  : goalPoints[stageList[i].enemySpawnDataList[j].goalPointIndex]);

                        timersList[i][j] = stageList[i].enemySpawnDataList[j].term;
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
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.UI;
public class EnemySpawner : MonoBehaviour
{
    private List<StageInfo> stageList = new List<StageInfo>();
    private List<float[]> timersList = new List<float[]>();
    private List<float[]> delayTimersList = new List<float[]>();
    private List<int[]> spawnCountersList = new List<int[]>();
    private List<List<GameObject>> enemiesSpawnedList = new List<List<GameObject>>();
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] goalPoints;
    [SerializeField] private SkipButtonUI _skipButtonUIPrefab;
    private SkipButtonUI[] _skipButtonsBuffer = new SkipButtonUI[10];
    public event Action<int> OnStageFinished;
    private Action EnemyDieAction;


    private bool _spawnFinishedTrigger;
    private bool spawnFinishedTrigger
    {
        set
        {
            if (value &&
                _spawnFinishedTrigger == false)
            {
                PopUpSkipButtons();
            }
            _spawnFinishedTrigger = value;
        }
        get
        {
            return _spawnFinishedTrigger;
        }
    }

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

    public void DestroyAllSkipButtons()
    {
        for (int i = 0; i < _skipButtonsBuffer.Length; i++)
        {
            if (_skipButtonsBuffer[i] != null)
                Destroy(_skipButtonsBuffer[i].gameObject);
        }
    }

    private void Start()
    {
        LevelInfo levelInfo = GamePlay.instance.levelInfo;

        foreach (StageInfo stageInfo in levelInfo.stagesInfo)
        {
            foreach (EnemySpawnData enemySpawnData in stageInfo.enemySpawnDataList)
            {
                ObjectPool.instance.AddPoolElement(enemySpawnData.poolElement);
            }
        }

        ObjectPool.instance.InstantiateAllPoolElements();
    }

    private void Update()
    {
        for (int i = stageList.Count - 1; i >= 0; i--)
        {
            bool tmpSpawnFinished = true;
            for (int j = 0; j < stageList[i].enemySpawnDataList.Count; j++)
            {
                if (spawnCountersList[i][j] > 0)
                {
                    tmpSpawnFinished = false;

                    if (delayTimersList[i][j] < 0)
                    {
                        if (timersList[i][j] < 0)
                        {
                            GameObject go = ObjectPool.instance.Spawn(stageList[i].enemySpawnDataList[j].poolElement.name,
                                                                      spawnPoints[stageList[i].enemySpawnDataList[j].spawnPointIndex].position);

                            enemiesSpawnedList[i].Add(go);

                            int tmpId = stageList[i].id;
                            go.GetComponent<Enemy>().OnDie -= EnemyDieAction;
                            EnemyDieAction = () => OnEnemyDie(go, tmpId);
                            go.GetComponent<Enemy>().OnDie += EnemyDieAction;

                            go.GetComponent<EnemyMove>().SetStartEnd(start: spawnPoints[stageList[i].enemySpawnDataList[j].spawnPointIndex],
                                                                     end: goalPoints[stageList[i].enemySpawnDataList[j].goalPointIndex]);

                            timersList[i][j] = stageList[i].enemySpawnDataList[j].term;
                            spawnCountersList[i][j]--;
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

            if (stageList[i].id == GamePlay.instance.currentStageId)
            {
                spawnFinishedTrigger = tmpSpawnFinished;
            }
        }
    }

    private void PopUpSkipButtons()
    {
        LevelInfo levelInfo = GamePlay.instance.levelInfo;
        int currentStage = GamePlay.instance.currentStage;

        // 다음 스테이지 없으면 리턴
        if (currentStage >= levelInfo.stagesInfo.Count - 1)
            return;

        HashSet<int> spawnPointIndexSet = new HashSet<int>();
        foreach (var enemySpawnData in levelInfo.stagesInfo[currentStage + 1].enemySpawnDataList)
        {
            spawnPointIndexSet.Add(enemySpawnData.spawnPointIndex);
        }

        foreach (var index in spawnPointIndexSet)
        {
            _skipButtonsBuffer[index] = Instantiate(_skipButtonUIPrefab,
                                        spawnPoints[index].position + Vector3.up,
                                        _skipButtonUIPrefab.transform.rotation);

            _skipButtonsBuffer[index].AddButtonOnClickListener(() =>
            {
                GamePlay.instance.NextStage();
                DestroyAllSkipButtons();
            });
        }
    }

    private void OnEnemyDie(GameObject go, int id)
    {
        int tmpIdx = stageList.FindIndex(stageInfo => stageInfo.id == id);

        if (tmpIdx >= 0)
        {
            enemiesSpawnedList[tmpIdx].Remove(go);
            if (enemiesSpawnedList[tmpIdx].Count == 0)
            {
                OnStageFinished(id);
                stageList.RemoveAt(tmpIdx);
                timersList.RemoveAt(tmpIdx);
                delayTimersList.RemoveAt(tmpIdx);
                spawnCountersList.RemoveAt(tmpIdx);
            }
        }
    }
}

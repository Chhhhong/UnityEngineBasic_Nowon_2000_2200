using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    public static GamePlay instance;

    public enum States
    {
        Idle,
        SetUpLevel,
        PlayStartEvents,
        WaitForStartEvents,
        PlayStage,
        WaitForStageFinished,
        NextStage,
        LevelCompleted,
        LevelFailed,
        WaitForUser,
    }
    public States state;
    public LevelInfo levelInfo;
    public int currentStage;
    public int currentStageId;
    private Dictionary<int, bool> _stageFinishedPairs;
    private float _nextStageDelay = 0.5f;

    [SerializeField] private EnemySpawner _spawner;

    public void StartLevel()
    {
        if (state == States.Idle)
            state = States.SetUpLevel;
    }

    public void NextStage()
    {
        if (currentStage < levelInfo.stagesInfo.Count - 1 &&
            state == States.WaitForStageFinished)
            state = States.NextStage;
    }

    private void Awake()
    {
        instance = this;
        StartCoroutine(E_Init());
    }

    IEnumerator E_Init()
    {
        yield return new WaitUntil(() => Player.instance);
        Player.instance.SetUp(levelInfo.lifeInit,
                              levelInfo.moneyInit);

        _stageFinishedPairs = new Dictionary<int, bool>();

        for (int i = 0; i < levelInfo.stagesInfo.Count; i++)
            _stageFinishedPairs.Add(levelInfo.stagesInfo[i].id, false);

        _spawner.OnStageFinished += OnStageFinished;

        StartLevel();
    }

    private void Update()
    {
        switch (state)
        {
            case States.Idle:
                break;
            case States.SetUpLevel:
                {
                    Pathfinder.SetNodeMap();
                    state = States.PlayStartEvents;
                }
                break;
            case States.PlayStartEvents:
                {
                    state = States.WaitForStartEvents;
                }
                break;
            case States.WaitForStartEvents:
                {
                    state = States.PlayStage;
                }
                break;
            case States.PlayStage:
                {
                    _spawner.StartSpawn(levelInfo.stagesInfo[currentStage]);
                    currentStageId = levelInfo.stagesInfo[currentStage].id;
                    state = States.WaitForStageFinished;
                }
                break;
            case States.WaitForStageFinished:
                // nothing to do
                break;
            case States.NextStage:
                {
                    currentStage++;
                    state = States.PlayStage;
                }
                break;
            case States.LevelCompleted:
                break;
            case States.LevelFailed:
                break;
            case States.WaitForUser:
                break;
            default:
                break;
        }
    }

    private void MoveNext()
    {
        if (state < States.WaitForUser)
            state++;
    }

    /// <summary>
    /// 스테이지 끝날때 호출되는 콜백
    /// 호출될때마다 레벨도 끝났는지 같이 체크
    /// </summary>
    /// <param name="stageId">스테이지 고유 인식 번호</param>
    private void OnStageFinished(int stageId)
    {
        // stage id 유효한지 체크
        if (_stageFinishedPairs.TryGetValue(stageId, out bool isFinished) &&
            isFinished == false)
        {
            _stageFinishedPairs[stageId] = true;

            // 레벨 끝났는지 체크
            if (IsLevelFinished())
            {
                OnLevelFinished();
            }
            // 현재 끝나기를 기다리던 스테이지가 끝난거면 다음 스테이지 진행
            else if (stageId == currentStageId)
            {
                _spawner.DestroyAllSkipButtons();
                Invoke("NextStage", _nextStageDelay);
            }
        }
    }

    private bool IsLevelFinished()
    {
        bool isFinished = true;
        foreach (var pair in _stageFinishedPairs)
        {
            if (pair.Value == false)
                isFinished = false;
        }
        return isFinished;
    }

    private void OnLevelFinished()
    {
        state = States.LevelCompleted;
    }

    
}

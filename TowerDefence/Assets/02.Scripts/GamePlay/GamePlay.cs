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
    [SerializeField] private EnemySpawner _spawner;

    public void StartLevel()
    {
        if (state == States.Idle)
            state = States.SetUpLevel;
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
                    state = States.WaitForStageFinished;
                }
                break;
            case States.WaitForStageFinished:
                break;
            case States.NextStage:
                {
                    // 다음 스테이지 없으면 레벨 끝
                    if (currentStage >= levelInfo.stagesInfo.Count - 1)
                        state = States.LevelCompleted;
                    else
                    {
                        currentStage++;
                        state = States.PlayStage;
                    }
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
}

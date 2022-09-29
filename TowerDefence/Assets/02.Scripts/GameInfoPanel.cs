using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public class GameInfoPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _life;
    [SerializeField] private TMP_Text _money;
    [SerializeField] private TMP_Text _level;
    [SerializeField] private TMP_Text _stage;

    private void Awake()
    {
        StartCoroutine(E_Init());
    }

    IEnumerator E_Init()
    {
        yield return new WaitUntil(() => Player.instance);
        Player.instance.OnMoneyChanged += SetMoneyText;
        Player.instance.OnLifeChanged += SetLifeText;
    }

    private void SetMoneyText(int money)
    {
        _money.text = money.ToString();
    }

    private void SetLifeText(int life)
    {
        _life.text = life.ToString();
    }

    private void SetLevelText(int level)
    {
        _level.text = level.ToString();
    }

    private void SetStageText(int stage)
    {
        _stage.text = stage.ToString();
    }
}

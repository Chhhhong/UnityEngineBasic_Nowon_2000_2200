using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelectButton : MonoBehaviour
{
    [SerializeField] private TowerInfo _towerInfo;
    private Button _button;

    public void OnClick()
    {
        TowerHandler.instance.SetTower(_towerInfo);
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
        StartCoroutine(E_Init());
    }

    IEnumerator E_Init()
    {
        yield return new WaitUntil(() => Player.instance);
        Player.instance.OnMoneyChanged += OnMoneyChanged;
    }

    private void OnMoneyChanged(int money)
    {
        _button.interactable = _towerInfo.buildPrice <= Player.instance.money;
    }
}

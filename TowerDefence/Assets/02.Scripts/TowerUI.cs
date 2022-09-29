using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TowerUI : MonoBehaviour
{
    public static TowerUI instance;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Button _sellButton;
    [SerializeField] private TMP_Text _upgradeCostText;
    [SerializeField] private Vector3 _offset = Vector3.up * 3.0f;
    private int _upgradeCost;
    private bool _upgradeAffordable => _upgradeCost <= Player.instance.money;

    public void SetUp(Tower tower)
    {
        // Upgrade 버튼 세팅
        if (TowerAssets.instance.TryGetTower(towerName: $"{tower.info.type}{tower.info.upgradeLevel + 1}",
                                             tower: out GameObject nextLevelTowerPrefab))
        {
            Tower nextLevelTower = nextLevelTowerPrefab.GetComponent<Tower>();
            _upgradeButton.gameObject.SetActive(true);
            _upgradeCost = nextLevelTower.info.buildPrice;
            _upgradeCostText.text = nextLevelTower.info.buildPrice.ToString();

            if (_upgradeAffordable) 
                _upgradeCostText.color = Color.black;
            else 
                _upgradeCostText.color = Color.red;

            _upgradeButton.onClick.RemoveAllListeners();
            _upgradeButton.onClick.AddListener(() => {

                if (_upgradeAffordable)
                {
                    Player.instance.money -= _upgradeCost;
                    SetUp(Upgrade(tower, nextLevelTower));
                }
            });
        }
        else
        {
            _upgradeButton.gameObject.SetActive(false);
        }

        // Sell 버튼 세팅
        _sellButton.onClick.RemoveAllListeners();
        _sellButton.onClick.AddListener(() =>
        {
            Player.instance.money += tower.info.sellPrice;
            tower.node.Clear();
            gameObject.SetActive(false);
        });

        gameObject.SetActive(true);
        transform.position = tower.transform.position + _offset;
    }

    public void Clear()
    {
        _upgradeCost = -1;
    }

    public Tower Upgrade(Tower before, Tower after)
    {
        Tower towerBuilt = null;

        if (before != null)
        {
            Node node = before.node;
            node.Clear();
            node.TryBuildTowerHere($"{after.info.type}{after.info.upgradeLevel}", out towerBuilt);
        }

        return towerBuilt;
    }


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Player.instance.OnMoneyChanged += RefreshUpgradeCostTextColor;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Clear();
    }

    private void RefreshUpgradeCostTextColor(int money)
    {
        if (_upgradeAffordable)
            _upgradeCostText.color = Color.black;
        else
            _upgradeCostText.color = Color.red;
    }
}

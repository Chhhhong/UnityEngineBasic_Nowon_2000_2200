using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerTypes
{
    MachineGun,
    RocketLauncher,
    Laser
}

[CreateAssetMenu(fileName = "TowerInfo", menuName = "TowerDefence/Create TowerInfo")]
public class TowerInfo : ScriptableObject
{
    public TowerTypes type;
    public int upgradeLevel;
    public int buildPrice;
    public int sellPrice;
    public Sprite icon;
}

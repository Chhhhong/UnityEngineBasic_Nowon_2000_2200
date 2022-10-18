using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUnitAssets : MonoBehaviour
{
    private static MapUnitAssets _instance;
    public static MapUnitAssets instance
    {
        get
        {
            if (_instance == null)
                _instance = Instantiate(Resources.Load<MapUnitAssets>("MapUnitAssets"));
            return _instance;
        }
    }

    [SerializeField] private List<MapUnit> _mapUnits;

    public MapUnit GetRandomMapUnit()
    {
        return _mapUnits[Random.Range(0, _mapUnits.Count)];
    }
}

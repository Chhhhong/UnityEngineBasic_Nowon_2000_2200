using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Pos
{
    Left,
    Center,
    Right
}

public class MapSpawner : MonoBehaviour
{
    [SerializeField] private Transform _mapUnitsLeftParent;
    [SerializeField] private Transform _mapUnitsCenterParent;
    [SerializeField] private Transform _mapUnitsRightParent;

    private LinkedList<MapUnit> _mapUnitsLeft = new LinkedList<MapUnit> ();
    private LinkedList<MapUnit> _mapUnitsCenter = new LinkedList<MapUnit> ();
    private LinkedList<MapUnit> _mapUnitsRight = new LinkedList<MapUnit> ();

    private MapUnit _tmpMapUnit;

    private void Awake()
    {
        foreach (MapUnit mapUnit in _mapUnitsLeftParent.GetComponentsInChildren<MapUnit>())
            _mapUnitsLeft.AddLast(mapUnit);

        foreach (MapUnit mapUnit in _mapUnitsCenterParent.GetComponentsInChildren<MapUnit>())
            _mapUnitsCenter.AddLast(mapUnit);

        foreach (MapUnit mapUnit in _mapUnitsRightParent.GetComponentsInChildren<MapUnit>())
            _mapUnitsRight.AddLast(mapUnit);

        foreach (MapUnit mapUnit in _mapUnitsLeft)
        {
            mapUnit.OnReachedToEnd += () =>
            {
                _mapUnitsLeft.Remove(mapUnit);
                Spawn(Pos.Left);
            };
        }

        foreach (MapUnit mapUnit in _mapUnitsCenter)
        {
            mapUnit.OnReachedToEnd += () =>
            {
                _mapUnitsCenter.Remove(mapUnit);
                Spawn(Pos.Center);
            };
        }

        foreach (MapUnit mapUnit in _mapUnitsRight)
        {
            mapUnit.OnReachedToEnd += () =>
            {
                _mapUnitsRight.Remove(mapUnit);
                Spawn(Pos.Right);
            };
        }
    }

    private void Spawn(Pos pos)
    {
        _tmpMapUnit = MapUnitAssets.instance.GetRandomMapUnit();
        MapUnit mapUnit;
        MapUnit lastMapUnit;
        switch (pos)
        {
            case Pos.Left:
                {
                    lastMapUnit = _mapUnitsLeft.Last.Value;
                    mapUnit = Instantiate(_tmpMapUnit,
                                          lastMapUnit.transform.position + Vector3.forward * (lastMapUnit.length / 2.0f + _tmpMapUnit.length / 2.0f),
                                          Quaternion.identity,
                                          _mapUnitsLeftParent);
                    mapUnit.OnReachedToEnd += () =>
                    {
                        _mapUnitsLeft.Remove(mapUnit);
                        Spawn(Pos.Left);
                    };
                    _mapUnitsLeft.AddLast(mapUnit);
                }                
                break;
            case Pos.Center:
                {
                    lastMapUnit = _mapUnitsCenter.Last.Value;
                    mapUnit = Instantiate(_tmpMapUnit,
                                          lastMapUnit.transform.position + Vector3.forward * (lastMapUnit.length / 2.0f + _tmpMapUnit.length / 2.0f),
                                          Quaternion.identity,
                                          _mapUnitsCenterParent);
                    mapUnit.OnReachedToEnd += () =>
                    {
                        _mapUnitsCenter.Remove(mapUnit);
                        Spawn(Pos.Center);
                    };
                    _mapUnitsCenter.AddLast(mapUnit);
                }
                break;
            case Pos.Right:
                {
                    lastMapUnit = _mapUnitsRight.Last.Value;
                    mapUnit = Instantiate(_tmpMapUnit,
                                          lastMapUnit.transform.position + Vector3.forward * (lastMapUnit.length / 2.0f + _tmpMapUnit.length / 2.0f),
                                          Quaternion.identity,
                                          _mapUnitsRightParent);
                    mapUnit.OnReachedToEnd += () =>
                    {
                        _mapUnitsRight.Remove(mapUnit);
                        Spawn(Pos.Right);
                    };
                    _mapUnitsRight.AddLast(mapUnit);
                }
                break;
            default:
                break;
        }
    }
}

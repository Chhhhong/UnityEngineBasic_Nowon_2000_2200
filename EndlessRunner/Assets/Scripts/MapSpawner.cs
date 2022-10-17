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
        Debug.Log($"Spawn {pos}");
    }
}

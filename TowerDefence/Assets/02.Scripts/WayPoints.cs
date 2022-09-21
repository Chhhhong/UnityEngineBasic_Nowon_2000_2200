using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public static WayPoints instance;
    public Transform[] points;

    public bool TryGetNextPoint(int currentPointIndex, out Transform nextPoint)
    {
        nextPoint = null;

        if (currentPointIndex < points.Length - 1)
        {
            nextPoint = points[currentPointIndex + 1];
        }

        return nextPoint;
    }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }
}

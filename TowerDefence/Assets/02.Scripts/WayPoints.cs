using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public static WayPoints instance;
    public Transform[] points;


    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }
}

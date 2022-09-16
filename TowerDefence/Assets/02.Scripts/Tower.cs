using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerInfo info;
    [HideInInspector] public Node node;

    private void OnMouseDown()
    {
        if (TowerHandler.instance.gameObject.activeSelf == false)
            TowerUI.instance.SetUp(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
    public bool isDetected;

    [SerializeField] private LayerMask _wallLayer;

    private void OnTriggerStay(Collider other)
    {
        if ((1<<other.gameObject.layer & _wallLayer) > 0)
            isDetected = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if ((1 << other.gameObject.layer & _wallLayer) > 0)
            isDetected = false;
    }
}

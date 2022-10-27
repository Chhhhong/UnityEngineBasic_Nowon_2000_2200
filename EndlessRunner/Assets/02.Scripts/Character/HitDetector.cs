using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _mapUnitLayer;

    private void OnCollisionEnter(Collision collision)
    {
        if (1 << collision.gameObject.layer == _mapUnitLayer)
        {
            Player.instance.hp -= 10;
        }
    }

}
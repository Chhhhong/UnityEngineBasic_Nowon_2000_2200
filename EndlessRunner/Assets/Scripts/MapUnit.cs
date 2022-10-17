using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUnit : MonoBehaviour
{
    public float length;
    public event Action OnReachedToEnd;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.back * 2.0f * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("MapEnd"))
        {
            OnReachedToEnd?.Invoke();
            Destroy(gameObject);
        }
    }
}

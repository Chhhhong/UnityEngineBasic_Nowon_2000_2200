using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public bool isDetected;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _groundLayer;
    private Transform _tr;

    private void Awake()
    {
        _tr = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        Collider[] grounds = Physics.OverlapSphere(_tr.position + _offset, _radius, _groundLayer);
        if (grounds.Length > 0)
            isDetected = true;
        else
            isDetected = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + _offset, _radius);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool _isGuided; 
    private float _speed;
    protected int damage;
    protected LayerMask touchLayer;
    protected LayerMask targetLayer;
    protected Transform target;
    protected Transform tr;

    public void SetUp(Transform target,
                      float speed, 
                      int damage, 
                      bool isGuided,
                      LayerMask touchLayer, 
                      LayerMask targetLayer)
    {
        this.target = target;
        _speed = speed;
        this.damage = damage;
        _isGuided = isGuided;
        this.touchLayer = touchLayer;
        this.targetLayer = targetLayer;

        tr.LookAt(this.target);
    }

    private void Awake()
    {
        tr = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        if (_isGuided)
            tr.LookAt(target);
        
        tr.Translate(Vector3.forward * _speed * Time.fixedDeltaTime, Space.Self);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (1<<other.gameObject.layer == targetLayer)
        {
            // todo -> damage to target
        }
        else if (1<<other.gameObject.layer == touchLayer)
        {
            // todo -> explode
        }
    }
}

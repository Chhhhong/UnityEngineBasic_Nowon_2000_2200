using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private Transform _tr;

    public float speed = 1.0f;
    private int _wayPointIndex = 0;
    private Transform _nextWayPoint;
    private float _originY;

    private Vector3 _targetPos;
    private Vector3 _dir;
    private float _posTolerance = 0.05f;

    private void Awake()
    {
        _tr = GetComponent<Transform>();
        _originY = _tr.position.y;
    }

    private void Start()
    {
        WayPoints.instance.TryGetNextPoint(_wayPointIndex, out _nextWayPoint);   
    }

    private void FixedUpdate()
    {
        _targetPos = new Vector3(_nextWayPoint.position.x, 
                                        _originY,
                                        _nextWayPoint.position.z);
        _dir = (_targetPos - _tr.position).normalized;

        if (Vector3.Distance(_targetPos, _tr.position) < _posTolerance)
        {
            if (WayPoints.instance.TryGetNextPoint(_wayPointIndex, out _nextWayPoint))
            {
                _wayPointIndex++;                
            }
            else
            {
                OnReachedToEnd();
            }
        }

        _tr.LookAt(_targetPos);
        _tr.Translate(_dir * speed * Time.fixedDeltaTime, Space.World);
    }

    private void OnReachedToEnd()
    {
        Player.instance.life -= 1;
        Destroy(gameObject);
    }
}

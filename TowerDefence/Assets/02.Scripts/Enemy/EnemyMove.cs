using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private Transform _tr;
    private Enemy _enemy;
    public float speed = 1.0f;
    [SerializeField] private float _offsetY;

    private Pathfinder _pathFinder;
    [SerializeField]private Transform _start;
    [SerializeField]private Transform _end;
    private List<Transform> _wayPoints;
    private int _wayPointIndex = 0;
    private Transform _nextWayPoint;
    private float _originY;

    private Vector3 _targetPos;
    private Vector3 _dir;
    private float _posTolerance = 0.05f;

    public void SetStartEnd(Transform start, Transform end)
    {
        _start = start;
        _end = end;
    }


    private void Awake()
    {
        _tr = GetComponent<Transform>();
        _enemy = GetComponent<Enemy>();
        _pathFinder = GetComponent<Pathfinder>();
    }

    private void Start()
    {
        if (_pathFinder.TryFindOptimizedPath(_start, _end, out _wayPoints) == false)
        {
            throw new System.Exception("EnemyMove : 길찾기 실패 !");
        }

        _nextWayPoint = _wayPoints[0];
        _originY = _tr.position.y + _offsetY;
    }

    private void FixedUpdate()
    {
        _targetPos = new Vector3(_nextWayPoint.position.x,
                                 _originY,
                                 _nextWayPoint.position.z);
        _dir = (_targetPos - _tr.position).normalized;

        if (Vector3.Distance(_targetPos, _tr.position) < _posTolerance)
        {
            if (TryGetNextPoint(_wayPointIndex, out _nextWayPoint))
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
        _enemy.Die();
    }

    public bool TryGetNextPoint(int curretPointIndex, out Transform nextPoint)
    {
        nextPoint = null;

        if (curretPointIndex < _wayPoints.Count - 1)
        {
            nextPoint = _wayPoints[curretPointIndex + 1];
        }

        return nextPoint;
    }
}

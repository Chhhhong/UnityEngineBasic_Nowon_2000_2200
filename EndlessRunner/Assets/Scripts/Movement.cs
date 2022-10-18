using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    private Pos _before;
    private Pos _after;
    private Pos _current;
    private Vector3 _leftPos;
    private Vector3 _centerPos;
    private Vector3 _rightPos;
    private float _moveTimer;
    private float _moveTime = 0.1f;
    private bool _doMoveLeft;
    public  bool doMoveLeft
    {
        set
        {
            if (value && 
                _current != Pos.Left)
            {
                _moveTimer = _moveTime;
                _doMoveLeft = value;
            }
            else
            {
                _moveTimer = -1.0f;
            }
        }
    }
    private bool _doMoveRight;
    public bool doMoveRight
    {
        set
        {
            if (value &&
                 _current != Pos.Right)
            {
                _moveTimer = _moveTime;
                _doMoveRight = value;
            }
            else
            {
                _moveTimer = -1.0f;
            }
        }
    }
    private bool isMovable { get; set; }
    public bool isMoving => _doMoveLeft || _doMoveRight;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _centerPos = transform.position;
        _leftPos = transform.position + Vector3.left * 1.5f;
        _rightPos = transform.position + Vector3.right * 1.5f;
        _current = Pos.Center;
        isMovable = true;
    }

    private void Update()
    {
        if (isMovable &&
            isMoving == false)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                doMoveLeft = true;
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                doMoveRight = true;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (_moveTimer < 0)
            return;

        if (_doMoveLeft)
        {
            _rb.MovePosition(Vector3.Lerp(GetVector(_current), GetVector(_current - 1), (1.0f - _moveTimer / _moveTime)));

            _moveTimer = Time.fixedDeltaTime;

            if (_moveTimer < 0)
            {
                _current--;
                _doMoveLeft = false;
            }
        }
        else if (_doMoveRight)
        {
            _rb.MovePosition(Vector3.Lerp(GetVector(_current), GetVector(_current + 1), (1.0f - _moveTimer / _moveTime)));

            _moveTimer = Time.fixedDeltaTime;

            if (_moveTimer < 0)
            {
                _current++;
                _doMoveRight = false;
            }
        }
    }

    private Vector3 GetVector(Pos pos)
    {
        switch (pos)
        {
            case Pos.Left:
                return _leftPos;
            case Pos.Center:
                return _centerPos;
            case Pos.Right:
                return _rightPos;
            default:
                throw new System.Exception("[Movement] : wrong pos");
        }
    }
}

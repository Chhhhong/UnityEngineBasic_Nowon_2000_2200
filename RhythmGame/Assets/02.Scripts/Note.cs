using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public KeyCode keyCode;
    private Transform _tr;
    public float speed;

    public void Hit(HitType hitType)
    {
        Debug.Log($"note hit ! {keyCode}, {hitType}");
        HitTypePopText.instance.hitType = hitType;
        switch (hitType)
        {
            case HitType.Bad:
                ScoringText.instance.score += Constants.SCORE_BAD;
                break;
            case HitType.Miss:
                ScoringText.instance.score += Constants.SCORE_MISS;
                break;
            case HitType.Good:
                ScoringText.instance.score += Constants.SCORE_GOOD;
                break;
            case HitType.Great:
                ScoringText.instance.score += Constants.SCORE_GREAT;
                break;
            case HitType.Perfect:
                ScoringText.instance.score += Constants.SCORE_PERFECT;
                break;
            default:
                break;
        }
        Destroy(gameObject);
    }

    private void Awake()
    {
        _tr = GetComponent<Transform>();    
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _tr.Translate(Vector2.down * speed * Time.fixedDeltaTime);
    }
}
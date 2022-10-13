using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSlowingDown<T> : IBuff<T>
{
    private float _slowGain;

    public BuffSlowingDown(float slowGain)
    {
        _slowGain = slowGain;
    }

    public void OnActive(T target)
    {
        if (target is ISpeed)
        {
            ((ISpeed)target).speed *= _slowGain;
        }
    }

    public void OnDeactive(T target)
    {
        if (target is ISpeed)
        {
            ((ISpeed)target).speed = ((ISpeed)target).speedOrigin; 
        }
    }

    public void OnDuration(T target)
    {
        
    }
}

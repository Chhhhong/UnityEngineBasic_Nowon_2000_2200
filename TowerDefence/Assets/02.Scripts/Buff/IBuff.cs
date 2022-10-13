using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff<T> 
{
    void OnActive(T target);
    void OnDeactive(T target);
    void OnDuration(T target);
}

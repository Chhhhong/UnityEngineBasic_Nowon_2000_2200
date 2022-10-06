using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolRegister : MonoBehaviour
{
    [SerializeField] private List<PoolElement> poolElements;

    private void Awake()
    {
        foreach (var poolElement in poolElements)
        {
            ObjectPool.instance.AddPoolElement(poolElement);
        }
    }
}

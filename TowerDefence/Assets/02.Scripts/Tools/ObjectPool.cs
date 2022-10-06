using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool _instance;
    public static ObjectPool instance
    {
        get
        {
            if (_instance == null)
                _instance = Instantiate(Resources.Load<ObjectPool>("Assets/ObjectPool"));
            return _instance;
        }
    }

    private List<PoolElement> poolElements = new List<PoolElement>();
    private Dictionary<string, Queue<GameObject>> spawnedQueuePairs = new Dictionary<string, Queue<GameObject>>();

    //=========================================================
    //******************* Public Methods **********************
    //=========================================================

    /// <summary>
    /// 필요한 객체와 갯수 발주 
    /// </summary>
    /// <param name="poolElement"></param>
    public void AddPoolElement(PoolElement poolElement)
        => poolElements.Add(poolElement);

    /// <summary>
    /// 발주받은 모든 객체 생성
    /// </summary>
    public void InstantiateAllPoolElements()
    {
        foreach (PoolElement poolElement in poolElements)
        {
            if (spawnedQueuePairs.ContainsKey(poolElement.name) == false)
                spawnedQueuePairs.Add(poolElement.name, new Queue<GameObject>());

            for (int i = 0; i < poolElement.num; i++)
            {
                InstantiatePoolElement(poolElement);
            }
        }
    }

    /// <summary>
    /// 창고에 쌓아놨던 발주품목중에서 하나 꺼내가지고 빌려준다
    /// </summary>
    /// <param name="name">빌려갈 품목 이름</param>
    /// <param name="spawnPoint">배송 위치</param>
    /// <returns></returns>
    public GameObject Spawn(string name, Vector3 spawnPoint)
    {
        if (spawnedQueuePairs.ContainsKey(name) == false)
            return null;

        // 생성해놨던 객체들을 모두 다 빌려줬을때 새로 생성함
        if (spawnedQueuePairs[name].Count <= 0)
        {
            PoolElement poolElement = poolElements.Find(element => element.name == name);
            if (poolElement != null)
            {
                // 원래 소환 갯수에 비례해서 많이 미리 생성해놓자
                for (int i = 0; i < Math.Ceiling(Math.Log10(poolElement.num)); i++)
                {
                    InstantiatePoolElement(poolElement);
                }
            }
        }

        GameObject go = spawnedQueuePairs[name].Dequeue();
        go.transform.position = spawnPoint;
        go.transform.rotation = Quaternion.identity;
        go.transform.SetParent(null);
        go.SetActive(true);
        return go;
    }

    /// <summary>
    /// 다쓴거 창고 반납
    /// </summary>
    /// <param name="obj">반납할 상품</param>
    public void Return(GameObject obj)
    {
        if (spawnedQueuePairs.ContainsKey(obj.name) == false)
        {
            Debug.LogError($"[ObjectPool] : {obj.name} 는 왜가져왔어? 내가 빌려준적이 없는데?");
            return;
        }

        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        spawnedQueuePairs[obj.name].Enqueue(obj);
        obj.SetActive(false);
        RearrangeSiblings(obj);
    }

    //=========================================================
    //******************* Private Methods *********************
    //=========================================================

    private void Awake()
    {
        transform.position = new Vector3(5000, 5000, 5000);
    }

    private GameObject InstantiatePoolElement(PoolElement poolElement)
    {
        GameObject go = Instantiate(poolElement.prefab, transform);
        go.name = poolElement.name;
        spawnedQueuePairs[poolElement.name].Enqueue(go);
        go.SetActive(false);
        RearrangeSiblings(go);
        return go;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj">정렬하고 싶은 자식</param>
    private void RearrangeSiblings(GameObject obj)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == obj.name)
            {
                obj.transform.SetSiblingIndex(i);
                return;
            }
        }

        obj.transform.SetAsLastSibling();
    }
}

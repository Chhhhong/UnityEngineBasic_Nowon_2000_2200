using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemSpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private List<GameObject> _spawnItems;

    private void OnEnable()
    {
        GameObject item = Instantiate(_spawnItems[Random.Range(0, _spawnItems.Count)], _spawnPoint);
    }
}

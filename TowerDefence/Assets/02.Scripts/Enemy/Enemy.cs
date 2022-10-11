using System;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private int _hp;
    public int hp
    {
        get
        {
            return _hp;
        }
        set
        {
            if (value < 0)
                value = 0;

            _hp = value;
            _hpBar.value = (float)_hp / hpMax;

            if (_hp <= 0)
                Die();
        }
    }
    public int hpMax;
    [SerializeField] private Slider _hpBar;
    public event Action OnDie;

    public void Die()
    {
        OnDie();
        Destroy(gameObject);
    }

    private void Awake()
    {
        hp = hpMax;
    }
}
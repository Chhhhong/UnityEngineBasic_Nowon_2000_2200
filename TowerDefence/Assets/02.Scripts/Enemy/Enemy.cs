using System;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour , IHp, ISpeed
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
            OnHPChanged?.Invoke(_hp);

            if (_hp <= 0)
                Die();
        }
    }

    private float _speed;
    public float speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
            OnSpeedChanged?.Invoke(_speed);
        }
    }

    public float speedOrigin { get; private set; }

    public int hpMax;
    [SerializeField] private Slider _hpBar;
    public event Action OnDie;
    public event Action<int> OnHPChanged;
    public event Action<float> OnSpeedChanged;

    public BuffManager<Enemy> buffManager;

    public void Die()
    {
        OnDie();
        buffManager.DeactiveAllBuffs();
        ObjectPool.instance.Return(gameObject);
    }

    private void Awake()
    {
        hp = hpMax;
        speedOrigin = 1.0f;
        speed = speedOrigin;

        buffManager = new BuffManager<Enemy>(this);
    }

    private void OnDisable()
    {
        buffManager.DeactiveAllBuffs();
    }
}
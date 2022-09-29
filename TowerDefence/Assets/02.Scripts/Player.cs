using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Player : MonoBehaviour
{
    public static Player instance;

    private int _life;
    public int life
    {
        get
        {
            return _life;
        }
        set
        {
            _life = value;
            OnLifeChanged(_life);
        }
    }
    private int _money;
    public int money
    {
        get
        {
            return _money;
        }
        set
        {   
            _money = value;
            OnMoneyChanged(_money);
        }
    }
    public event Action<int> OnMoneyChanged;
    public event Action<int> OnLifeChanged;

    public void SetUp(int life, int money)
    {
        this.life = life;
        this.money = money;
    }

    private void Awake()
    {
        instance = this;
    }
}

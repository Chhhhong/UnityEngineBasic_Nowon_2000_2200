using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public static Player Instance;
    // ���⿡�ٰ� �÷��̾� HP �� �����ϴ� HP ������Ƽ �� ������� �߰�,

    private float _hp;
    public float hp
    {
        set
        {
            if (value < 0)
                value = 0;

            _hp = value;
            _hpSlider.value = _hp / _hpMax;

            if (_hp <= 0)
                GameManager.instance.GameOver();
        }

        get
        {
            return _hp;
        }
    }

    [SerializeField] private Slider _hpSlider;
    [SerializeField] public float _hpMax;
    
    public void RecoverHP()
    {
        hp = _hpMax;
    }
    
    private void Awake()
    {
        hp = _hpMax;
        Instance = this;
    }


   
}

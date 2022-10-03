using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class SkipButtonUI : MonoBehaviour
{
    [SerializeField] private Button _button;

    public void AddButtonOnClickListener(UnityAction call)
    {
        _button.onClick.AddListener(call);
    }
}

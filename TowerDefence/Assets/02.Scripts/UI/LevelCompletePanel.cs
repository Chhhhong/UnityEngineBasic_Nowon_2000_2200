using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelCompletePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _level;
    [SerializeField] private Transform _star1;
    [SerializeField] private Transform _star2;
    [SerializeField] private Transform _star3;
    [SerializeField] private Button _BackToLobbyButton;
    [SerializeField] private Button _ReplayButton;
    [SerializeField] private Button _NextButton;

    public void SetUp(int level, float lifeRatio, UnityAction buttonAction)
    {
        _level.text = level.ToString();
        StartCoroutine(E_StarAnimation(lifeRatio));
        _BackToLobbyButton.onClick.AddListener(buttonAction);
        _ReplayButton.onClick.AddListener(buttonAction);
        gameObject.SetActive(true);
    }

    IEnumerator E_StarAnimation(float lifeRatio)
    {
        yield return new WaitForSeconds(0.3f);
        if (lifeRatio >= 1.0f / 3.0f)
            _star1.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        if (lifeRatio >= 2.0f / 3.0f)
            _star2.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        if (lifeRatio >= 3.0f / 3.0f)
            _star3.GetChild(0).gameObject.SetActive(true);
    }

}

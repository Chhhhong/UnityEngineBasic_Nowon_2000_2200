using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager<T> where T : MonoBehaviour
{
    private T _subject;
    private Dictionary<IBuff<T>, Coroutine> _buffs;
    private Coroutine _tmpCoroutine;

    public BuffManager(T subject)
    {
        _subject = subject;
        _buffs = new Dictionary<IBuff<T>, Coroutine>();
    }

    public bool IsBuffExist(IBuff<T> buff) => _buffs.ContainsKey(buff);

    public void ActiveBuff(IBuff<T> buff, float duration)
    {
        if (_subject == null ||
            _subject.gameObject.activeSelf == false)
            return;

        if (_buffs.TryGetValue(buff, out _tmpCoroutine))
        {
            _subject.StopCoroutine(_tmpCoroutine);
            _buffs[buff] = _subject.StartCoroutine(E_ActiveBuff(buff, duration));
        }
        else
        {
            _buffs.Add(buff, _subject.StartCoroutine(E_ActiveBuff(buff, duration)));
        }

    }

    public void DeactiveBuff(IBuff<T> buff)
    {
        if (_buffs.TryGetValue(buff, out _tmpCoroutine))
        {
            _subject.StopCoroutine(_tmpCoroutine);
            _buffs.Remove(buff);
        }
    }

    public void DeactiveAllBuffs()
    {
        foreach (Coroutine coroutine in _buffs.Values)
        {
            _subject.StopCoroutine(coroutine);
        }
        _buffs.Clear();
    }

    private IEnumerator E_ActiveBuff(IBuff<T> buff, float duration)
    {
        buff.OnActive(_subject);

        float timer = duration;
        while (timer > 0)
        {
            buff.OnDuration(_subject);
            timer -= Time.deltaTime;
            yield return null;
        }

        buff.OnDeactive(_subject);

        _buffs[buff] = null;
        _buffs.Remove(buff);
    }
}

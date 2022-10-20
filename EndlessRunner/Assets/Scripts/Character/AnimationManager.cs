using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator _animator;

    public void Play(string clipName) => _animator.Play(clipName);
    public void SetBool(string name, bool value) => _animator.SetBool(name, value);
    public void SetFloat(string name, float value) => _animator.SetFloat(name, value);
    public void GetBool(string name) => _animator.GetBool(name);
    public void GetFloat(string name) => _animator.GetFloat(name);

    public float GetNormalizedTime()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}

using System;
using Unity.Animations.SpringBones.GameObjectExtensions;
using UnityEngine;

public class StateSlide<T> : StateBase<T> where T : Enum
{
    private CapsuleCollider _col;
    private Vector3 _colCenterOrigin;
    private float _colHeightOrigin;
    private float _colShrinkRate = 0.25f;
    public StateSlide(StateMachineBase<T> stateMachine, T machineState, T canExecuteConditionMask, T nextTarget) 
        : base(stateMachine, machineState, canExecuteConditionMask, nextTarget)
    {
        _col = stateMachine.owner.gameObject.FindChildByName("Collision").GetComponent<CapsuleCollider>();
        _colCenterOrigin = _col.center;
        _colHeightOrigin = _col.height;
    }

    public override void Execute()
    {
        base.Execute();
        _col.center = _colCenterOrigin * _colShrinkRate;
        _col.height = _colHeightOrigin * _colShrinkRate;
    }

    public override T Update()
    {
        T next = machineState;

        switch (current)
        {
            case IState<T>.Commands.Idle:
                break;
            case IState<T>.Commands.Prepare:
                {
                    animationManager.SetBool("DoSlide", true);
                    MoveNext();
                }
                break;
            case IState<T>.Commands.Casting:
                {
                    MoveNext();
                }
                break;
            case IState<T>.Commands.WaitForCastingFinished:
                {
                    if (animationManager.isCastingFinished)
                    {
                        animationManager.SetBool("DoSlide", false);
                        MoveNext();
                    }
                }
                break;
            case IState<T>.Commands.Action:
                {
                    MoveNext();
                }
                break;
            case IState<T>.Commands.WaitForActionFinished:
                {


                    if (animationManager.GetNormalizedTime() >= 0.9f)
                        MoveNext();
                }
                break;
            case IState<T>.Commands.Finish:
                {
                    MoveNext();
                }
                break;
            case IState<T>.Commands.WaitForFinished:
                {
                    next = nextTarget;
                }
                break;
            default:
                break;
        }

        return next;
    }

    public override void Reset()
    {
        base.Reset();
        animationManager.SetBool("DoSlide", false);
        _col.center = _colCenterOrigin;
        _col.height = _colHeightOrigin;
    }
}

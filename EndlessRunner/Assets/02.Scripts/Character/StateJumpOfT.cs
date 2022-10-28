using System;
using UnityEngine;
public class StateJump<T> : StateBase<T> where T : Enum
{
    private GroundDetector _groundDetector;
    private Rigidbody _rb;
    private CharacterBase _character;
    public StateJump(StateMachineBase<T> stateMachine, T machineState, T canExecuteConditionMask, T nextTarget)
        : base(stateMachine, machineState, canExecuteConditionMask, nextTarget)
    {
        _groundDetector = stateMachine.owner.GetComponentInChildren<GroundDetector>();
        _rb = stateMachine.owner.GetComponent<Rigidbody>();
        _character = stateMachine.owner.GetComponent<CharacterBase>();
    }

    public override bool canExecute => base.canExecute &&
                                       _groundDetector.isDetected;

    public override T Update()
    {
        T next = machineState;

        switch (current)
        {
            case IState<T>.Commands.Idle:
                break;
            case IState<T>.Commands.Prepare:
                {
                    animationManager.SetBool("DoJump", true);
                    _rb.velocity = Vector3.zero;
                    _rb.AddForce(Vector3.up * _character.jumpForce, ForceMode.Impulse);
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
                    if (_groundDetector.isDetected == false)
                    {
                        animationManager.SetBool("DoJump", false);
                        MoveNext();
                    }
                    else if (animationManager.isCastingFinished &&
                             animationManager.GetNormalizedTime() > 0.8f)
                    {
                        current = IState<T>.Commands.Finish;
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
                    if (_groundDetector.isDetected)
                    {
                        MoveNext();
                    }
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
}
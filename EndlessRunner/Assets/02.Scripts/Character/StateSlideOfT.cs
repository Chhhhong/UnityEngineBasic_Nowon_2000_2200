using System;
public class StateSlide<T> : StateBase<T> where T : Enum
{
    public StateSlide(StateMachineBase<T> stateMachine, T machineState, T canExecuteConditionMask, T nextTarget) 
        : base(stateMachine, machineState, canExecuteConditionMask, nextTarget)
    {
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
    }
}

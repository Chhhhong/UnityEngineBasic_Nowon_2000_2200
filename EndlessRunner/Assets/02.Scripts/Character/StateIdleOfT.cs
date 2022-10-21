using System;
public class StateIdle<T> : StateBase<T> where T : Enum
{
    public StateIdle(StateMachineBase<T> stateMachine, T machineState, T canExecuteConditionMask, T nextTarget) 
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
                    animationManager.SetBool("Idle", true);
                    MoveNext();
                }
                break;
            case IState<T>.Commands.Casting:
                MoveNext();
                break;
            case IState<T>.Commands.WaitForCastingFinished:
                {
                    animationManager.SetBool("Idle", false);
                    MoveNext();
                }
                break;
            case IState<T>.Commands.Action:
                {
                    // nothing to do
                }
                break;
            case IState<T>.Commands.WaitForActionFinished:
                MoveNext();
                break;
            case IState<T>.Commands.Finish:
                MoveNext();
                break;
            case IState<T>.Commands.WaitForFinished:
                MoveNext();
                break;
            default:
                break;
        }

        return next;
    }
}

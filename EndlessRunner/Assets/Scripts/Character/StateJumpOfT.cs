using System;
public class StateJump<T> : StateBase<T> where T : Enum
{
    private GroundDetector _groundDetector;
    public StateJump(StateMachineBase<T> stateMachine, T machineState) 
        : base(stateMachine, machineState)
    {
        _groundDetector = stateMachine.owner.GetComponentInChildren<GroundDetector>();
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
                    else if (animationManager.GetNormalizedTime() > 0.5f)
                    {
                        current = IState<T>.Commands.Finish;
                    }
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

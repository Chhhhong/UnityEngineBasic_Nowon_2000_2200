using System;
public abstract class StateBase<T> : IState<T> where T : Enum
{
    protected AnimationManager animationManager;
    protected StateMachineBase<T> stateMachine;
    protected T canExecuteConditionMask;
    protected T nextTarget;

    public IState<T>.Commands current { get; protected set; }

    public virtual bool canExecute => canExecuteConditionMask.HasFlag(stateMachine.currentType) &&
                                      animationManager.isPreviousStateHasFinished;

    public T machineState { get; private set; }

    public StateBase(StateMachineBase<T> stateMachine,
                     T machineState,
                     T canExecuteConditionMask,
                     T nextTarget)
    {
        this.stateMachine = stateMachine;
        this.machineState = machineState;
        this.canExecuteConditionMask = canExecuteConditionMask;
        this.nextTarget = nextTarget;
        animationManager = stateMachine.owner.GetComponent<AnimationManager>();
    }

    public virtual void Execute()
    {
        current = IState<T>.Commands.Prepare;
    }

    public virtual T Update()
    {
        T next = machineState;

        switch (current)
        {
            case IState<T>.Commands.Idle:
                break;
            case IState<T>.Commands.Prepare:
                MoveNext();
                break;
            case IState<T>.Commands.Casting:
                MoveNext();
                break;
            case IState<T>.Commands.WaitForCastingFinished:
                MoveNext();
                break;
            case IState<T>.Commands.Action:
                MoveNext();
                break;
            case IState<T>.Commands.WaitForActionFinished:
                MoveNext();
                break;
            case IState<T>.Commands.Finish:
                MoveNext();
                break;
            case IState<T>.Commands.WaitForFinished:
                next = nextTarget;
                break;
            default:
                break;
        }

        return next;
    }

    public void MoveNext()
    {
        if (current >= IState<T>.Commands.WaitForFinished)
            throw new System.Exception($"[{this.GetType()}] : not enable to move next");

        current++;
    }

    public virtual void Reset()
    {
        current = IState<T>.Commands.Idle;
    }
}
using System;
public abstract class StateBase<T> : IState<T> where T : Enum
{
    public StateBase(StateMachineBase<T> stateMachine, T machineState)
    {
        this.stateMachine = stateMachine;
        this.machineState = machineState;
    }
    protected StateMachineBase<T> stateMachine;
    public IState<T>.Commands current { get; private set; }

    public virtual bool canExecute => true;

    public T machineState { get; private set; }

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
                MoveNext();
                break;
            default:
                break;
        }

        return next;
    }

    public void MoveNext()
    {
        if (current >= IState<T>.Commands.WaitForCastingFinished)
            throw new System.Exception($"[{this.GetType()}] : not enable to move next");

        current++;
    }

    public void Reset()
    {
        current = IState<T>.Commands.Idle;
    }
}
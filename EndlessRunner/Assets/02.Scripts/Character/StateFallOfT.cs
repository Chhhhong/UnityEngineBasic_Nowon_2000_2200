using System;
public class StateFall<T> : StateBase<T> where T : Enum
{
    public StateFall(StateMachineBase<T> stateMachine, T machineState, T canExecuteConditionMask, T nextTarget) : base(stateMachine, machineState, canExecuteConditionMask, nextTarget)
    {
    }
}
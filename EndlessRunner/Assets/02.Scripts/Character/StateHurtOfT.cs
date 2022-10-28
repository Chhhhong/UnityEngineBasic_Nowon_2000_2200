using System;
public class StateHurt<T> : StateBase<T> where T : Enum
{
    public StateHurt(StateMachineBase<T> stateMachine, T machineState, T canExecuteConditionMask, T nextTarget) 
        : base(stateMachine, machineState, canExecuteConditionMask, nextTarget)
    {
    }
}
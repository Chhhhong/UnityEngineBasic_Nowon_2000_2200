using System;
public class StateHurt<T> : StateBase<T> where T : Enum
{
    public StateHurt(StateMachineBase<T> stateMachine, T machineState) 
        : base(stateMachine, machineState)
    {
    }
}

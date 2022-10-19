using System;
public class StateIdle<T> : StateBase<T> where T : Enum
{
    public StateIdle(StateMachineBase<T> stateMachine, T machineState) 
        : base(stateMachine, machineState)
    {
    }
}

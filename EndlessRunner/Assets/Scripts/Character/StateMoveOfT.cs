using System;
public class StateMove<T> : StateBase<T> where T : Enum
{
    public StateMove(StateMachineBase<T> stateMachine, T machineState) 
        : base(stateMachine, machineState)
    {
    }
}

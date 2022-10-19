using System;
public class StateJump<T> : StateBase<T> where T : Enum
{
    public StateJump(StateMachineBase<T> stateMachine, T machineState) 
        : base(stateMachine, machineState)
    {
    }
}

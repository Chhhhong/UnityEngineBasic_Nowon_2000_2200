using System;
public class StateWallRun<T> : StateBase<T> where T : Enum
{
    public StateWallRun(StateMachineBase<T> stateMachine, T machineState) 
        : base(stateMachine, machineState)
    {
    }
}

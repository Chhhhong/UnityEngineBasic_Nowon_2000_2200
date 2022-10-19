using System;
public class StateSlide<T> : StateBase<T> where T : Enum
{
    public StateSlide(StateMachineBase<T> stateMachine, T machineState) 
        : base(stateMachine, machineState)
    {
    }
}

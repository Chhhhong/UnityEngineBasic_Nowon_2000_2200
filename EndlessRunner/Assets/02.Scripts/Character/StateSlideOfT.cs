using System;
public class StateSlide<T> : StateBase<T> where T : Enum
{
    public StateSlide(StateMachineBase<T> stateMachine, T machineState, T canExecuteConditionMask, T nextTarget) 
        : base(stateMachine, machineState, canExecuteConditionMask, nextTarget)
    {
    }
}

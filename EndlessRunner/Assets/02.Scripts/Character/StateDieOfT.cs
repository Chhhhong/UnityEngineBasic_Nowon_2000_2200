using System;
using System.Diagnostics;

public class StateDie<T> : StateBase<T> where T : Enum
{
    public StateDie(StateMachineBase<T> stateMachine, T machineState, T canExecuteConditionMask, T nextTarget) 
        : base(stateMachine, machineState, canExecuteConditionMask, nextTarget)
    {
    }

    public override void Execute()
    {
        base.Execute();
        UnityEngine.Debug.Log("DieStateExecuted");
    }
} 
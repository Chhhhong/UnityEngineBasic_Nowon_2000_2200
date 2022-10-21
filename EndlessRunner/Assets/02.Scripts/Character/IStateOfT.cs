using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IState<T> where T : Enum
{
    public enum Commands
    {
        Idle,
        Prepare,
        Casting,
        WaitForCastingFinished,
        Action,
        WaitForActionFinished,
        Finish,
        WaitForFinished,
    }
    public Commands current { get; }
    public bool canExecute { get; }
    public T machineState { get; }

    public void Execute();
    public void Reset();
    public T Update();
    public void MoveNext();
}

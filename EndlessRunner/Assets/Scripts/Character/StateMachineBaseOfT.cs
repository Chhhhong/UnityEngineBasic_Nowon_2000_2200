using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class StateMachineBase<T> where T : Enum
{
    public GameObject owner;
    public bool isReady;
    public T currentType;
    public IState<T> current;
    protected Dictionary<T, IState<T>> states;

    public StateMachineBase(GameObject owner)
    {
        this.owner = owner;
        InitStates();
    }

    public void ChangeState(T newType) 
    {
        if (EqualityComparer<T>.Default.Equals(currentType, newType))
            return;
        
        if (states[newType].canExecute)
        {
            current.Reset();
            states[newType].Execute();
            current = states[newType];
            currentType = newType;
        }


    }

    public void Update()
    {
        if (isReady)
            ChangeState(current.Update());
    }


    private void InitStates()
    {
        Array values = Enum.GetValues(typeof(T));
        foreach (T value in values)
        {
            string typeName = "State" + value.ToString();
            Assembly stateTypeAssembly = typeof(T).Assembly;
            Type stateType = Type.GetType($"{typeName}`1[{typeof(T)}{stateTypeAssembly}]");
            ConstructorInfo constructorInfo 
                = stateType.GetConstructor(new Type[] { this.GetType(), typeof(T) });
            if (constructorInfo != null)
            {
                constructorInfo.Invoke(new object[] { this, value });
            }
        }
        isReady = true;
    }
}

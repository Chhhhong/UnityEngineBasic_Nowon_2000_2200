using System;
using System.Collections.Generic;
using System.Reflection;

public class StateMachineBase<T> where T : Enum
{
    public bool isReady;
    public T currentType;
    public StateBase<T> current;
    protected Dictionary<T, StateBase<T>> states;

    public StateMachineBase()
    {
        InitStates();
    }

    public void ChangeState(T newType) 
    {
        if (Enum.Equals(currentType, newType))
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
            Type generic = Type.GetType(typeName + "<>");
            Assembly assembly = Assembly.GetAssembly(generic);
            Type stateType = assembly.GetType($"{typeName}`1[{value}]");
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

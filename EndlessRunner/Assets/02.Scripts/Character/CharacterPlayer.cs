using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayer : CharacterBase
{
    [Flags]
    public enum StateTypes
    {
        Idle =    0 << 0,
        Move =    1 << 0,
        Jump =    1 << 1,
        Slide =   1 << 3,
        WallRun = 1 << 4,
        Hurt =    1 << 5,
        Die =     1 << 6,
        All = ~Idle
    }
    private StateMachineBase<StateTypes> _machine;
    [SerializeField] StateTypes _currentType => _machine.currentType;
    [SerializeField] IState<StateTypes>.Commands _currentCommand => _machine.current.current;

    private void Awake()
    {
        _machine = new StateMachineBase<StateTypes>(gameObject,
                                                    GetStateExecuteConditionMask(),
                                                    GetStateTransitionPairs());
    }

    private void Update()
    {
        Debug.Log($"{_currentType}, {_currentCommand}");
        _machine.Update();
    }

    private Dictionary<StateTypes, StateTypes> GetStateExecuteConditionMask()
    {
        Dictionary<StateTypes, StateTypes> result = new Dictionary<StateTypes, StateTypes>();
        result.Add(StateTypes.Idle, StateTypes.All);
        result.Add(StateTypes.Move, StateTypes.All);
        result.Add(StateTypes.Jump, StateTypes.Idle | StateTypes.Move);
        result.Add(StateTypes.Slide, StateTypes.Idle | StateTypes.Move);
        result.Add(StateTypes.WallRun, StateTypes.Jump);
        result.Add(StateTypes.Hurt, StateTypes.All);
        result.Add(StateTypes.Die, StateTypes.All);
        return result;
    }

    private Dictionary<StateTypes, StateTypes> GetStateTransitionPairs()
    {
        Dictionary<StateTypes, StateTypes> result = new Dictionary<StateTypes, StateTypes>();
        result.Add(StateTypes.Idle, StateTypes.Idle);
        result.Add(StateTypes.Move, StateTypes.Move);
        result.Add(StateTypes.Jump, StateTypes.Move);
        result.Add(StateTypes.Slide, StateTypes.Move);
        result.Add(StateTypes.WallRun, StateTypes.Move);
        result.Add(StateTypes.Hurt, StateTypes.Move);
        result.Add(StateTypes.Die, StateTypes.Move);
        return result;
    }
}
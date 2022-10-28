using System;
using System.Collections.Generic;
using Unity.Animations.SpringBones.GameObjectExtensions;
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
    [SerializeField] StateTypes _currenType => _machine.currentType;
    [SerializeField] IState<StateTypes>.Commands _currentCommand => _machine.current.current;

    [Header("Detectors")]
    [SerializeField] private GroundDetector _groundDetector;
    [SerializeField] private WallDetector _wallDetector_L;
    [SerializeField] private WallDetector _wallDetector_R;

    [Header("Movement")]
    [SerializeField] private Movement _movement;
    //==============================================================================
    //****************************** Public Methods ********************************
    //==============================================================================

    public void StartMove()
    {
        _machine.ChangeState(StateTypes.Move);
    }
    
    public void ChangeMachineState(StateTypes newStateType)
    {
        _machine.ChangeState(newStateType);
    }

    //==============================================================================
    //****************************** Private Methods *******************************
    //==============================================================================

    private void Awake()
    {
        _machine = new StateMachineBase<StateTypes>(gameObject,
                                                    GetStateExecuteConditionMask(),
                                                    GetStateTransitionPairs());
        RegisterAllKeyActions();
    }

    private void RegisterAllKeyActions()
    {
        InputHandler.RegisterKeyDownAction(InputHandler.SHORTCUT_PLAYER_JUMP, 
                                           () => _machine.ChangeState(StateTypes.Jump));
        InputHandler.RegisterKeyDownAction(InputHandler.SHORTCUT_PLAYER_SLIDE,
                                           () => _machine.ChangeState(StateTypes.Slide));

        
        InputHandler.RegisterKeyDownAction(InputHandler.SHORTCUT_PLAYER_MOVE_LEFT,
                                           () =>
                                           {
                                               if (_groundDetector.isDetected == false &&
                                                   _wallDetector_L.isDetected == true)
                                               {
                                                   _machine.ChangeState(StateTypes.WallRun);
                                               }
                                               else
                                               {
                                                   _movement.doMoveLeft = true;
                                               }
                                           });
        InputHandler.RegisterKeyDownAction(InputHandler.SHORTCUT_PLAYER_MOVE_RIGHT,
                                           () =>
                                           {
                                               if (_groundDetector.isDetected == false &&
                                                   _wallDetector_R.isDetected == true)
                                               {
                                                   _machine.ChangeState(StateTypes.WallRun);
                                               }
                                               else
                                               {
                                                   _movement.doMoveRight = true;
                                               }
                                           });


    }

    private void Update()
    {
        Debug.Log($"{_currenType}, {_currentCommand}");
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
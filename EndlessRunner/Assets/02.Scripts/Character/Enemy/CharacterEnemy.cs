using BT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterEnemy : CharacterBase
{
    [Flags]
    public enum StateTypes
    {
        Idle   = 0 << 0,
        Move   = 1 << 0,
        Jump   = 1 << 1,
        Attack = 1 << 2,
        Hurt   = 1 << 3,
        Die    = 1 << 4,
        All    = ~Idle,
    }
    private StateMachineBase<StateTypes> _machine;
    [SerializeField] private StateTypes _currentType => _machine.currentType;
    [SerializeField] private IState<StateTypes>.Commands _currentCommand => _machine.current.current;

    [SerializeField] private GroundDetector _groundDetector;

    public LayerMask _targetLayer;
    public float _detectRange;
    public float _detectAttackRange;

    public class BehaviorTreeForEnemy : BehaviorTree
    {
        private CharacterEnemy _owner;

        public class Detect : Execution
        {
            private Collider[] _tmp;
            public Detect(Func<ReturnTypes> function, Vector3 center, float detectRange, LayerMask targetLayer)
                : base(function)
            {
                function += () =>
                {
                    _tmp = Physics.OverlapSphere(center, detectRange, targetLayer);
                    if (_tmp != null &&
                        _tmp.Length > 0)
                    {
                        return ReturnTypes.Success;
                    }
                    else
                    {
                        return ReturnTypes.Failure;
                    }
                };
            }
        }
        public class Look : Execution
        {
            public Look(Func<ReturnTypes> function, Transform owner, Transform target) : base(function)
            {
                function += () =>
                {
                    if (target == null)
                        return ReturnTypes.Failure;
                    else
                    {
                        owner.LookAt(target);
                        return ReturnTypes.Success;
                    }
                };
            }
        }

        public override RootNode Root { get; set; }
        public Selector SelectorForTarget;
        public Sequence SequenceWhenTargetDetected;
        public ConditionNode ConditionPlayerDetected;
        public ConditionNode ConditionMovable;
        public RandomSelector RandomSelectorForMovement;
        public Detect ExecutionDetectPlayer;
        public Execution ExecutionLookPlayer;
        public Detect ExecutionDetectPlayerInAttackRange;
        public Execution ExecutionAttack;

        public BehaviorTreeForEnemy(CharacterEnemy owner)
        {
            _owner = owner;
        }

        public override void Init()
        {
            Root = new RootNode();

            ExecutionDetectPlayer = new Detect(null, _owner.transform.position, _owner._detectRange, _owner._targetLayer);
            ExecutionDetectPlayer = new Detect(null, _owner.transform.position, _owner._detectAttackRange, _owner._targetLayer);
        }

        public override ReturnTypes Tick()
        {
            return Root.Invoke();
        }
    }
    private BehaviorTreeForEnemy _aiTree;
    public void ChangeMachineState(StateTypes newStateType) => _machine.ChangeState(newStateType);

    private void Awake()
    {
        _machine = new StateMachineBase<StateTypes>(gameObject,
                                                    GetStateExecuteConditionMask(),
                                                    GetStateTransitionPairs());
        _aiTree = new BehaviorTreeForEnemy(this);
    }

    private void Update()
    {
        _aiTree.Tick();
        _machine.Update();
    }

    private Dictionary<StateTypes, StateTypes> GetStateExecuteConditionMask()
    {
        Dictionary<StateTypes, StateTypes> result = new Dictionary<StateTypes, StateTypes>();
        result.Add(StateTypes.Idle, StateTypes.All);
        result.Add(StateTypes.Move, StateTypes.All);
        result.Add(StateTypes.Jump, StateTypes.Idle | StateTypes.Move);
        result.Add(StateTypes.Attack, StateTypes.Idle | StateTypes.Move);
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
        result.Add(StateTypes.Attack, StateTypes.Move);
        result.Add(StateTypes.Hurt, StateTypes.Move);
        result.Add(StateTypes.Die, StateTypes.Move);
        return result;
    }
}

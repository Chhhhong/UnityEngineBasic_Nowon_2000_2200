using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace BehaviorTree
{
    public enum ReturnTypes
    {
        Success,
        Failure,
        OnRunning
    }

    public abstract class Node
    {
        public abstract ReturnTypes Invoke();
    }

    public class Root : Node
    {
        public Node Child;

        public override ReturnTypes Invoke()
        {
            return Child.Invoke();
        }

        public void SetChild(Node child)
        {
            Child = child;
        }
    }


    public class Execution : Node
    {
        public Func<ReturnTypes> Function;

        public Execution(Func<ReturnTypes> function)
        {
            Function = function;
        }

        public override ReturnTypes Invoke()
        {
            return Function.Invoke();
        }
    }

    public abstract class CompositeNode : Node
    {
        public List<Node> Children;
        public void AddChild(Node child)
        {
            Children.Add(child);
        }

        public IEnumerable<Node> GetChildren() => Children;
    }

    public class ConditionNode : Node
    {
        public Node Child;
        public event Func<bool> Condition;


        public ConditionNode(Func<bool> condition)
        {
            Condition = condition;
        }

        public void SetChild(Node child)
        {
            Child = child;
        }
        public override ReturnTypes Invoke()
        {
            if (Condition.Invoke())
            {
                return Child.Invoke();
            }
            return ReturnTypes.Failure;
        }
    }

    public abstract class Decorator : Node
    {
        public Node Child;

        public void SetChild(Node child)
        {
            Child = child;
        }
        public override ReturnTypes Invoke()
        {
            return Decorate(Child.Invoke());
        }

        protected abstract ReturnTypes Decorate(ReturnTypes childReturn);
    }

    public class Repeater : Decorator
    {
        public event Func<bool> Condition;
        protected override ReturnTypes Decorate(ReturnTypes childReturn)
        {
            if (Condition.Invoke())
            {
                return ReturnTypes.OnRunning;
            }
            return childReturn;
        }
    }

    public class Inverter : Decorator
    {
        protected override ReturnTypes Decorate(ReturnTypes childReturn)
        {
            switch (childReturn)
            {
                case ReturnTypes.Success:
                    return ReturnTypes.Failure;
                case ReturnTypes.Failure:
                    return ReturnTypes.Success;
                case ReturnTypes.OnRunning:
                    return ReturnTypes.OnRunning;
                default:
                    throw new Exception("[BehaviorTree_Inverter] : Wrong return type");
            }
        }
    }

    public class Selector : CompositeNode
    {
        private ReturnTypes _tmpResult;

        public override ReturnTypes Invoke()
        {
            foreach (var child in GetChildren())
            {
                _tmpResult = child.Invoke();

                if (_tmpResult == ReturnTypes.Success ||
                    _tmpResult == ReturnTypes.OnRunning)
                {
                    return _tmpResult;
                }
            }
            return ReturnTypes.Failure;
        }
    }

    public class RandomSelector : CompositeNode
    {
        private ReturnTypes _tmpResult;
        public override ReturnTypes Invoke()
        {
            foreach (var child in GetChildren().OrderBy(node => Guid.NewGuid()))
            {
                _tmpResult = child.Invoke();

                if (_tmpResult == ReturnTypes.Success ||
                    _tmpResult == ReturnTypes.OnRunning)
                {
                    return _tmpResult;
                }
            }
            return ReturnTypes.Failure;
        }
    }

    public class Sequence : CompositeNode
    {
        private ReturnTypes _tmpResult;
        public override ReturnTypes Invoke()
        {
            foreach (var child in GetChildren())
            {
                _tmpResult = child.Invoke();

                if (_tmpResult == ReturnTypes.Failure ||
                    _tmpResult == ReturnTypes.OnRunning)
                {
                    return _tmpResult;
                }
            }
            return ReturnTypes.Success;
        }
    }

    public abstract class BehaviorTree : MonoBehaviour
    {
        public Root Root { get; set; }
        public abstract void Init();
        public abstract void Tick();
    }

    public class BehaviorTreeForEnemy : BehaviorTree
    {
        public override Root Root { get; set; }

        public override void Init()
        {
            Root = new Root();

            Selector selector1 = new Selector();
            Root.SetChild(selector1);
            ConditionNode ifPlayerHit = new ConditionNode(() => true);
            ifPlayerHit.SetChild(new Execution(() => ReturnTypes.Success));
            selector1.AddChild(ifPlayerHit);

        }

        public override ReturnTypes Tick()
        {
            return Root.Invoke();
        }

    }
}



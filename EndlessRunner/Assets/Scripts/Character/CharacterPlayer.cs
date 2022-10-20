public class CharacterPlayer : CharacterBase
{
    public enum StateTypes
    {
        Idle,
        Move,
        Jump,
        Fall,
        Slide,
        WallRun,
        Hurt,
        Die
    }
    private StateMachineBase<StateTypes> _machine;

    private void Awake()
    {
        _machine = new StateMachineBase<StateTypes>(this.gameObject);
    }

    private void Update()
    {
        _machine.Update();
    }
}
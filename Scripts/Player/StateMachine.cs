public interface IState
{
    public void Enter();
    public void Exit();
    public void HandleInput();
    public void Update();
    public void PhysicsUpdate();
}
public abstract class StateMachine
{
    protected IState _currentState;

    public void ChangeState(IState state)
    {
        _currentState?.Exit();
        _currentState = state;
        _currentState?.Enter();
    }

    public void HandleInput()
    {
        _currentState?.HandleInput();
    }

    public void Update()
    {
        _currentState?.Update();
    }

    public void PhysicsUpdate()
    {
        _currentState?.PhysicsUpdate();
    }
}
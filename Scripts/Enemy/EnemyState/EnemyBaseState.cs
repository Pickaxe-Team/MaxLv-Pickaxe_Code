public class EnemyBaseState : IState
{
    protected EnemyStateMachine _stateMachine;
    protected readonly EnemyData _enemyData;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this._stateMachine = stateMachine;
        _enemyData = stateMachine.Enemy.EnemyData;
    }
    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void HandleInput()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {

    }
    protected void StartAnimation(int animatorHash)
    {
        _stateMachine.Enemy.Animator.SetBool(animatorHash, true);
    }

    protected void StopAnimation(int animatorHash)
    {
        _stateMachine.Enemy.Animator.SetBool(animatorHash, false);
    }

}

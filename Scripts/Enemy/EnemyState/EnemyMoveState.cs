public class EnemyMoveState : EnemyBaseState
{
    public EnemyMoveState(EnemyStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(_stateMachine.Enemy.AnimationData.WalkParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(_stateMachine.Enemy.AnimationData.WalkParameterHash);
    }

    public override void Update()
    {
        base.Update();

        float playerDistanceSqr = (_stateMachine.Target.transform.position - _stateMachine.Enemy.transform.position).sqrMagnitude;

        if (playerDistanceSqr <= _stateMachine.Enemy.EnemyData.AttackRange * _stateMachine.Enemy.EnemyData.AttackRange)
        {
            _stateMachine.ChangeState(_stateMachine.AttackState);
        }
        else if (playerDistanceSqr > _stateMachine.Enemy.PlayerChasingRange * _stateMachine.Enemy.PlayerChasingRange)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        _stateMachine.Enemy.Agent.SetDestination(_stateMachine.Target.transform.position);
    }
}

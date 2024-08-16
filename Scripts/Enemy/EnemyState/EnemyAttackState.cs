public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {  
        base.Enter();
        StartAnimation(_stateMachine.Enemy.AnimationData.AttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        
        StopAnimation(_stateMachine.Enemy.AnimationData.AttackParameterHash);   
    }

    public override void Update()
    {
        base.Update();
        float playerDistanceSqr = (_stateMachine.Target.transform.position - _stateMachine.Enemy.transform.position).sqrMagnitude;
        if (playerDistanceSqr >= _stateMachine.Enemy.EnemyData.AttackRange * _stateMachine.Enemy.EnemyData.AttackRange)
        {
            _stateMachine.ChangeState(_stateMachine.MoveState);
        }
    }
}

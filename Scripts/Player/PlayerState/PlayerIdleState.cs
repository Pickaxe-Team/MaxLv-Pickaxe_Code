using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(_stateMachine.Player.AnimationData.IdleParameterHash);
        _stateMachine.Player.Input.SetVelocity(Vector2.zero);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(_stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (_stateMachine.MovementInput != Vector2.zero)
        {
            _stateMachine.ChangeState(_stateMachine.WalkState);
        }
    }
}

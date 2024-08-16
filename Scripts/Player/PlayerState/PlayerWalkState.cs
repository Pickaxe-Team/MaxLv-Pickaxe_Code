using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        StartAnimation(_stateMachine.Player.AnimationData.WalkParameterHash);
        AddInputActionCallbacks();
    }

    public override void Exit()
    {
        StopAnimation(_stateMachine.Player.AnimationData.WalkParameterHash);
        RemoveInputActionCallbacks();
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void PhysicsUpdate()
    {
        Move();
    }

    public override void Update()
    {
        base.Update();
        if (_stateMachine.MovementInput == Vector2.zero)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }

    private void Move()
    {
        _stateMachine.Player.Input.Move(_stateMachine.MovementInput, _stateMachine.MovementSpeed, _playerData.SpeedMultiplier);
    }
}

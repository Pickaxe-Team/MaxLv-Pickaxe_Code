using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerBaseState : IState
{
    protected PlayerStateMachine _stateMachine;
    protected readonly PlayerSO _playerData;

    protected Vector2 _currentVelocity;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this._stateMachine = stateMachine;
        _playerData = stateMachine.Player.PlayerSO;
    }

    public virtual void Enter()
    {
        AddInputActionCallbacks();
        _currentVelocity = _stateMachine.Player.Input.GetVelocity();
        _stateMachine.Player.Input.SetVelocity(_currentVelocity);
    }

    public virtual void Exit()
    {
        RemoveInputActionCallbacks();
        _stateMachine.Player.Input.SetVelocity(Vector2.zero);
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
        ReadAttackInput();
    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {
       
    }

    protected virtual void AddInputActionCallbacks()
    {
        PlayerController input = _stateMachine.Player.Input;
        input.PlayerInputActions.Player.Move.performed += OnMovementCanceled;
        input.PlayerInputActions.Player.Attack.performed += OnAttackPerformed;
        input.OnRollInput.AddListener(OnRollButtonPressed);
    }

    protected virtual void RemoveInputActionCallbacks()
    {
        PlayerController input = _stateMachine.Player.Input;
        input.PlayerInputActions.Player.Move.canceled -= OnMovementCanceled;
        input.PlayerInputActions.Player.Attack.performed -= OnAttackPerformed;
        input.OnRollInput.RemoveListener(OnRollButtonPressed);
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context) { }

    protected virtual void OnRollStarted(InputAction.CallbackContext context)
    {
        _stateMachine.ChangeState(_stateMachine.RollState);
    }

    protected void OnRollButtonPressed()
    {
        _stateMachine.ChangeState(_stateMachine.RollState);
    }

    protected virtual void OnAttackPerformed(InputAction.CallbackContext context)
    {
        _stateMachine.ChangeState(_stateMachine.AttackState);
    }

    protected void StartAnimation(int animatorHash)
    {
        _stateMachine.Player.Animator.SetBool(animatorHash, true);
    }

    protected void StopAnimation(int animatorHash)
    {
        _stateMachine.Player.Animator.SetBool(animatorHash, false);
    }

    protected void ReadMovementInput()
    {
        _stateMachine.MovementInput = _stateMachine.Player.Input.PlayerInputActions.Player.Move.ReadValue<Vector2>();
    }
    protected void ReadAttackInput()
    {
        _stateMachine.AttackInput = _stateMachine.Player.Input.PlayerInputActions.Player.Attack.ReadValue<Vector2>();

    }

}

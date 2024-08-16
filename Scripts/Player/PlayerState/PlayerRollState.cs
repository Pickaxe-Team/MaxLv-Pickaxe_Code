using System.Collections;
using UnityEngine;

public class PlayerRollState : PlayerBaseState
{
    private float _rollDuration = 0.3f;
    private float _rollTimer;
    private Vector2 _rollDirection;
    private Ghost _ghost;

    public PlayerRollState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        _ghost = stateMachine.Player.GetComponent<Ghost>();
    }

    public override void Enter()
    {
        StartAnimation(_stateMachine.Player.AnimationData.RollParameterHash);
        _rollTimer = _rollDuration;

        _currentVelocity = _stateMachine.Player.Input.GetVelocity();

        if (_currentVelocity != Vector2.zero)
        {
            _rollDirection = _currentVelocity.normalized;
        }
        else
        {
            _rollDirection = new Vector2(_stateMachine.Player.transform.right.x, _stateMachine.Player.transform.right.y).normalized;
        }

        // 몬스터와의 충돌 무시
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Layer.Player), LayerMask.NameToLayer(Layer.Enemy), true);
        // 플레이어와 보스 총알 간의 충돌 무시
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Layer.Player), LayerMask.NameToLayer(Layer.BossBullet), true);

        Move();

        GameManager.Instance.PlaySFX(SFX.Dash);

        if (_ghost != null)
        {
            _ghost.ToggleGhost(true);
        }
    }

    public override void Exit()
    {
        StopAnimation(_stateMachine.Player.AnimationData.RollParameterHash);
        _stateMachine.Player.Input.SetVelocity(Vector2.zero);

      
        if (_ghost != null)
        {
            _ghost.ToggleGhost(false);
        }
        _stateMachine.Player.Input.StartCoroutine(EndInvincibilityAfterDelay(0.5f));
    }

    public override void Update()
    {
        _rollTimer -= Time.deltaTime;
        if (_rollTimer <= 0f)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }

    private void Move()
    {
        float rollSpeed = _currentVelocity.magnitude * 1.5f;
        _stateMachine.Player.Input.Move(_rollDirection, rollSpeed, _playerData.SpeedMultiplier);
    }
    private IEnumerator EndInvincibilityAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // 몬스터와의 충돌 다시 활성화
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Layer.Player), LayerMask.NameToLayer(Layer.Enemy), false);
        // 플레이어와 보스 총알 간의 충돌 다시 활성화
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(Layer.Player), LayerMask.NameToLayer(Layer.BossBullet), false);
    }
}

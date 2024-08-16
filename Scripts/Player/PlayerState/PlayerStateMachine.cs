using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }
    public Vector2 MovementInput { get; set; }
    public Vector2 AttackInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;
    public PlayerIdleState IdleState { get; }
    public PlayerWalkState WalkState { get; }
    public PlayerRollState RollState { get; }
    public PlayerAttackState AttackState { get; }

    public PlayerStateMachine(Player player)
    {
        this.Player = player;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RollState = new PlayerRollState(this);
        AttackState = new PlayerAttackState(this);
        MovementSpeed = player.PlayerSO.BaseSpeed;
    }

}

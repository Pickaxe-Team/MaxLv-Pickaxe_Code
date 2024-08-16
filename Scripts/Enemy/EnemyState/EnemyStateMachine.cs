public class EnemyStateMachine : StateMachine
{
    public Enemy Enemy { get; }
    public HealthSystem Target { get; private set; }

    public EnemyIdleState IdleState { get; }
    public EnemyMoveState MoveState { get; }
    public EnemyAttackState AttackState { get; }

    public EnemyStateMachine(Enemy enemy)
    {
        this.Enemy = enemy;

        Target = GameManager.Instance.Player.HealthSystem;
        if (Target == null)
        {
            return;
        }

        IdleState = new EnemyIdleState(this);
        MoveState = new EnemyMoveState(this);
        AttackState = new EnemyAttackState(this);
    }
}

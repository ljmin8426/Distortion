public class Enemy : EnemyBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void InitStateMachine()
    {
        stateMachine = new StateMachine<ENEMY_STATE, EnemyBase>(ENEMY_STATE.Idle, new EnemyIdleState(this));
        stateMachine.AddState(ENEMY_STATE.Chase, new EnemyChaseState(this));
        stateMachine.AddState(ENEMY_STATE.Attack, new EnemyAttackState(this));
        stateMachine.AddState(ENEMY_STATE.Hit, new EnemyHitState(this));
        stateMachine.AddState(ENEMY_STATE.Die, new EnemyDieState(this));
    }
}

public abstract class EnemyBaseState
{
    protected EnemyBase enemy;

    public EnemyBaseState(EnemyBase enemy)
    {
        this.enemy = enemy;
    }

    public virtual void OnEnterState() { }
    public virtual void OnExitState() { }
    public virtual void OnUpdateState() { }
    public virtual void OnFixedUpdateState() { }
}

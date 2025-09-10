using System;

public abstract class BaseState<T>
{
    protected T owner { get; private set; }

    public BaseState(T owner)
    {
        this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
    }

    public virtual bool CanEnter() => true;

    public abstract void OnEnterState();
    public abstract void OnUpdateState();
    public abstract void OnFixedUpdateState();
    public abstract void OnExitState();
}

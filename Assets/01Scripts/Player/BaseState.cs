public abstract class BaseState<TOwner>
{
    protected TOwner Owner { get; private set; }

    public BaseState(TOwner controller)
    {
        Owner = controller;
    }

    public abstract void OnEnterState();
    public abstract void OnUpdateState();
    public abstract void OnFixedUpdateState();
    public abstract void OnExitState();
}

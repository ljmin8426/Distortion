public abstract class BaseState<T>
{
    protected T controller { get; private set; }

    public BaseState(T controller)
    {
        this.controller = controller;
    }

    public abstract void OnEnterState();
    public abstract void OnUpdateState();
    public abstract void OnFixedUpdateState();
    public abstract void OnExitState();
}

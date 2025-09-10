using Unity.VisualScripting;
using UnityEngine;

public class AttackState : BaseState<PlayerCtrl>
{
    public AttackState(PlayerCtrl controller) : base(controller) { }

    public override bool CanEnter()
    {
        if (owner.StateMachine.CurrentState is DashState)
            return false;

        return true;
    }

    public override void OnEnterState()
    {
        owner.LookAtCursor();
    }

    public override void OnUpdateState() { }

    public override void OnFixedUpdateState() { }

    public override void OnExitState() { }
}

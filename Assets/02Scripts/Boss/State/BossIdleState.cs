using UnityEngine;

public class BossIdleState : BaseState<BossCtrl>
{
    public BossIdleState(BossCtrl owner) : base(owner) { }

    private bool isDectected = false;

    public override void OnEnterState() { }

    public override void OnUpdateState()
    {
        if(!isDectected)
        {
            if (owner.Target == null) return;

            float distance = Vector3.Distance(owner.transform.position, owner.Target.position);
            if (distance <= owner.DectectionRange)
            {
                isDectected = true;
                owner.Animator.SetBool("isWakeUp", true);
            }
        }
    }

    public override void OnFixedUpdateState() { }

    public override void OnExitState()
    {
        isDectected = false;
    }
}

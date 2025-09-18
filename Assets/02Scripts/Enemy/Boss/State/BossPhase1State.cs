using UnityEngine;

public class BossPhase1State : BaseState<BossCtrl>
{
    public BossPhase1State(BossCtrl owner) : base(owner) { }

    public override void OnEnterState()
    {
        Debug.Log("Boss Phase1 진입");
        owner.Agent.isStopped = false;
    }

    public override void OnUpdateState()
    {
        if (owner.Target == null) return;
    }

    public override void OnFixedUpdateState() { }

    public override void OnExitState()
    {
        Debug.Log("Boss Phase1 종료");
    }
}

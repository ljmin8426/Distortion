using UnityEngine;

public class EnemyDieState : BaseState<EnemyBase>
{
    public EnemyDieState(EnemyBase enemy) : base(enemy) { }

    public override void OnEnterState()
    {
        controller.agent.isStopped = true;
        controller.animator.SetTrigger("IsDie");

        controller.GetComponent<Collider>().enabled = false; 
        controller.enabled = false;

        ExpManager.Instance.GetExp(controller.EnemyData.expReward);
    }

    public override void OnUpdateState() { }
    public override void OnFixedUpdateState() { }
    public override void OnExitState() { }
}

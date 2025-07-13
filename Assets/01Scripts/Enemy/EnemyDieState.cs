using UnityEngine;

public class EnemyDieState : BaseState<EnemyBase>
{
    public EnemyDieState(EnemyBase enemy) : base(enemy) { }

    public override void OnEnterState()
    {
        Owner.agent.isStopped = true;
        Owner.animator.SetTrigger("IsDie");

        AudioManager.instance.PlaySFX("Die");

        Owner.GetComponent<Collider>().enabled = false;
        Owner.enabled = false;

        ExpManager.instance.GetExp(Owner.EnemyData.expReward);
    }

    public override void OnUpdateState() { }
    public override void OnFixedUpdateState() { }
    public override void OnExitState() { }
}

using UnityEngine;

public class EnemyIdleState : BaseState<EnemyBase>
{
    public EnemyIdleState(EnemyBase enemy) : base(enemy) { }

    public override void OnEnterState()
    {
        controller.agent.isStopped = true;
    }

    public override void OnUpdateState()
    {
        float dist = Vector3.Distance(controller.transform.position, controller.player.position);

        if (dist < controller.EnemyData.detectionRange)
        {
            controller.ChangeState(ENEMY_STATE.Chase);
        }
    }

    public override void OnFixedUpdateState() { }
    public override void OnExitState() { }
}

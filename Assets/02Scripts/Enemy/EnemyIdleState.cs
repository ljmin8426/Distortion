using UnityEngine;

public class EnemyIdleState : BaseState<EnemyBase>
{
    private float idleTimer = 0f;
    private float idleDuration = 2f;

    public EnemyIdleState(EnemyBase enemy) : base(enemy) { }

    public override void OnEnterState()
    {
        Owner.agent.isStopped = true;
        idleTimer = 0f;
    }

    public override void OnUpdateState()
    {
        idleTimer += Time.deltaTime;

        float dist = Vector3.Distance(Owner.transform.position, Owner.player.position);

        if (dist < Owner.EnemyData.detectionRange)
        {
            Owner.ChangeState(ENEMY_STATE.Chase);
        }
        else if (idleTimer > idleDuration)
        {
            // Optional: Patrol 상태 전환 가능
            // Controller.stateMachine.ChangeState(ENEMY_STATE.Patrol);
        }
    }

    public override void OnFixedUpdateState() { }
    public override void OnExitState() { }
}

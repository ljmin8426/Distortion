using UnityEngine;

public class EnemyHitState : BaseState<EnemyBase>
{
    private float hitTimer;

    public EnemyHitState(EnemyBase enemy) : base(enemy) { }

    public override void OnEnterState()
    {
        controller.Agent.isStopped = true;
        controller.Animator.SetTrigger("IsHit");
        hitTimer = 0f;
    }

    public override void OnUpdateState()
    {
        hitTimer += Time.deltaTime;
        if (hitTimer >= 1f)
        {
            controller.ChangeState(ENEMY_STATE.Idle);
        }
    }

    public override void OnFixedUpdateState() { }
    public override void OnExitState() { }
}

using UnityEngine;

public class EnemyHitState : BaseState<EnemyBase>
{
    private float hitTimer;

    public EnemyHitState(EnemyBase enemy) : base(enemy) { }

    public override void OnEnterState()
    {
        controller.agent.isStopped = true;
        controller.animator.SetTrigger("IsHit");
        //AudioManager.instance.PlaySFX("Hit");
        hitTimer = 0f;
    }

    public override void OnUpdateState()
    {
        // 0.3 초 동안 스턴
        hitTimer += Time.deltaTime;
        if (hitTimer >= 0.3f)
        {
            controller.ChangeState(ENEMY_STATE.Idle);
        }
    }

    public override void OnFixedUpdateState() { }
    public override void OnExitState() { }
}

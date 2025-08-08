using UnityEngine;

public class EnemyAttackState : BaseState<EnemyBase>
{
    private float attackTimer;

    public EnemyAttackState(EnemyBase enemy) : base(enemy) { }

    public override void OnEnterState()
    {
        controller.agent.isStopped = true;
        attackTimer = 0f;
        controller.animator.SetTrigger("IsAttack");
    }

    public override void OnUpdateState()
    {
        float dist = Vector3.Distance(controller.transform.position, controller.player.position);

        if (dist > controller.EnemyData.attackRange)
        {
            controller.ChangeState(ENEMY_STATE.Chase);
            return;
        }

        attackTimer += Time.deltaTime;

        if (attackTimer >= controller.EnemyData.attackCooldown)
        {
            controller.animator.SetTrigger("IsAttack");
            attackTimer = 0f;
        }
    }

    public override void OnFixedUpdateState() { }

    public override void OnExitState()
    {
        controller.AttackCollider?.DisableCollider(); // 안전하게 끄기
    }
}

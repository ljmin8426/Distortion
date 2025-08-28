using UnityEngine;

public class EnemyAttackState : BaseState<EnemyBase>
{
    private float attackTimer;

    public EnemyAttackState(EnemyBase enemy) : base(enemy) { }

    public override void OnEnterState()
    {
        controller.Agent.isStopped = true;
        attackTimer = 0f;
        controller.Animator.SetTrigger("IsAttack");
    }

    public override void OnUpdateState()
    {
        float dist = Vector3.Distance(controller.transform.position, controller.Player.position);

        if (dist > controller.EnemyData.attackRange)
        {
            controller.ChangeState(ENEMY_STATE.Chase);
            return;
        }

        attackTimer += Time.deltaTime;

        if (attackTimer >= controller.EnemyData.attackCooldown)
        {
            controller.Animator.SetTrigger("IsAttack");
            attackTimer = 0f;
        }
    }

    public override void OnFixedUpdateState() { }

    public override void OnExitState()
    {
        controller.AttackCollider?.DisableCollider();
    }
}

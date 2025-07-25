using UnityEngine;

public class EnemyAttackState : BaseState<EnemyBase>
{
    private float attackTimer;

    public EnemyAttackState(EnemyBase enemy) : base(enemy) { }

    public override void OnEnterState()
    {
        Owner.agent.isStopped = true;
        attackTimer = 0f;
        Owner.animator.SetTrigger("IsAttack");
    }

    public override void OnUpdateState()
    {
        float dist = Vector3.Distance(Owner.transform.position, Owner.player.position);

        // 공격 범위 밖이면 추격 상태로 전환
        if (dist > Owner.EnemyData.attackRange)
        {
            Owner.ChangeState(ENEMY_STATE.Chase);
            return;
        }

        attackTimer += Time.deltaTime;

        if (attackTimer >= Owner.EnemyData.attackCooldown)
        {
            Owner.animator.SetTrigger("IsAttack");
            attackTimer = 0f;
        }
    }

    public override void OnFixedUpdateState() { }

    public override void OnExitState()
    {
        Owner.AttackCollider?.DisableCollider(); // 안전하게 끄기
    }
}

using UnityEngine;

public class MonsterAttackState : BaseState<MonsterBase>
{
    private float lastAttackTime;
    private bool isAttacking; // 공격 모션이 끝날 때까지 상태 유지

    public MonsterAttackState(MonsterBase owner) : base(owner) { }

    public override void OnEnterState()
    {
        owner.Agent.isStopped = true;
        owner.Agent.ResetPath();
        lastAttackTime = -999f;
        isAttacking = false;
    }

    public override void OnUpdateState()
    {
        if (owner.Target == null) return;

        float dist = Vector3.Distance(owner.transform.position, owner.Target.position);

        // 공격 모션 중이면 다른 상태 전환 금지
        if (isAttacking) return;

        // 공격 쿨타임이 지났으면 공격 시작
        if (Time.time >= lastAttackTime + owner.MonsterData.attackSpeed)
        {
            DoAttack();
            lastAttackTime = Time.time;
        }
        else
        {
            // 공격 끝난 뒤에만 Chase 전환
            if (dist > owner.AttackRange)
            {
                owner.ChangeState(ENEMY_STATE.Chase);
            }
        }
    }

    public override void OnFixedUpdateState() { }

    public override void OnExitState()
    {
        // Trigger는 자동으로 리셋되므로 여기선 할 게 없음
        isAttacking = false;
    }

    private void DoAttack()
    {
        isAttacking = true;

        // 플레이어 방향 바라보기
        if (owner.Target != null)
        {
            Vector3 direction = (owner.Target.position - owner.transform.position).normalized;
            direction.y = 0; // 수평 회전만
            if (direction.sqrMagnitude > 0)
            {
                owner.transform.rotation = Quaternion.LookRotation(direction);
            }
        }
        owner.Animator.SetTrigger("isAttack");
    }

    // 애니메이션 중간 이벤트 (데미지 판정)
    public void OnAttackAnimationHit()
    {
        if (owner.Target == null) return;

        var target = owner.Target.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(owner.MonsterData.attackDamage, owner.gameObject);
        }
    }

    // 애니메이션 끝 이벤트에서 호출
    public void OnAttackAnimationEnd()
    {
        isAttacking = false;
    }
}

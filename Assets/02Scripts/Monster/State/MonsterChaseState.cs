using UnityEngine;

public class MonsterChaseState : BaseState<MonsterBase>
{
    public MonsterChaseState(MonsterBase owner) : base(owner) { }

    public override void OnEnterState()
    {
        if (owner.Agent != null)
            owner.Agent.isStopped = false; // 이동 재개
    }

    public override void OnUpdateState()
    {
        if (owner.Target == null) return;

        float distance = Vector3.Distance(owner.transform.position, owner.Target.position);

        // 공격 범위에 들어오면 Attack 상태로 전환
        if (distance <= owner.AttackRange)
        {
            owner.ChangeState(ENEMY_STATE.Attack);
            return;
        }

        // 플레이어 위치로 이동
        if (owner.Agent != null)
        {
            owner.Agent.SetDestination(owner.Target.position);

            // 이동 애니메이션 반영
            if (owner.Animator != null)
                owner.Animator.SetFloat("moveSpeed", owner.Agent.velocity.magnitude);
        }
    }

    public override void OnFixedUpdateState() { }

    public override void OnExitState()
    {
        if (owner.Agent != null)
            owner.Agent.isStopped = true; // 상태 종료 시 이동 정지
    }
}

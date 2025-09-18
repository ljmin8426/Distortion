using UnityEngine;

public class MonsterIdleState : BaseState<MonsterBase>
{
    public MonsterIdleState(MonsterBase owner) : base(owner) { }

    public override void OnEnterState()
    {
        // 이동 정지
        if (owner.Agent != null)
            owner.Agent.isStopped = true;

        // Idle 애니메이션
        if (owner.Animator != null)
            owner.Animator.SetFloat("moveSpeed", 0f);
    }

    public override void OnUpdateState()
    {
        if (owner.Target == null) return;

        // 플레이어 거리 체크
        float distance = Vector3.Distance(owner.transform.position, owner.Target.position);

        // 감지 범위 안이면 Chase 상태로 전환
        if (distance <= owner.DetectionRange)
        {
            owner.ChangeState(Enemy_State.Chase);
        }
    }

    public override void OnFixedUpdateState() { }

    public override void OnExitState()
    {
        // 이동 재개
        if (owner.Agent != null)
            owner.Agent.isStopped = false;
    }
}

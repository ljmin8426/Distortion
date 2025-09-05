using UnityEngine;
using System.Collections;

public class MonsterHitState : BaseState<MonsterBase>
{
    private float hitDuration = 1f; // 스턴 시간

    public MonsterHitState(MonsterBase owner) : base(owner) { }

    public override void OnEnterState()
    {
        Debug.Log("Hit");


        AudioManager.Instance.PlaySoundFXClip(owner.HitSoundClip, owner.transform, 1f);
        // 이동 멈춤
        owner.Agent.isStopped = true;
        owner.Agent.ResetPath();

        // Hit 애니메이션 재생
        owner.Animator.SetTrigger("isHit");

        // 스턴 코루틴 시작
        owner.StartCoroutine(HitStunCoroutine());
    }

    public override void OnUpdateState()
    {
        // 스턴 중에는 아무 행동도 하지 않음
    }

    public override void OnFixedUpdateState() { }

    public override void OnExitState()
    {
    }

    // 스턴 코루틴
    private IEnumerator HitStunCoroutine()
    {
        yield return new WaitForSeconds(hitDuration);

        // 스턴 종료 후 Chase 상태로 전환
        owner.ChangeState(ENEMY_STATE.Idle);
    }
}

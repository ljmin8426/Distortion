using UnityEngine;
using System.Collections;

public class MonsterHitState : BaseState<MonsterBase>
{
    private float hitDuration = 1f; // 스턴 시간

    public MonsterHitState(MonsterBase owner) : base(owner) { }

    public override void OnEnterState()
    {
        AudioManager.Instance.PlaySoundFXClip(owner.HitSoundClip, owner.transform, 1f);
        // 이동 멈춤
        owner.Agent.isStopped = true;
        owner.Agent.ResetPath();

        // Hit 애니메이션 재생
        owner.Animator.SetTrigger("isHit");

        // 스턴 코루틴 시작
        owner.StartCoroutine(HitStunCoroutine());
    }

    public override void OnUpdateState() { }

    public override void OnFixedUpdateState() { }

    public override void OnExitState() { }

    private IEnumerator HitStunCoroutine()
    {
        yield return new WaitForSeconds(hitDuration);

        owner.ChangeState(ENEMY_STATE.Idle);
    }
}

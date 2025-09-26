using UnityEngine;
using System.Collections;

public class MonsterHitState : BaseState<MonsterBase>
{
    public MonsterHitState(MonsterBase owner) : base(owner) { }

    public override void OnEnterState()
    {
        AudioManager.Instance.PlaySoundFXClip(owner.HitSoundClip, owner.transform, 1f);
     
        owner.Agent.isStopped = true;
        owner.Agent.ResetPath();

        owner.Animator.SetTrigger(owner.AnimHash_Hit);
        owner.StartCoroutine(HitStunCoroutine());
    }

    public override void OnUpdateState() { }

    public override void OnFixedUpdateState() { }

    public override void OnExitState()
    { 
        owner.StopAllCoroutines();
    }

    private IEnumerator HitStunCoroutine()
    {
        yield return YieldCache.WaitForSeconds(owner.StunTime);

        owner.ChangeState(Enemy_State.Idle);
    }
}

using UnityEngine;

public class MonsterDieState : BaseState<MonsterBase>
{
    private bool isDieTriggered = false;

    public MonsterDieState(MonsterBase owner) : base(owner) { }

    public override void OnEnterState()
    {
        owner.Agent.isStopped = true;   
        owner.Agent.ResetPath();        

        if (!isDieTriggered)
        {
            isDieTriggered = true;
            owner.Animator.SetTrigger(owner.AnimHash_Die);
            AudioManager.Instance.PlaySoundFXClip(owner.DeathSoundClip, owner.transform, 1f);
            PlayerStatManager.Instance.GetExp(owner.MonsterData.EXP);
        }

        owner.gameObject.layer = LayerMask.NameToLayer("DeadMonster");
    }

    public override void OnUpdateState() { }

    public override void OnFixedUpdateState() { }

    public override void OnExitState() { }
}

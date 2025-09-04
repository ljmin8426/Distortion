using Unity.VisualScripting;
using UnityEngine;

public class AttackState : BaseState<PlayerController>
{
    public AttackState(PlayerController controller) : base(controller) { }

    public override bool CanEnter()
    {
        // 대시 중에는 공격 불가
        if (owner.StateMachine.CurrentState is DashState)
            return false;

        return true;
    }

    public override void OnEnterState()
    {
        owner.LookAtCursor();

        switch (owner.CurrentWeaponType)
        {
            case WEAPON_TYPE.Melee:
                owner.Animator.SetTrigger("IsAttack");
                owner.Animator.SetFloat("AttackSpeed", PlayerStatManager.Instance.AG / 5);
                break;
                // 필요한 다른 무기 타입 처리 가능
        }

        owner.CurrentWeapon.Attack();
    }

    public override void OnUpdateState()
    {
        // 공격 상태 중 특수 처리 필요시 여기에 구현
    }

    public override void OnFixedUpdateState() { }

    public override void OnExitState() { }
}

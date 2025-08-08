using UnityEngine;
public class AttackState : BaseState<PlayerController>
{
    public AttackState(PlayerController controller) : base(controller) { }

    public override void OnEnterState()
    {
        controller.LookAtCursor();

        switch (controller.CurrentWeaponType)
        {
            case WEAPON_TYPE.Melee:
                controller.Animator.SetTrigger("IsAttack");
                controller.Animator.SetFloat("AttackSpeed", PlayerStatManager.Instance.AG / 5);
                break;
        }

        controller.CurrentWeapon.Attack();
    }

    public override void OnUpdateState() { }
    public override void OnFixedUpdateState() { }
    public override void OnExitState() { }
}

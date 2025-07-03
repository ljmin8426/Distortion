using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(PlayerController controller) : base(controller) { }

    public override void OnEnterState()
    {
        Controller.LookAtCursor();

        switch (Controller.CurrentWeaponType)
        {
            case WEAPON_TYPE.Melee:
                Controller.Animator.SetTrigger("IsAttack");
                break;
            case WEAPON_TYPE.Range:
                Controller.Animator.SetTrigger("IsFire");
                break;
        }

        Controller.TryAttack();  // 애니메이션 재생과 함께 발동
    }

    public override void OnUpdateState() { }
    public override void OnFixedUpdateState() { }
    public override void OnExitState() { }
}

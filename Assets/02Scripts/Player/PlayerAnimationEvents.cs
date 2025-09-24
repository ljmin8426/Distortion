using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerCtrl ctrl;

    private void Awake()
    {
        ctrl = GetComponentInParent<PlayerCtrl>();
    }

    public void AttackStart()
    {
        ctrl.WeaponManager.CurWeapon.AttackStart();
    }

    public void AttackEnd()
    {
        ctrl.WeaponManager.CurWeapon.AttackEnd();
    }

    public void CanMove()
    {
        ctrl.StateMachine.ChangeState(Player_State.Move);
    }
}

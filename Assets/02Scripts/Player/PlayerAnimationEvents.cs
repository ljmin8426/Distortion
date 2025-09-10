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

    public void FinishiMotion()
    {
        ctrl.StateMachine.ChangeState(PLAYER_STATE.Move);
    }

    public void OnDashEnd()
    {
        if (ctrl.StateMachine.CurrentState is DashState)
        {
             ctrl.StateMachine.ChangeState(PLAYER_STATE.Move);
        }
    }
}

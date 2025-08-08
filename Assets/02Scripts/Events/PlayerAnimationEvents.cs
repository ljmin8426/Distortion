using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerController controller;

    private void Awake()
    {
        controller = GetComponentInParent<PlayerController>();
    }

    public void StartAttack()
    {
        if (controller.CurrentWeapon is IMeleeWeapon meleeWeapon)
        {
            meleeWeapon.EnableMelee();
        }
    }

    public void EndAttack()
    {
        if (controller.CurrentWeapon is IMeleeWeapon meleeWeapon)
        {
            meleeWeapon.DisableMelee();
        }
    }

    public void FinishiMotion()
    {
        controller.StateMachine.ChangeState(PLAYER_STATE.Move);
    }

    public void OnDashEnd()
    {
        if (controller.StateMachine.CurrentState is DashState)
        {
             controller.StateMachine.ChangeState(PLAYER_STATE.Move);
        }
    }
}

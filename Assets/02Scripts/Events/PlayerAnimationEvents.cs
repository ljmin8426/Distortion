using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerController controller;

    private void Awake()
    {
        controller = GetComponentInParent<PlayerController>();
    }

    public void OnAttack()
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
        controller.StateMachine.ChangeState(PLAYER_STATE.Move);
    }
}

using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerController _controller;

    private void Awake()
    {
        _controller = GetComponentInParent<PlayerController>();
    }

    public void OnDash()
    {
        var dashState = _controller.StateMachine.GetState(PLAYER_STATE.Dash);
    }

    public void EndDash()
    {
        _controller.StateMachine.ChangeState(PLAYER_STATE.Move);
    }


    public void OnAttack()
    {
        if (_controller.CurrentWeapon is IMeleeWeapon meleeWeapon)
        {
            meleeWeapon.EnableMelee();
        }
    }

    public void EndAttack()
    {
        if (_controller.CurrentWeapon is IMeleeWeapon meleeWeapon)
        {
            meleeWeapon.DisableMelee();
        }
        _controller.StateMachine.ChangeState(PLAYER_STATE.Move);
    }

}
